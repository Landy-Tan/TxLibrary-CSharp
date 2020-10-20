using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAttribute
{
    class Program
    {
        static void Main(string[] args)
        {
            Type ty = typeof(test);
            var pro = ty.GetProperties();
            foreach(var p in pro)
            {
                var a = p.GetCustomAttributes(false);
                var b = p.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            
            
        }
    }

    class test
    {
        [Description("名称")]
        public string Name { get; set; } = string.Empty;
    }
}
