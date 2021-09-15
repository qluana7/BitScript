using System;
using System.Linq;

namespace BitScript
{
    public static class Utils
    {
        public static byte Reverse(byte b)
        {
            var a = Convert.ToString(b, 2).PadLeft(8, '0');
            
            return (byte)Convert.ToInt32(new string(a.Reverse().ToArray()), 2);
        }
    }
    
    public static class Extensions
    {
        public static byte Reverse(this byte b)
            => Utils.Reverse(b);
    }
}