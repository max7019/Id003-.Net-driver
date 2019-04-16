using System;

namespace Id003Driver
{
    public class Denomination
    {
        public double Value { get; }
        public string Currency { get; }

        public Denomination(double value, string currency)
        {
            if (value < 0.0)
                throw new ArgumentOutOfRangeException(nameof(value));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Incorrect currency value!");

            Value = value;
            Currency = currency.ToUpperInvariant();
        }

        protected bool Equals(Denomination other)
        {
            return Value.Equals(other.Value) && string.Equals(Currency, other.Currency);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            
            if (ReferenceEquals(this, obj)) 
                return true;
            
            return obj.GetType() == GetType() && Equals((Denomination) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ (Currency?.GetHashCode() ?? 0);
            }
        }
    }
}
