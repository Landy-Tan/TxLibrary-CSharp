using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxLibrary.Config;

namespace TestSetting
{
    public class Setting : ISetting
    {
        [Category("基础信息"), DisplayName("姓名"), Description("姓名")]
        public string Name { get; set; }

        [Category("基础信息"), DisplayName("年龄")]
        public ushort Age { get; set; }

        [Category("通信信息"), DisplayName("地址"), Description("家庭地址")]
        public string Address { get; set; }

        [Category("通信信息"), DisplayName("手机号码"), Description("")]
        public string IPhone { get; set; }
    }
}
