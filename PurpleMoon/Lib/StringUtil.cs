using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon
{
    public static class StringUtil
    {
        private const string _hexvals = "0123456789ABCDEF";

        public static string ConvertUIntToHex(uint num, int bytes)
        {
            string hex = string.Empty;
            if (bytes == 1)
            {
                hex += _hexvals[(int)((num & 0xF0) >> 4)];
                hex += _hexvals[(int)((num & 0x0F) >> 0)];
            }
            else if (bytes == 2)
            {
                hex += _hexvals[(int)((num & 0xF000) >> 12)];
                hex += _hexvals[(int)((num & 0x0F00) >> 8)];
                hex += _hexvals[(int)((num & 0x00F0) >> 4)];
                hex += _hexvals[(int)((num & 0x000F) >> 0)];
            }
            else if (bytes == 4)
            {
                hex += _hexvals[(int)((num & 0xF0000000) >> 28)];
                hex += _hexvals[(int)((num & 0x0F000000) >> 24)];
                hex += _hexvals[(int)((num & 0x00F00000) >> 20)];
                hex += _hexvals[(int)((num & 0x000F0000) >> 16)];
                hex += _hexvals[(int)((num & 0x0000F000) >> 12)];
                hex += _hexvals[(int)((num & 0x00000F00) >> 8)];
                hex += _hexvals[(int)((num & 0x000000F0) >> 4)];
                hex += _hexvals[(int)((num & 0x0000000F) >> 0)];
            }
            return hex;
        }
    }
}
