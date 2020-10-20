/**********************************************************************
 * File name:		ISetting.cs
 * Author:			txl
 * Create date:		2020年8月18日
 * Description:		抽象参数基类
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年8月18日    txl         创建，实现GetControl、GetAttribute等方法
 * 2020年9月1日     txl         label.Text值改为从CLanguage获取
 * 2020年9月7日     txl         迁移到WPF（未完成）
 * 2020年9月10日    txl         迁移到WPF已完成并已测试，描述编辑控件已优化
 **********************************************************************/
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;
using System.Windows.Input;

namespace TxLibrary.Config
{

    /// <summary>
    /// 参数基类
    /// </summary>
    [Serializable()]
    public /*abstract*/ class ISetting// : ISerializable
    {
        [NonSerialized]
        private List<Control> Ctrl = null;

        [NonSerialized]
        private TextBox descriptionBox = null;

        [NonSerialized]
        private Window DescWindow = null;

        /// <summary>
        /// 创建一个控件，该控件与此类的公共属性绑定
        /// </summary>
        /// <param name="page">分页方式</param>
        /// <param name="DescriptionBox">true：当鼠标进入相关参数区域时显示其描述信息</param>
        /// <param name="ValueChanged">值更改事件</param>
        /// <param name="Width">控件的宽度</param>
        /// <param name="Height">控件的高度</param>
        /// <param name="LabelCtrlTextAlign">参数名称对齐方式</param>
        /// <param name="LabelCtrlWidth">参数名称宽度</param>
        /// <param name="ValueCtrlWidth">编辑框宽度</param>
        /// <returns></returns>
        public UserControl GetControl(TxLibrary.Config.Page.IPage page,
            bool DescriptionBox = false,
            Action<object, object> ValueChanged = null,
            int Width = 800, int Height = 600,
            HorizontalAlignment LabelCtrlTextAlign = HorizontalAlignment.Left,
            int LabelCtrlWidth = 100, int ValueCtrlWidth = 100)
        {
            // 鼠标进入
            MouseEventHandler mouseEnter = (sender, e) =>
            {
                try
                {
                    if (DescWindow != null) return;
                    if (descriptionBox == null)
                    {
                        descriptionBox = new TextBox()
                        {
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                            IsReadOnly = true,
                            TextWrapping = TextWrapping.Wrap,
                            Width = 600,
                            Height = 60
                        };
                    }
                    {
                        DescWindow = new Window() { WindowStyle = WindowStyle.None };
                        DescWindow.Content = descriptionBox;
                        DescWindow.Width = descriptionBox.Width;
                        DescWindow.Height = descriptionBox.Height;
                        DescWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                    }

                    var send = (Control)sender;
                    var pro = (PropertyInfo)send.Tag;
                    if (pro == null) return;
                    var objDesc = pro.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    var desc = (DescriptionAttribute)(objDesc.Length > 0 ? objDesc[0] : null);
                    if (desc == null) return;
                    if (desc.Description == null || desc.Description == string.Empty)
                    {
                        DescWindow.Content = null;
                        DescWindow.Close();
                        DescWindow = null;
                    }

                    descriptionBox.Text = desc.Description;
                    API.POINT p;
                    API.GetCursorPos(out p);
                    Console.WriteLine($"X:{p.X}\tY:{p.Y}");

                    DescWindow.Left = p.X + 100;
                    DescWindow.Top = p.Y;
                    DescWindow.Show();
                }
                catch (Exception) { }
            };

            // 鼠标离开
            MouseEventHandler MouseLeave = (sender, e) =>
            {
                try
                {
                    if (DescWindow != null)
                    {
                        DescWindow.Content = null;
                        DescWindow.Close();
                        DescWindow = null;
                    }
                }
                catch (Exception) { }
            };

            if (Ctrl == null) Ctrl = new List<Control>();
            Ctrl.Clear();
            Dictionary<string, StackPanel> ctl = new Dictionary<string, StackPanel>();
            //var ctl = new StackPanel();
            //ctl.Orientation = Orientation.Vertical;
            int ControlX = 0, ControlY = 5;

            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo propertie in properties)
            {
                var childCtl = new StackPanel();
                childCtl.Orientation = Orientation.Horizontal;

                var attributes = propertie.GetCustomAttributes(false);

                // 获取相关的属性
                object[] objNames = propertie.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                string name = ((DisplayNameAttribute)(objNames?.Length > 0 ? objNames[0] : null))?.DisplayName;

                object[] objDesc = propertie.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = ((DescriptionAttribute)(objDesc.Length > 0 ? objDesc[0] : null))?.Description;

                object[] objCategory = propertie.GetCustomAttributes(typeof(CategoryAttribute), false);
                string Category = ((CategoryAttribute)(objCategory.Length > 0 ? objCategory[0] : null)).Category;

                // 创建用于显示数据名称的标签控件
                var label = new Label();
                label.Content = name;
                label.Name = $"lab{name}";
                label.Width = LabelCtrlWidth;
                label.HorizontalContentAlignment = LabelCtrlTextAlign;
                label.Tag = propertie;
                ControlX += (int)(label.Width + 5);
                childCtl.Children.Add(label);

                // 创建用于编辑数据的编辑控件
                Control valueCtl = null;
                if (propertie.PropertyType.BaseType.Name == nameof(Enum))
                {
                    //if (Enum.GetNames(propertie.PropertyType).Length <= 2)
                    //{
                    //    // TODO：创建单选按钮
                    //}
                    //else
                    {
                        valueCtl = CreateEnumComboBox(propertie);
                        ((ComboBox)valueCtl).SelectionChanged += EnumComboBox_SelectedIndexChanged;
                        if (ValueChanged != null) ((ComboBox)valueCtl).SelectionChanged += (sender, e) => ValueChanged(sender, e);
                    }
                }
                else if (propertie.PropertyType.Name == nameof(Boolean))
                {
                    valueCtl = CreateBoolComboBox(propertie);
                    ((ComboBox)valueCtl).SelectionChanged += BoolComboBox_SelectedIndexChanged;
                    if (ValueChanged != null) ((ComboBox)valueCtl).SelectionChanged += (sender, e) => ValueChanged(sender, e);
                }
                else if (propertie.PropertyType.Name == nameof(Int16) ||
                    propertie.PropertyType.Name == nameof(Int32) ||
                    propertie.PropertyType.Name == nameof(Int64) ||
                    propertie.PropertyType.Name == nameof(UInt16) ||
                    propertie.PropertyType.Name == nameof(UInt32) ||
                    propertie.PropertyType.Name == nameof(UInt64) ||
                    propertie.PropertyType.Name == nameof(Double))
                {
                    valueCtl = CreateNumberTextBox(propertie);
                    ((TextBox)valueCtl).TextChanged += NumberTextBox_TextChanged;
                    if (ValueChanged != null) ((TextBox)valueCtl).TextChanged += (sender, e) => ValueChanged(sender, e);
                }
                else if (propertie.PropertyType.Name == nameof(String))
                {
                    valueCtl = CreateStringTextBox(propertie);
                    ((TextBox)valueCtl).TextChanged += StringTextBox_TextChanged;
                    if (ValueChanged != null) ((TextBox)valueCtl).TextChanged += (sender, e) => ValueChanged(sender, e);
                }
                valueCtl.Tag = propertie;
                valueCtl.Name = propertie.Name;
                valueCtl.Width = ValueCtrlWidth;

                // 禁用该控件，防止用户编辑值
                //if (Const != null)
                //    valueCtl.IsEnabled = false;

                // 描述信息编辑控件的处理
                if (DescriptionBox)
                {
                    label.MouseEnter += mouseEnter;
                    valueCtl.MouseEnter += mouseEnter;

                    valueCtl.MouseLeave += MouseLeave;
                    label.MouseLeave += MouseLeave;
                }

                childCtl.Children.Add(valueCtl);
                Ctrl.Add(valueCtl);

                ControlX += ValueCtrlWidth + 5;

                ControlX = 0;
                ControlY += 25;
                if (childCtl.Children.Count > 0)
                {
                    if (!ctl.ContainsKey(Category))
                    {
                        var v = new StackPanel() { Orientation = Orientation.Vertical };
                        ctl.Add(Category, v);
                    }
                    ctl[Category]?.Children.Add(childCtl);
                }
                //ctl.Children.Add(childCtl);
            }

            foreach (var item in ctl)
            {
                page.AddControl(item.Key, item.Value);
            }

            UserControl control = new UserControl();
            var panel = new StackPanel() { Orientation = Orientation.Vertical };
            panel.Children.Add(page);
            control.Content = panel;

            return control;
        }

        /// <summary>
        /// 创建枚举类型的ComboBox控件
        /// </summary>
        /// <param name="propertie"></param>
        /// <returns></returns>
        protected virtual ComboBox CreateEnumComboBox(PropertyInfo propertie)
        {
            var cbm = new ComboBox();
            cbm.Name = "cbm" + propertie.Name;
            cbm.IsEditable = false;
            var enumValueNmae = Enum.GetNames(propertie.PropertyType);
            foreach (var name in enumValueNmae) cbm.Items.Add(name);
            cbm.Text = ((Enum)propertie.GetValue(this)).ToString();
            cbm.Tag = propertie;
            return cbm;
        }

        /// <summary>
        /// 创建Bool类型的ComboBox控件
        /// </summary>
        /// <param name="propertie"></param>
        /// <returns></returns>
        protected virtual ComboBox CreateBoolComboBox(PropertyInfo propertie)
        {
            var cbm = new ComboBox();
            cbm.Name = "cbm" + propertie.Name;
            cbm.Style = new Style();
            cbm.Items.Add("是");
            cbm.Items.Add("否");
            cbm.Text = ((bool)propertie.GetValue(this)) ? "是" : "否";
            cbm.Tag = propertie;
            return cbm;
        }

        /// <summary>
        /// 创建数值类型的编辑框控件
        /// </summary>
        /// <param name="propertie"></param>
        /// <returns></returns>
        protected virtual TextBox CreateNumberTextBox(PropertyInfo propertie)
        {
            var txt = new TextBox();
            txt.Name = "txt" + propertie.Name;
            if (propertie.PropertyType.Name == nameof(Double))
                txt.Text = ((double)propertie.GetValue(this)).ToString("0.0000");
            else
                txt.Text = propertie.GetValue(this).ToString();
            txt.Tag = propertie;
            return txt;
        }

        /// <summary>
        /// 创建字符串类型的编辑框控件
        /// </summary>
        /// <param name="propertie"></param>
        /// <returns></returns>
        protected virtual TextBox CreateStringTextBox(PropertyInfo propertie)
        {
            var txt = new TextBox();
            txt.Name = "txt" + propertie.Name;
            txt.Text = (string)propertie.GetValue(this);
            txt.Tag = propertie;
            return txt;
        }

        /// <summary>
        /// 枚举类型的ComboBox控件选中的项被更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void EnumComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var SendCbm = (ComboBox)sender;
                var Property = GetType().GetProperty(SendCbm.Name);
                if (Property == null) return;
                Property.SetValue(this, SendCbm.SelectedIndex);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// boo类型的ComboBox控件选中的项被更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BoolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var SendCbm = (ComboBox)sender;
                var Property = GetType().GetProperty(SendCbm.Name);
                if (Property == null) return;
                Property.SetValue(this, SendCbm.Text == true.ToString() || SendCbm.Text == "是" ? true : false);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 数值类型的编辑框控件的值被更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void NumberTextBox_TextChanged(object sender, EventArgs e)
        {
            var SendText = (TextBox)sender;
            var Property = GetType().GetProperty(SendText.Name);
            if (Property == null) return;

            object objValue = null;
            switch (Property.PropertyType.Name)
            {
                case nameof(Int16):
                    objValue = Convert.ToInt16(SendText.Text);
                    break;
                case nameof(Int32):
                    objValue = Convert.ToInt32(SendText.Text);
                    break;
                case nameof(Int64):
                    objValue = Convert.ToInt64(SendText.Text);
                    break;
                case nameof(UInt16):
                    objValue = Convert.ToUInt16(SendText.Text);
                    break;
                case nameof(UInt32):
                    objValue = Convert.ToUInt32(SendText.Text);
                    break;
                case nameof(UInt64):
                    objValue = Convert.ToUInt64(SendText.Text);
                    break;
                case nameof(Double):
                    objValue = Convert.ToDouble(SendText.Text);
                    break;
            }
            //Property.SetValue(this, Convert.ToInt32(SendText.Text));
            Property.SetValue(this, objValue);
        }

        /// <summary>
        /// 字符串类型的编辑框控件的值被更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void StringTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var SendCbm = (TextBox)sender;
                ((PropertyInfo)SendCbm.Tag).SetValue(this, SendCbm.Text);
            }
            catch (Exception) { }
        }

    }

    /// <summary>
    /// 获取鼠标所在位置
    /// </summary>
    class API
    {
        /// <summary>
        /// 获取鼠标当前所在位置
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y) { X = x; Y = y; }
        }
    }
}
