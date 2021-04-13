using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxLibrary.IO;

namespace TestZip
{
    class Program
    {
        static void Main(string[] args)
        {
            testFile();
        }

        static void testFolder()
        {
            TxZip.Zip(@"C:\Users\Landy\Desktop\TxFramework.Plugin - 副本", @"C:\Users\Landy\Desktop\TxFramework.Plugin - 副本.zip");
            TxZip.UnZip(@"C:\Users\Landy\Desktop\TxFramework.Plugin - 副本.zip", @"C:\Users\Landy\Desktop\TxFramework.Plugin - 副本2");
        }

        static void testFile()
        {
            TxZip.Zip(@"C:\Users\Landy\Desktop\hosts", @"C:\Users\Landy\Desktop\hosts.zip");
            TxZip.UnZip(@"C:\Users\Landy\Desktop\hosts.zip", @"C:\Users\Landy\Desktop\hosts1");
        }
    }
}
