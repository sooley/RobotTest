using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Static
{
    public static class MemoryDebug
    {
        public static void Print(string name)
        {
            long managed = GC.GetTotalMemory(false);
            using (Process p = Process.GetCurrentProcess())
            {
                Console.WriteLine(
    $"{name,-20} " +
    $"Managed = {managed / 1024.0 / 1024.0:F2} MB | " +
    $"Private = {p.PrivateMemorySize64 / 1024.0 / 1024.0:F2} MB | " +
    $"Working = {p.WorkingSet64 / 1024.0 / 1024.0:F2} MB");
            }                    
        }
    }
}
