using System;

namespace Id003Driver
{
    public static class Utils
    {
        public static int ExtractPortNumber(string portName)
        {
            const string comPrefix = "COM";
            
            return int.Parse(
                portName.Substring(
                    portName.LastIndexOf(comPrefix, StringComparison.InvariantCulture) + comPrefix.Length));
        }
    }
}
