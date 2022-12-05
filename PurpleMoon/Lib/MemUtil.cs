using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon
{
    public static unsafe class MemUtil
    {
        public static void Copy(uint[] dest, uint dest_offset, uint[] src, uint src_offset, uint sz)
        {
            fixed (uint* dptr = &dest[dest_offset / 4])
            {
                fixed (uint* sptr = &src[src_offset / 4])
                {
                    Cosmos.Core.MemoryOperations.Copy((byte*)dptr, (byte*)sptr, (int)sz);
                }
            }
        }
    }
}
