using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Id003Driver.Events;
using Id003Driver.Extensions;
using WBALib;

namespace Id003Driver
{
    public class Id003
    {
        private const int DenominationTableSize = 8;

        private volatile bool _disposed;

        private WBADrv2 _driver;

        private Task _pollingTask;
        private Task _eventRaisingTask;
        
        private Task<Error> _openingTask;
        private Task<Error> _closingTask;

        private Task _pollingStartingTask;
        private Task _pollingFinishingTask;

        private Task<Tuple<Error, bool>> _acceptanceStartingTask;
        private Task<Tuple<Error, bool>> _acceptanceFinishingTask;

        private BlockingCollection<Tuple<Status, object, Status, object>> _statusUpdates;

        private Denomination[] _denominationTable;

        private CancellationTokenSource _pollingCancellationTokenSource;
        private CancellationToken _pollingCancellationToken;

        public event EventHandler Connection;
        public event EventHandler Connected;
        
        public event EventHandler Disconnection;
        public event EventHandler<ErrorEventArgs> Disconnected;

        public event EventHandler PollingStarting;
        public event EventHandler PollingStarted;

        public event EventHandler PollingFinishing;
        public event EventHandler PollingFinished;

        public event EventHandler AcceptanceStarting;
        public event EventHandler AcceptanceStarted;

        public event EventHandler AcceptanceFinishing;
        public event EventHandler AcceptanceFinished;

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        public event EventHandler IdlingStatusDetected;
        public event EventHandler IdlingStatusCleared;

        public event EventHandler AcceptingStatusDetected;
        public event EventHandler AcceptingStatusCleared;

        public event EventHandler<DenominationEventArgs> EscrowStatusDetected;
        public event EventHandler<DenominationEventArgs> EscrowStatusCleared;

        public event EventHandler StackingStatusDetected;
        public event EventHandler StackingStatusCleared;

        public event EventHandler VendValidStatusDetected;
        public event EventHandler VendValidStatusCleared;

        public event EventHandler StackedStatusDetected;
        public event EventHandler StackedStatusCleared;

        public event EventHandler<RejectingEventArgs> RejectingStatusDetected;
        public event EventHandler<RejectingEventArgs> RejectingStatusCleared;

        public event EventHandler ReturningStatusDetected;
        public event EventHandler ReturningStatusCleared;

        public event EventHandler HoldingStatusDetected;
        public event EventHandler HoldingStatusCleared;

        public event EventHandler InhibitStatusDetected;
        public event EventHandler InhibitStatusCleared;

        public event EventHandler InitializeStatusDetected;
        public event EventHandler InitializeStatusCleared;

        public event EventHandler PowerUpStatusDetected;
        public event EventHandler PowerUpStatusCleared;

        public event EventHandler PowerUpWithBillInAcceptorStatusDetected;
        public event EventHandler PowerUpWithBillInAcceptorStatusCleared;

        public event EventHandler PowerUpWithBillInStackerStatusDetected;
        public event EventHandler PowerUpWithBillInStackerStatusCleared;

        public event EventHandler StackerFullStatusDetected;
        public event EventHandler StackerFullStatusCleared;

        public event EventHandler StackerOpenStatusDetected;
        public event EventHandler StackerOpenStatusCleared;

        public event EventHandler JamInAcceptorStatusDetected;
        public event EventHandler JamInAcceptorStatusCleared;

        public event EventHandler JamInStackerStatusDetected;
        public event EventHandler JamInStackerStatusCleared;

        public event EventHandler PauseStatusDetected;
        public event EventHandler PauseStatusCleared;

        public event EventHandler CheatedStatusDetected;
        public event EventHandler CheatedStatusCleared;

        public event EventHandler<DeviceErrorEventArgs> FailureStatusDetected;
        public event EventHandler<DeviceErrorEventArgs> FailureStatusCleared;

        public event EventHandler CommunicationErrorStatusDetected;
        public event EventHandler CommunicationErrorStatusCleared;

        #region StateLock

        public object StateLock { get; } = new object();

        #endregion //StateLock

        #region PortName

        private string _portName;

        public string PortName
        {
            get
            {
                return F(() => _portName);
            }
        }

        #endregion //PortName

        #region IsOpening

        private bool _isOpening;

        public bool IsOpening
        {
            get
            {
                return F(() => _isOpening);
            }
        }

        #endregion //IsOpening

        #region IsOpened

        private bool _isOpened;

        public bool IsOpened
        {
            get
            {
                return F(() => _isOpened);
            }
        }

        #endregion //IsOpened

        #region IsClosing

        private bool _isClosing;

        public bool IsClosing
        {
            get
            {
                return F(() => _isClosing);
            }
        }

        #endregion //IsClosing

        #region Status

        private Status _currentStatus;

        public Status Status
        {
            get
            {
                return F(() => _currentStatus);
            }
        }

        #endregion //Status

        #region IsPollingStarting

        private bool _isPollingStarting;

        public bool IsPollingStarting
        {
            get
            {
                return F(() => _isPollingStarting);
            }
        }

        #endregion //IsPollingStarting

        #region IsPollingStarted

        private bool _isPollingStarted;

        public bool IsPollingStarted
        {
            get
            {
                return F(() => _isPollingStarted);
            }
        }

        #endregion //IsPollingStarted

        #region IsPollingFinishing

        private bool _isPollingFinishing;

        public bool IsPollingFinishing
        {
            get
            {
                return F(() => _isPollingFinishing);
            }
        }

        #endregion //IsPollingFinishing

        #region IsEscrow

        private bool _isEscrow;

        public bool IsEscrow
        {
            get
            {
                return F(() => _isEscrow);
            }
        }

        #endregion //IsEscrow

        #region Denomination

        private Denomination _denomination;

        public Denomination Denomination
        {
            get
            {
                return F(() => _denomination);
            }
        }

        #endregion //Denomination

        #region RejectingError

        private RejectingError _rejectingError;

        public RejectingError RejectingError
        {
            get
            {
                return F(() => _rejectingError);
            }
        }

        #endregion //RejectingError

        #region DeviceError

        private DeviceError _deviceError;

        public DeviceError DeviceError
        {
            get
            {
                return F(() => _deviceError);
            }
        }

        #endregion //DeviceError

        #region IsAcceptanceStarting

        private bool _isAcceptanceStarting;

        public bool IsAcceptanceStarting
        {
            get
            {
                return F(() => _isAcceptanceStarting);
            }
        }

        #endregion //IsAcceptanceStarting

        #region IsAcceptanceStarted

        private bool _isAcceptanceStarted;

        public bool IsAcceptanceStarted
        {
            get
            {
                return F(() => _isAcceptanceStarted);
            }
        }

        #endregion //IsAcceptanceStarted

        #region IsAcceptanceFinishing

        private bool _isAcceptanceFinishing;

        public bool IsAcceptanceFinishing
        {
            get
            {
                return F(() => _isAcceptanceFinishing);
            }
        }

        #endregion //IsAcceptanceFinishing
        
        public Id003()
        {
            var wbaDrv2ClassId = new Guid("7EB7310A-A4A9-41E9-93D1-4A3B76B73F11");
            var classFactoryIid = new Guid("00000001-0000-0000-C000-000000000046");

            object classFactoryObject = null;
            
            var hResult = DllGetClassObject(wbaDrv2ClassId, classFactoryIid, ref classFactoryObject);

            if (hResult < 0)
                Marshal.ThrowExceptionForHR(hResult);

            try
            {
                var classFactory = classFactoryObject as IClassFactory;

                if (classFactory == null)
                    throw new NullReferenceException($"'{nameof(classFactory)}' is null!");

                var wbaDrv2Iid = new Guid("D21EA04D-54D2-407B-8762-1F8F2C37B4DD");

                object wbaDrv2Object = null;

                hResult = classFactory.CreateInstance(null, wbaDrv2Iid, ref wbaDrv2Object);

                if (hResult < 0)
                    Marshal.ThrowExceptionForHR(hResult);

                _driver = wbaDrv2Object as WBADrv2;

                if (_driver != null || wbaDrv2Object == null || !Marshal.IsComObject(wbaDrv2Object)) 
                    return;
                
                try
                {
                    Marshal.FinalReleaseComObject(wbaDrv2Object);
                }
                catch
                {
                    // ignored
                }
            }
            finally
            {
                if (classFactoryObject != null && Marshal.IsComObject(classFactoryObject))
                {
                    try
                    {
                        Marshal.FinalReleaseComObject(classFactoryObject);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        ~Id003()
        {
            Dispose(false);
        }

        public Task<Error> Open(string portName)
        {
            return F(() =>
            {
                if (_openingTask != null)
                    return _openingTask;

                _openingTask = Task.Run(() =>
                {
                    A(ThrowIfClosing);
                    
                    if (F(() => _isOpened))
                        return Error.NoError;
                    
                    if (string.IsNullOrWhiteSpace(portName))
                        throw new ArgumentException("Invalid port name!");

                    A(() =>
                    {
                        _isOpening = true;

                        _portName = portName;
                    });

                    Connection.SafeRaiseEvent(this);

                    try
                    {
                        A(() => _driver.PortNumber = Utils.ExtractPortNumber(_portName));

                        var isOk = false;
                        var error = Error.Unknown;

                        A(() =>
                        {
                            isOk = _driver.OpenPort();
                            error = ErrorUtils.IntToError(_driver.ResultCode);
                        });
                        
                        if (!isOk || error != Error.NoError)
                        {
                            Disconnected.SafeRaiseEvent(this, new ErrorEventArgs(error));
                            
                            return error;
                        }

                        A(() =>
                        {
                            isOk = _driver.ReadStatus();
                            error = ErrorUtils.IntToError(_driver.ResultCode);
                        });
                        
                        if (!isOk || error != Error.NoError)
                        {
                            Disconnected.SafeRaiseEvent(this, new ErrorEventArgs(error));
                            
                            return error;
                        }

                        A(() =>
                        {
                            DoChangeAcceptanceStatus(false);

                            _isOpened = true;
                        });
                        
                        Connected.SafeRaiseEvent(this);

                        return Error.NoError;
                    }
                    catch
                    {
                        SafeA(() =>
                        {
                            _driver.ClosePort();

                            _portName = null;
                        });

                        const Error error = Error.Unknown;
                        
                        Disconnected.SafeRaiseEvent(this, new ErrorEventArgs(error));
                            
                        return error;
                    }
                    finally
                    {
                        SafeA(() =>
                        {
                            _isOpening = false;

                            _openingTask = null;
                        });
                    }
                }, 
                CancellationToken.None);

                return _openingTask;
            });
            
        }

        public Task<Error> Close()
        {
            return F(() =>
            {
                if (_closingTask != null)
                    return _closingTask;

                _closingTask = Task.Run(async () =>
                {
                    A(ThrowIfOpening);
                    
                    if (F(() => !_isOpened))
                        return Error.NoError;
                    
                    A(() => _isClosing = true);

                    Disconnection.SafeRaiseEvent(this);

                    try
                    {
                        try
                        {
                            await StopPolling().ConfigureAwait(false);
                        }
                        catch
                        {
                            // ignored
                        }

                        var error = Error.Unknown;

                        A(() =>
                        {
                            _driver.ClosePort();

                            error = ErrorUtils.IntToError(_driver.ResultCode);
                        });
                    
                        Disconnected.SafeRaiseEvent(this, new ErrorEventArgs(error));

                        return error;
                    }
                    catch
                    {
                        const Error error = Error.Unknown;

                        Disconnected.SafeRaiseEvent(this, new ErrorEventArgs(error));

                        return error;
                    }
                    finally
                    {
                        SafeA(() =>
                        {
                            _isClosing = false;
                            _isOpened = false;

                            _portName = null;

                            _closingTask = null;
                        });
                    }
                },
                CancellationToken.None);
                
                return _closingTask;
            });
        }

        public Task StartPolling()
        {
            return StartPolling(TimeSpan.FromMilliseconds(100.0));
        }

        public Task StartPolling(TimeSpan poolingInterval)
        {
            return F(() =>
            {
                if (_pollingStartingTask != null)
                    return _pollingStartingTask;

                _pollingStartingTask = Task.Run(() =>
                {
                    try
                    {
                        A(() =>
                        {
                            ThrowIfPoolingFinishing();
                            ThrowIfNotOpened();
                        });
                        
                        if (F(() => _isPollingStarted))
                            return;
                        
                        A(() =>
                        {
                            _currentStatus = Status.None;

                            _isPollingStarting = true;
                        });
                    
                        PollingStarting.SafeRaiseEvent(this);

                        A(() =>
                        {
                            _statusUpdates = new BlockingCollection<Tuple<Status, object, Status, object>>();

                            _pollingCancellationTokenSource = new CancellationTokenSource();
                            _pollingCancellationToken = _pollingCancellationTokenSource.Token;

                            _eventRaisingTask = Task.Factory.StartNew(
                                RaiseStatusChangedEvents,
                                _pollingCancellationToken,
                                TaskCreationOptions.LongRunning,
                                TaskScheduler.Default);

                            _pollingTask = Task.Factory.StartNew(
                                () => DoPolling(poolingInterval),
                                _pollingCancellationToken,
                                TaskCreationOptions.LongRunning,
                                TaskScheduler.Default);

                            _isPollingStarted = true;
                        });

                        PollingStarted.SafeRaiseEvent(this);
                    }
                    catch
                    {
                        SafeA(() => _pollingCancellationTokenSource?.Cancel());

                        WaitPollingTasks();

                        SafeA(FreePollingTaskResources);
                    }
                    finally
                    {
                        SafeA(() =>
                        {
                            _isPollingStarting = false;

                            _pollingStartingTask = null;
                        });
                    }
                }, CancellationToken.None);

                return _pollingStartingTask;
            });
        }

        public Task StopPolling()
        {
            return F(() =>
            {
                if (_pollingFinishingTask != null)
                    return _pollingFinishingTask;

                _pollingFinishingTask = Task.Run(async () =>
                {
                    var isPoolingFinishingFired = false;
                
                    try
                    {
                        A(ThrowIfPoolingStarting);

                        if (F(() => !_isPollingStarted))
                            return;
                        
                        A(() => _isPollingFinishing = true);

                        try
                        {
                            await StopAcceptance().ConfigureAwait(false);
                        }
                        catch
                        {
                            // ignored
                        }

                        isPoolingFinishingFired = true; 
                    
                        PollingFinishing.SafeRaiseEvent(this);

                        A(() => _pollingCancellationTokenSource.Cancel());

                        WaitPollingTasks();
                    }
                    finally
                    {
                        SafeA(() =>
                        {
                            FreePollingTaskResources();

                            _isPollingStarted = false;
                            _isPollingFinishing = false;
                        });

                        if (isPoolingFinishingFired)
                            PollingFinished.SafeRaiseEvent(this);
                    }
                }, 
                CancellationToken.None);

                return _pollingFinishingTask;
            });
        }

        public Task SetDenominationTable(IReadOnlyCollection<Denomination> denominationTable)
        {
            if (denominationTable == null)
                throw new ArgumentNullException(nameof(denominationTable));

            if (denominationTable.Count != DenominationTableSize)
                throw new ArgumentException($"Incorrect denomination table size ({denominationTable.Count})!");

            return Task.Run(() =>
            {
                A(() =>
                {
                    ThrowIfNotOpened();
                    ThrowIfClosing();

                    _denominationTable = denominationTable.ToArray();
                });
            }, 
            CancellationToken.None);
        }

        public Task<IReadOnlyCollection<Denomination>> GetDenominationTable()
        {
            return Task.Run(() =>
            {
                LinkedList<Denomination> result = null;

                A(() =>
                {
                    result = _denominationTable == null 
                        ? null 
                        : new LinkedList<Denomination>(_denominationTable);
                });

                return (IReadOnlyCollection<Denomination>) result.ToList().AsReadOnly();
            },
            CancellationToken.None);
        }

        public Task<Tuple<Error, bool>> StartAcceptance()
        {
            return F(() =>
            {
                if (_acceptanceStartingTask != null)
                    return _acceptanceStartingTask;

                _acceptanceStartingTask = Task.Run(() =>
                {
                    try
                    {
                        A(() =>
                        {
                            ThrowIfPoolingNotStarted();
                            ThrowIfPoolingFinishing();
                            ThrowIfAcceptanceFinishing();
                            ThrowIfIncorrectDenominationTable();

                            _isAcceptanceStarting = true;
                        });

                        AcceptanceStarting.SafeRaiseEvent(this);

                        var isOk = false;
                        var error = Error.Unknown;

                        A(() =>
                        {
                            isOk = DoChangeAcceptanceStatus(true);

                            error = ErrorUtils.IntToError(_driver.ResultCode);
                        });

                        if (!isOk || error != Error.NoError)
                            return Tuple.Create(error, isOk);

                        A(() => _isAcceptanceStarted = true);

                        AcceptanceStarted.SafeRaiseEvent(this);

                        return Tuple.Create(error, isOk);
                    }
                    finally
                    {
                        SafeA(() =>
                        {
                            _isAcceptanceStarting = false;

                            _acceptanceStartingTask = null;
                        });
                    }
                }, CancellationToken.None);

                return _acceptanceStartingTask;
            });
        }

        public Task<Tuple<Error, bool>> StopAcceptance()
        {
            return F(() =>
            {
                if (_acceptanceFinishingTask != null)
                    return _acceptanceFinishingTask;

                _acceptanceFinishingTask = Task.Run(() =>
                {
                    var isAcceptanceFinishingFired = false;

                    try
                    {
                        A(ThrowIfAcceptanceStarting);
                        
                        if (!F(() => _isAcceptanceStarted))
                            return Tuple.Create(Error.NoError, true);

                        A(() =>
                        {
                            _isAcceptanceFinishing = true;
                        });

                        isAcceptanceFinishingFired = true;

                        AcceptanceFinishing.SafeRaiseEvent(this);

                        var isOk = false;
                        var error = Error.Unknown;

                        A(() =>
                        {
                            isOk = DoChangeAcceptanceStatus(false);

                            error = ErrorUtils.IntToError(_driver.ResultCode);
                        });

                        return Tuple.Create(error, isOk);
                    }
                    finally
                    {
                        SafeA(() =>
                        {
                            _isAcceptanceStarted = false;
                            _isAcceptanceFinishing = false;

                            _acceptanceFinishingTask = null;
                        });

                        if (isAcceptanceFinishingFired)
                            AcceptanceFinished.SafeRaiseEvent(this);
                    }
                }, CancellationToken.None);

                return _acceptanceFinishingTask;
            });
        }

        public Task<Error> Stack1()
        {
            return EscrowAction(() => _driver.Stack1());
        }

        public Task<Error> Stack2()
        {
            return EscrowAction(() => _driver.Stack2());
        }

        public Task<Error> Return()
        {
            return EscrowAction(() => _driver.Return());
        }

        public Task<Error> Hold()
        {
            return EscrowAction(() => _driver.Hold());
        }

        private Task<Error> EscrowAction(Action stackAction)
        {
            return Task.Run(() =>
            {
                var error = Error.Unknown;
                
                A(() =>
                {
                    ThrowIfNotEscrowOrHoldingStatus();

                    stackAction.Invoke();

                    error = ErrorUtils.IntToError(_driver.ResultCode);
                });

                return error;

            }, CancellationToken.None);
        }

        public Task<Error> Ack()
        {
            return Task.Run(() =>
            {
                var error = Error.Unknown;
                
                A(() =>
                {
                    ThrowIfNotVendValidStatus();

                    _driver.Ack();

                    error = ErrorUtils.IntToError(_driver.ResultCode);
                });

                return error;

            }, CancellationToken.None);
        }

        public Task<Error> Reset()
        {
            return Task.Run(() =>
            {
                var error = Error.Unknown;
                
                A(() =>
                {
                    ThrowIfNotOpened();
                    ThrowIfClosing();

                    _driver.Reset();

                    error = ErrorUtils.IntToError(_driver.ResultCode);
                });

                return error;

            }, CancellationToken.None);
        }

        public Task<Error> ReadStatus()
        {
            return Task.Run(() =>
            {
                var error = Error.Unknown;

                A(() =>
                {
                    ThrowIfNotOpened();
                    ThrowIfClosing();

                    var status = Status.None;

                    if (_driver.ReadStatus() && (error = ErrorUtils.IntToError(_driver.ResultCode)) == Error.NoError)
                        status = StatusUtils.IntToStatus(_driver.StatusCode);

                    SetPreviousStatusData(_currentStatus);
                    SetCurrentStatusData(status);

                    _currentStatus = status;
                });

                return error;

            }, CancellationToken.None);
        }

        public Task<Tuple<Error, string>> GetModelId()
        {
            return Task.Run(() =>
            {
                var error = Error.Unknown;
                string modelId = null;
                
                A(() =>
                {
                    ThrowIfNotOpened();
                    ThrowIfClosing();

                    _driver.ReadModelID();

                    error = ErrorUtils.IntToError(_driver.ResultCode);
                    modelId = error == Error.NoError ? _driver.ModelID : null;
                });

                return Tuple.Create(error, modelId);

            }, CancellationToken.None);
        }

        public Task<Tuple<Error, string>> GetBootVersion()
        {
            return Task.Run(() =>
            {
                var error = Error.Unknown;
                string bootVersion = null;
                
                A(() =>
                {
                    ThrowIfNotOpened();
                    ThrowIfClosing();

                    _driver.ReadBootVersion();

                    error = ErrorUtils.IntToError(_driver.ResultCode);
                    bootVersion = error == Error.NoError ? _driver.BootVersion : null;
                });

                return Tuple.Create(error, bootVersion);

            }, CancellationToken.None);
        }

        public Task<Error> SetValidatedDenominations(bool[] selectors)
        {
            return SetDenominationsAction(selectors, () => _driver.WriteValidatedDenominations());
        }

        public Task<Tuple<Error, bool[]>> GetValidatedDenominations()
        {
            return GetDenominationsAction(() => _driver.ReadValidatedDenominations());
        }

        public Task<Error> SetEnabledDenominations(bool[] selectors)
        {
            return SetDenominationsAction(selectors, () => _driver.WriteEnabledDenominations());
        }

        public Task<Tuple<Error, bool[]>> GetEnabledDenominations()
        {
            return GetDenominationsAction(() => _driver.ReadEnabledDenominations());
        }

        private Task<Error> SetDenominationsAction(bool[] selectors, Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            
            if (selectors == null)
                throw new ArgumentNullException(nameof(selectors));

            if (selectors.Length != DenominationTableSize)
                throw new ArgumentException($"Incorrect selectors count ({selectors.Length})!");

            return Task.Run(() =>
            {
                var error = Error.Unknown;

                A(() =>
                {
                    ThrowIfNotOpened();
                    ThrowIfClosing();

                    _driver.Denomination1 = selectors[0];
                    _driver.Denomination2 = selectors[1];
                    _driver.Denomination3 = selectors[2];
                    _driver.Denomination4 = selectors[3];
                    _driver.Denomination5 = selectors[4];
                    _driver.Denomination6 = selectors[5];
                    _driver.Denomination7 = selectors[6];
                    _driver.Denomination8 = selectors[7];

                    action.Invoke();

                    error = ErrorUtils.IntToError(_driver.ResultCode);
                });

                return error;

            }, CancellationToken.None);
        }

        private Task<Tuple<Error, bool[]>> GetDenominationsAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return Task<Tuple<Error, bool[]>>.Factory.StartNew(() =>
            {
                var error = Error.Unknown;
                bool[] selectors = null;

                A(() =>
                {
                    ThrowIfNotOpened();
                    ThrowIfClosing();

                    action.Invoke();

                    error = ErrorUtils.IntToError(_driver.ResultCode);

                    if (error == Error.NoError)
                    {
                        selectors = new[]
                        {
                            _driver.Denomination1,
                            _driver.Denomination2,
                            _driver.Denomination3,
                            _driver.Denomination4,
                            _driver.Denomination5,
                            _driver.Denomination6,
                            _driver.Denomination7,
                            _driver.Denomination8
                        };
                    }

                });

                return Tuple.Create(error, selectors);

            }, CancellationToken.None);
        }

        private void ThrowIfNotOpened()
        {
            if (!_isOpened)
                throw new InvalidOperationException("Device do not opened!");
        }

        private void ThrowIfOpening()
        {
            if (_isOpening)
                throw new InvalidOperationException("Device opening in progress!");
        }

        private void ThrowIfClosing()
        {
            if (_isClosing)
                throw new InvalidOperationException("Device closing in progress!");
        }

        private void ThrowIfPoolingNotStarted()
        {
            if (!_isPollingStarted)
                throw new InvalidOperationException("Pooling do not started!");
        }

        private void ThrowIfPoolingStarting()
        {
            if (_isPollingStarting)
                throw new InvalidOperationException("Pooling starting in progress!");
        }

        private void ThrowIfPoolingFinishing()
        {
            if (_isPollingFinishing)
                throw new InvalidOperationException("Pooling finishing in progress!");
        }

        private void ThrowIfAcceptanceStarting()
        {
            if (_isAcceptanceStarting)
                throw new InvalidOperationException("Acceptance starting in progress!");
        }

        private void ThrowIfAcceptanceFinishing()
        {
            if (_isAcceptanceFinishing)
                throw new InvalidOperationException("Acceptance finishing in progress!");
        }

        private void ThrowIfIncorrectDenominationTable()
        {
            if (_denominationTable == null)
                throw new InvalidOperationException("Incorrect denomination table!");
        }

        private void ThrowIfNotEscrowOrHoldingStatus()
        {
            if (_currentStatus != Status.Escrow && _currentStatus != Status.Holding)
                throw new InvalidOperationException("Do not escrow or holding status!");
        }

        private void ThrowIfNotVendValidStatus()
        {
            if (_currentStatus != Status.VendValid)
                throw new InvalidOperationException("Do not vend valid status!");
        }

        public void Dispose()
        {
            lock (StateLock)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    if (!_driver.InhibitStatus)
                        DoChangeAcceptanceStatus(false);

                    FreeAcceptanceResources();

                    _pollingCancellationTokenSource.Cancel();
                
                    FreePollingTaskResources();
                }
                catch
                {
                    // ignored
                }
            }

            if (_driver != null && Marshal.IsComObject(_driver))
            {
                try
                {
                    Marshal.FinalReleaseComObject(_driver);
                }
                catch
                {
                    // ignored
                }

                _driver = null;
            }

            _disposed = true;
        }

        private bool DoChangeAcceptanceStatus(bool isStart)
        {
            _driver.InhibitStatus = !isStart;

            return _driver.WriteInhibitStatus();
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("Object already disposed!");
        }

        private void FreeAcceptanceResources()
        {
            _isAcceptanceStarting = false;
            _isAcceptanceStarted = false;
            _isAcceptanceFinishing = false;

            _denominationTable = null;
        }

        private void FreePollingTaskResources()
        {
            _pollingCancellationTokenSource?.Dispose();
            _pollingCancellationTokenSource = null;

            _pollingCancellationToken = CancellationToken.None;

            _pollingTask = null;
            _eventRaisingTask = null;

            _statusUpdates?.Dispose();
            _statusUpdates = null;

            _currentStatus = Status.None;
        }

        private void WaitPollingTasks()
        {
            var timeout = TimeSpan.FromSeconds(10.0);
            var timer = TimeSpan.Zero;

            while (timer.Milliseconds < timeout.Milliseconds)
            {
                var operationTimeout = TimeSpan.FromMilliseconds(100.0);

                if (F(() => Task.WaitAll(new []{_pollingTask, _eventRaisingTask}, operationTimeout)))
                    break;

                timer += operationTimeout;
            }
        }

        private void DoPolling(TimeSpan poolingInterval)
        {
            while (true)
            {
                try
                {
                    A(() =>
                    {
                        _pollingCancellationToken.ThrowIfCancellationRequested();

                        ProcessPolling();
                    });

                    Thread.Sleep(poolingInterval);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    try
                    {
                        StopPolling();
                    }
                    catch
                    {
                        // ignored
                    }

                    break;
                }
            }
        }

        private void ProcessPolling()
        {
            var status = Status.None;

            if (_driver.ReadStatus() && ErrorUtils.IntToError(_driver.ResultCode) == Error.NoError)
                status = StatusUtils.IntToStatus(_driver.StatusCode);

            var previousStatusData = SetPreviousStatusData(_currentStatus);
            var currentStatusData = SetCurrentStatusData(status);

            if (_currentStatus == status)
            {
                if (previousStatusData == null && currentStatusData == null)
                    return;

                if (previousStatusData != null && currentStatusData == null)
                    return;

                if (currentStatusData.Equals(previousStatusData))
                    return;
            }

            var previousStatus = _currentStatus;

            _currentStatus = status;

            _statusUpdates.Add(
                Tuple.Create(
                    previousStatus, 
                    previousStatusData, 
                    _currentStatus, 
                    currentStatusData), 
                _pollingCancellationToken);

            if (CheckAndProcessNoneStatus(_currentStatus))
                return;
            
            CheckAndProcessNonAcceptanceStatus(_currentStatus);
        }

        private bool CheckAndProcessNoneStatus(Status status)
        {
            if (!_isOpened || _isClosing || status != Status.None)
                return false;
            
            Close();

            return true;
        }

        private void CheckAndProcessNonAcceptanceStatus(Status status)
        {
            if (!_isAcceptanceStarted || _isAcceptanceFinishing || StatusUtils.IsAcceptanceStatus(status))
                return;

            StopAcceptance();
        }

        private object SetPreviousStatusData(Status previousStatus)
        {
            switch (previousStatus)
            {
                case Status.Escrow:
                    return SetPreviousEscrowStatusData();
                case Status.Rejecting:
                    return SetPreviousRejectingStatusData();
                case Status.Failure:
                    return SetPreviousFailureStatusData();
                default:
                    return null;
            }
        }

        private object SetCurrentStatusData(Status currentStatus)
        {
            switch (currentStatus)
            {
                case Status.Escrow:
                    return SetCurrentEscrowStatusData();
                case Status.Rejecting:
                    return SetCurrentRejectingStatusData();
                case Status.Failure:
                    return SetCurrentFailureStatusData();
                default:
                    return null;
            }
        }

        private Denomination SetCurrentEscrowStatusData()
        {
            _isEscrow = true;

            int denominationIndex;

            try
            {
                denominationIndex = DenominationUtils.GetDenominationPureIndex(_driver.Denomination);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException($"Incorrect denomination value '{_driver.Denomination}'!");
            }
            
            _denomination = _denominationTable?[denominationIndex];

            return _denomination;
        }

        private RejectingError SetCurrentRejectingStatusData()
        {
            _rejectingError = RejectingErrorUtils.IntToRejectingError(_driver.RejectionCode);

            return _rejectingError;
        }

        private DeviceError SetCurrentFailureStatusData()
        {
            _deviceError = DeviceErrorUtils.IntToDeviceError(_driver.FailureCode);

            return _deviceError;
        }

        private Denomination SetPreviousEscrowStatusData()
        {
            _isEscrow = false;

            var result = _denomination;
            
            _denomination = null;

            return result;
        }

        private RejectingError SetPreviousRejectingStatusData()
        {
            var result = _rejectingError;
            
            _rejectingError = RejectingError.Unknown;

            return result;
        }

        private DeviceError SetPreviousFailureStatusData()
        {
            var result = _deviceError;
            
            _deviceError = DeviceError.Unknown;

            return result;
        }

        private void RaiseStatusChangedEvents()
        {
            while (true)
            {
                try
                {
                    BlockingCollection<Tuple<Status, object, Status, object>> statusUpdates = null;
                    
                    var pollingCancellationToken = CancellationToken.None;
                    
                    A(() =>
                    {
                        statusUpdates = _statusUpdates;

                        pollingCancellationToken = _pollingCancellationToken;
                    });
                        
                    if (pollingCancellationToken == CancellationToken.None)
                        throw new OperationCanceledException();

                    _pollingCancellationToken.ThrowIfCancellationRequested();
                    
                    var statuses = statusUpdates.Take(F(() => _pollingCancellationToken));

                    DoRaiseStatusChangedEvents(statuses.Item1, statuses.Item2, statuses.Item3, statuses.Item4);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    // ignored
                }
            }            
        }

        private void DoRaiseStatusChangedEvents(Status previousStatus, object previousStatusData, Status currentStatus, object currentStatusData)
        {
            StatusChanged.SafeRaiseEvent(
                this, 
                new StatusChangedEventArgs(previousStatus, previousStatusData, currentStatus, currentStatusData));

            DoRaiseStatusClearedEvent(previousStatus, previousStatusData);
            DoRaiseStatusDetectedEvent(currentStatus, currentStatusData);
        }

        private void DoRaiseStatusClearedEvent(Status status, object statusData)
        {
            switch (status)
            {
                case Status.Idling:
                    IdlingStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Accepting:
                    AcceptingStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Escrow:
                    DoEscrowStatusCleared(statusData);
                    break;
                case Status.Stacking:
                    StackingStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.VendValid:
                    VendValidStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Stacked:
                    StackedStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Rejecting:
                    DoRejectingStatusCleared(statusData);
                    break;
                case Status.Returning:
                    ReturningStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Holding:
                    HoldingStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Inhibit:
                    InhibitStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Initialize:
                    InitializeStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.PowerUp:
                    PowerUpStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.PowerUpWithBillInAcceptor:
                    PowerUpWithBillInAcceptorStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.PowerUpWithBillInStacker:
                    PowerUpWithBillInStackerStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.StackerFull:
                    StackerFullStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.StackerOpen:
                    StackerOpenStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.JamInAcceptor:
                    JamInAcceptorStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.JamInStacker:
                    JamInStackerStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Pause:
                    PauseStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Cheated:
                    CheatedStatusCleared.SafeRaiseEvent(this);
                    break;
                case Status.Failure:
                    DoFailureStatusCleared(statusData);
                    break;
                case Status.CommunicationError:
                    CommunicationErrorStatusCleared.SafeRaiseEvent(this);
                    break;
            }
        }

        private void DoRaiseStatusDetectedEvent(Status status, object statusData)
        {
            switch (status)
            {
                case Status.Idling:
                    IdlingStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Accepting:
                    AcceptingStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Escrow:
                    DoEscrowStatusDetected(statusData);
                    break;
                case Status.Stacking:
                    StackingStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.VendValid:
                    VendValidStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Stacked:
                    StackedStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Rejecting:
                    DoRejectingStatusDetected(statusData);
                    break;
                case Status.Returning:
                    ReturningStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Holding:
                    HoldingStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Inhibit:
                    InhibitStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Initialize:
                    InitializeStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.PowerUp:
                    PowerUpStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.PowerUpWithBillInAcceptor:
                    PowerUpWithBillInAcceptorStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.PowerUpWithBillInStacker:
                    PowerUpWithBillInStackerStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.StackerFull:
                    StackerFullStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.StackerOpen:
                    StackerOpenStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.JamInAcceptor:
                    JamInAcceptorStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.JamInStacker:
                    JamInStackerStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Pause:
                    PauseStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Cheated:
                    CheatedStatusDetected.SafeRaiseEvent(this);
                    break;
                case Status.Failure:
                    DoFailureStatusDetected(statusData);
                    break;
                case Status.CommunicationError:
                    CommunicationErrorStatusDetected.SafeRaiseEvent(this);
                    break;
            }
        }

        private void DoEscrowStatusCleared(object statusData)
        {
            var escrowStatusData = statusData as Denomination;

            if (escrowStatusData == null)
                return;

            EscrowStatusCleared.SafeRaiseEvent(this, new DenominationEventArgs(escrowStatusData));
        }

        private void DoEscrowStatusDetected(object statusData)
        {
            var escrowStatusData = statusData as Denomination;

            if (escrowStatusData == null)
                return;

            EscrowStatusDetected.SafeRaiseEvent(this, new DenominationEventArgs(escrowStatusData));
        }

        private void DoRejectingStatusCleared(object statusData)
        {
            RejectingError rejectingStatusData;

            try
            {
                rejectingStatusData = (RejectingError) statusData;
            }
            catch
            {
                return;
            }
            
            RejectingStatusCleared.SafeRaiseEvent(this, new RejectingEventArgs(rejectingStatusData));
        }

        private void DoRejectingStatusDetected(object statusData)
        {
            RejectingError rejectingStatusData;

            try
            {
                rejectingStatusData = (RejectingError) statusData;
            }
            catch
            {
                return;
            }
            
            RejectingStatusDetected.SafeRaiseEvent(this, new RejectingEventArgs(rejectingStatusData));
        }

        private void DoFailureStatusCleared(object statusData)
        {
            DeviceError failureStatusData;

            try
            {
                failureStatusData = (DeviceError) statusData;
            }
            catch
            {
                return;
            }
            
            FailureStatusCleared.SafeRaiseEvent(this, new DeviceErrorEventArgs(failureStatusData));
        }

        private void DoFailureStatusDetected(object statusData)
        {
            DeviceError failureStatusData;

            try
            {
                failureStatusData = (DeviceError) statusData;
            }
            catch
            {
                return;
            }
            
            FailureStatusDetected.SafeRaiseEvent(this, new DeviceErrorEventArgs(failureStatusData));
        }

        private void A(Action action)
        {
            lock (StateLock)
            {
                ThrowIfDisposed();
            
                action.Invoke();
            }
        }

        private void SafeA(Action action)
        {
            try
            {
                A(action);
            }
            catch
            {
                // ignored
            }
        }

        private TResult F<TResult>(Func<TResult> function)
        {
            lock (StateLock)
            {
                ThrowIfDisposed();
            
                return function.Invoke();
            }
        }

        #region WIN32

        [Guid("00000001-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        internal interface IClassFactory
        {
            int CreateInstance(
                [In, MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, 
                [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, 
                [MarshalAs(UnmanagedType.IUnknown)] ref object ppvObject);
            
            int LockServer(bool fLock);
        }

        [DllImport("WBADrv.dll", CharSet = CharSet.Unicode)]
        private static extern int DllGetClassObject(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)] ref object ppv);
        
        #endregion //WIN32
    }
}
