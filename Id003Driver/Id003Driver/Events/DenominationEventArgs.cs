using System;

namespace Id003Driver.Events
{
    public class DenominationEventArgs : EventArgs
    {
        public Denomination Denomination { get; }

        public DenominationEventArgs(Denomination denomination)
        {
            Denomination = denomination;
        }
    }
}
