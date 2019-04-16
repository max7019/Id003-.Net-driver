using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Id003Driver;
using Id003Driver.Events;

namespace Id003DriverTester
{
    public partial class Form1 : Form
    {
        private readonly Id003 _driver;
        
        public Form1()
        {
            InitializeComponent();

            _driver = new Id003();

            AttachDriverEvents();

            //SetInitState();
        }

        private void AttachDriverEvents()
        {
            _driver.Connection += OnDriverConnection;
            _driver.Connected += OnDriverConnected;
            _driver.Disconnection += OnDriverDisconnection;
            _driver.Disconnected += OnDriverDisconnected;
            _driver.PollingStarting += OnDriverPollingStarting;
            _driver.PollingStarted += OnDriverPollingStarted;
            _driver.PollingFinishing += OnDriverPollingFinishing;
            _driver.PollingFinished += OnDriverPollingFinished;
            _driver.AcceptanceStarting += OnDriverAcceptanceStarting;
            _driver.AcceptanceStarted += OnDriverAcceptanceStarted;
            _driver.AcceptanceFinishing += OnDriverAcceptanceFinishing;
            _driver.AcceptanceFinished += OnDriverAcceptanceFinished;
            _driver.StatusChanged += OnDriverStatusChanged;
            _driver.VendValidStatusDetected += OnDriverVendValidStatusDetected;
        }

        private void OnDriverVendValidStatusDetected(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                DoAction(() =>
                {
                    var result = _driver.Ack();

                    result.Wait();

                    ThrowIfError(result.Result);
                });
            }));
        }

        private void OnDriverStatusChanged(object sender, StatusChangedEventArgs statusChangedEventArgs)
        {
            var previousStatusDataText = 
                GetPoolingStatusDataText(
                    statusChangedEventArgs.PreviousStatus,
                    statusChangedEventArgs.PreviousStatusData);

            var currentStatusDataText =
                GetPoolingStatusDataText(
                    statusChangedEventArgs.CurrentStatus,
                    statusChangedEventArgs.CurrentStatusData);

            Invoke((MethodInvoker)(() =>
            {
                if (string.IsNullOrWhiteSpace(previousStatusDataText) && string.IsNullOrWhiteSpace(currentStatusDataText))
                    WriteToOutput(string.Format("Pooling status changed from '{0}' to '{1}'", statusChangedEventArgs.PreviousStatus, statusChangedEventArgs.CurrentStatus));
                else if (string.IsNullOrWhiteSpace(previousStatusDataText) && !string.IsNullOrWhiteSpace(currentStatusDataText))
                    WriteToOutput(string.Format("Pooling status changed from '{0}' to '{1}' [{2}]", statusChangedEventArgs.PreviousStatus, statusChangedEventArgs.CurrentStatus, currentStatusDataText));
                else if (!string.IsNullOrWhiteSpace(previousStatusDataText) && string.IsNullOrWhiteSpace(currentStatusDataText))
                    WriteToOutput(string.Format("Pooling status changed from '{0}' [{1}] to '{2}'", statusChangedEventArgs.PreviousStatus, previousStatusDataText, statusChangedEventArgs.CurrentStatus));
                else
                    WriteToOutput(string.Format("Pooling status changed from '{0}' [{1}] to '{2}' [{3}]", statusChangedEventArgs.PreviousStatus, previousStatusDataText, statusChangedEventArgs.CurrentStatus, currentStatusDataText));
            }));
        }

        private void OnDriverAcceptanceFinished(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Acceptance finished");
            }));
        }

        private void OnDriverAcceptanceFinishing(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Acceptance finishing...");
            }));
        }

        private void OnDriverAcceptanceStarted(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Acceptance started");
            }));
        }

        private void OnDriverAcceptanceStarting(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Acceptance starting...");
            }));
        }

        private void OnDriverPollingFinished(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Pooling finished");
            }));
        }

        private void OnDriverPollingFinishing(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Pooling finishing...");
            }));
        }

        private void OnDriverPollingStarted(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Pooling started");
            }));
        }

        private void OnDriverPollingStarting(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Pooling starting...");
            }));
        }

        private void OnDriverDisconnected(object sender, ErrorEventArgs errorEventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput(string.Format("Disconnected: {0}", errorEventArgs.Error));
            }));
        }

        private void OnDriverDisconnection(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Disconnection...");
            }));
        }

        private void OnDriverConnected(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Connected");
            }));
        }

        private void OnDriverConnection(object sender, EventArgs eventArgs)
        {
            Invoke((MethodInvoker)(() =>
            {
                WriteToOutput("Connection...");
            }));
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.Open((string)cbPorts.SelectedItem);

                ThrowIfError(result);

                await SetDenominations();

                WriteToOutput(string.Format("Port {0} opened", (string)cbPorts.SelectedItem));
            });
        }

        private void btnGetModelId_Click(object sender, EventArgs e)
        {
            DoAction(async () => 
            {
                var result = await _driver.GetModelId();

                ThrowIfError(result.Item1);
                
                WriteToOutput(string.Format("Model ID: {0}", result.Item2));
            });
        }

        private void DoAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (AggregateException exception)
            {
                WriteToOutput(exception.InnerException != null ? exception.InnerException.Message : exception.Message);
            }
            catch (Exception exception)
            {
                WriteToOutput(exception.Message);
            }
        }

        private void WriteToOutput(string text)
        {
            rtbOutput.AppendText(string.IsNullOrWhiteSpace(rtbOutput.Text) ? text : "\r\n" + text);
            rtbOutput.ScrollToCaret();
        }

        private static void ThrowIfError(Error error)
        {
            if (error != Error.NoError)
                throw new InvalidOperationException(string.Format("Operation error: {0}", error));
        }

        private void btnGetBootVersion_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.GetBootVersion();

                ThrowIfError(result.Item1);

                WriteToOutput(string.Format("Boot version: {0}", result.Item2));
            });
        }

        private void btnStartPooling_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                await _driver.StartPolling(TimeSpan.FromMilliseconds(100.0));
            });
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.Close();

                ThrowIfError(result);
            });
        }

        private void btnStopPooling_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                await _driver.StopPolling();
            });
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.Reset();

                ThrowIfError(result);
            });
        }

        private void btnGetStatus_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.ReadStatus();

                ThrowIfError(result);

                lock (_driver.StateLock)
                    WriteStatusAndDataToOutput();
            });
        }

        private void WriteStatusAndDataToOutput()
        {
            var statusDataText = GetStatusDataText();

            WriteToOutput(string.IsNullOrWhiteSpace(statusDataText)
                ? string.Format("Status '{0}'", _driver.Status)
                : string.Format("Status '{0}' [{1}]", _driver.Status, statusDataText));
        }

        private string GetStatusDataText()
        {
            switch (_driver.Status)
            {
                case Status.Escrow:
                    return GetEscrowStatusDataText(_driver.Denomination);
                case Status.Rejecting:
                    return GetRejectingStatusDataText(_driver.RejectingError);
                case Status.Failure:
                    return GetFailureStatusDataText(_driver.DeviceError);
                default:
                    return null;
            }
        }

        private string GetPoolingStatusDataText(Status status, object data)
        {
            switch (status)
            {
                case Status.Escrow:
                    return GetEscrowStatusDataText(data);
                case Status.Rejecting:
                    return GetRejectingStatusDataText(data);
                case Status.Failure:
                    return GetFailureStatusDataText(data);
                default:
                    return null;
            }
        }

        private string GetEscrowStatusDataText(object data)
        {
            return data == null 
                ? null 
                : string.Format("{0} {1}", _driver.Denomination.Value, ((Denomination) data).Currency);
        }

        private string GetRejectingStatusDataText(object data)
        {
            return ((RejectingError) data).ToString();
        }

        private string GetFailureStatusDataText(object data)
        {
            return ((DeviceError) data).ToString();
        }

        private Task SetDenominations()
        {
            const string currencyCode = "RUB";
            
            var denominationTable = new []
            {
                new Denomination(0, currencyCode),
                new Denomination(0, currencyCode),
                new Denomination(10, currencyCode),
                new Denomination(50, currencyCode),
                new Denomination(100, currencyCode),
                new Denomination(500, currencyCode),
                new Denomination(1000, currencyCode),
                new Denomination(5000, currencyCode)
            };

            return _driver.SetDenominationTable(denominationTable);
        }

        private void btnEnableAll_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clbDenominations.Items.Count; i++)
                clbDenominations.SetItemChecked(i, true);
        }

        private void btnDisableAll_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clbDenominations.Items.Count; i++)
                clbDenominations.SetItemChecked(i, false);
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            var checkedDenominations = new bool[clbDenominations.Items.Count];

            for (var i = 0; i < clbDenominations.Items.Count; i++)
                checkedDenominations[i] = clbDenominations.GetItemChecked(i);

            DoAction(async () =>
            {
                var result = await _driver.SetEnabledDenominations(checkedDenominations);

                ThrowIfError(result);
            });
        }

        private void btnEnableAcceptance_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.StartAcceptance();

                ThrowIfError(result.Item1);
            });
        }

        private void btnDisableAcceptance_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.StopAcceptance();

                ThrowIfError(result.Item1);
            });
        }

        private void btnStack_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.Stack1();

                ThrowIfError(result);
            });
        }

        private void btnStack2_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.Stack2();

                ThrowIfError(result);
            });
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.Return();

                ThrowIfError(result);
            });
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            DoAction(async () =>
            {
                var result = await _driver.Hold();

                ThrowIfError(result);
            });
        }
    }
}
