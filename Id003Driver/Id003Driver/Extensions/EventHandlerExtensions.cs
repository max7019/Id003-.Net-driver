using System;
using System.Runtime.CompilerServices;

namespace Id003Driver.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void SafeRaiseEvent(this EventHandler @this, object sender)
        {
            try
            {
                RaiseEvent(@this, sender);
            }
            catch
            {
                // ignored
            }
        }

        public static void SafeRaiseEvent<TEventArgs>(this EventHandler<TEventArgs> @this, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            try
            {
                RaiseEvent(@this, sender, eventArgs);
            }
            catch
            {
                // ignored
            }
        }
        
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RaiseEvent(this EventHandler @this, object sender)
        {
            @this?.Invoke(sender, EventArgs.Empty);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> @this, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            @this?.Invoke(sender, eventArgs);
        }
    }
}
