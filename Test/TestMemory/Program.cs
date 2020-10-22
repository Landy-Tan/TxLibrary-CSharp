using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMemory
{
    class Program
    {
        static void Main(string[] args)
        {
            var setting = new Setting1() { Age = 10, Name = "name" };
            //new TxLibrary.Config.Memory.BinarySerializeMemory<Setting1>().Save("test1.setting", setting);
            //var setting2 = new TxLibrary.Config.Memory.BinarySerializeMemory<Setting1>().Load("test1.setting");

            new TxLibrary.Config.Memory.JsonMemory<Setting1>().Save("test1.json", setting);
            var setting2 = new TxLibrary.Config.Memory.JsonMemory<Setting1>().Load("test1.json");
        }
    }

    [Serializable]
    class Setting : TxLibrary.Config.ISetting
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [Serializable]
    class Setting1
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
