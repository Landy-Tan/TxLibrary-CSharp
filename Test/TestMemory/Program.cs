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
            var setting = new Setting() { Age = 10, Name = "test", Address="中华人民共和国", iPhone = "110" };
            //new TxLibrary.Config.Memory.BinarySerializeMemory<Setting1>().Save("test1.setting", setting);
            //var setting2 = new TxLibrary.Config.Memory.BinarySerializeMemory<Setting1>().Load("test1.setting");

            //new TxLibrary.Config.Memory.JsonMemory<Setting1>().Save("test1.json", setting);
            //var setting2 = new TxLibrary.Config.Memory.JsonMemory<Setting1>().Load("test1.json");

            //new TxLibrary.Config.Serialize.XmlSerialize<Setting1>().Save("test.xml", setting);
            //var setting2 = new TxLibrary.Config.Serialize.XmlSerialize<Setting1>().Load("test.xml");

            //new TxLibrary.Config.Serialize.UserDefaultSerialize<Setting>().Save("t.cfg", setting);
            var setting2 = new TxLibrary.Config.Serialize.UserDefaultSerialize<Setting>().Load("t.cfg");
        }
    }

    [Serializable]
    public class Setting : TxLibrary.Config.ISetting
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        //public char[] Addresss { get; set; }// UserDefaultSerialize 执行转换
        public string iPhone { get; set; }
    }

    [Serializable]
    public class Setting1
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
