using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using ProcessMemoryUtilities.Managed;
using ProcessMemoryUtilities.Native;


namespace UpgradeRemover06
{
    public class MemoryUtils
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            IntPtr hProcess,
            ref long lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead);

        private Int64 baseaddr;
       private Process xeniaProc;
       private IntPtr xeniaHandle;

       public MemoryUtils()
       {
           baseaddr = 0x1401F2B1C;
       }
        public bool tryOpenXenia()
        {
            Process[] xenias = Process.GetProcessesByName("xenia");
            if (xenias.Length > 0)
            {
                xeniaProc = xenias[0];
                xeniaHandle = NativeWrapper.OpenProcess(ProcessAccessFlags.All, false, xeniaProc.Id);
                return true;
            }

            return false;
        }
        
        public bool readFlag(int flagnum)
        {
            Int64 flagloc =baseaddr + 4 * flagnum + 0x104;
            

            byte[] flagbuf = new byte[4];
            NativeWrapper.ReadProcessMemoryArray(xeniaHandle, new IntPtr(flagloc), flagbuf, 0);
            Debug.WriteLine(flagbuf[3].ToString());
                return flagbuf[3] != 0;
            }

        public void writeFlag(int flagnum, byte val)
        {
            Int64 flagloc =baseaddr + 4 * flagnum + 0x104;

            NativeWrapper.WriteProcessMemory(xeniaHandle, new IntPtr(flagloc + 3), ref val);

        }
        
    }   
}