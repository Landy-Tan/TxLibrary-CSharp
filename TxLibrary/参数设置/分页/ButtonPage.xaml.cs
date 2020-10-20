/**********************************************************************
 * File name:		ButtonPage.xaml.cs
 * Author:			txl
 * Create date:		
 * Description:		按钮页
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年9月10日    txl         迁移到WPF，测试完成   ▲▼
 **********************************************************************/
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TxLibrary.Config.Page
{
    /// <summary>
    /// 以分栏的方式分页
    /// </summary>
    public partial class ButtonPage : IPage
    {
        List<Tuple<string, FrameworkElement, bool>> Ctrl = new List<Tuple<string, FrameworkElement, bool>>();
        List<Button> CurCtrl = new List<Button>();

        /// <summary>
        /// 创建对象
        /// </summary>
        public ButtonPage()
        {
            InitializeComponent();
            this.SizeChanged += ButtonPage_SizeChanged;
        }

        private void ButtonPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // 设置控件的宽度，高度默认即可
            foreach (var btn in CurCtrl)
            {
                if(btn.Tag is Tuple<string, FrameworkElement, bool>)
                    ((Tuple<string, FrameworkElement, bool>)btn.Tag).Item2.Width = this.Width;
            }
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public override bool AddControl(string name, FrameworkElement ctl)
        {
            Ctrl.Add(new Tuple<string, FrameworkElement, bool>(name, ctl, false));
            return LoadWindow();
        }

        /// <summary>
        /// 加载窗口
        /// </summary>
        /// <returns></returns>
        protected bool LoadWindow()
        {
            // 清空相关数据
            stackPanel.Children.Clear();
            CurCtrl.Clear();
            try
            {
                foreach (var tup in Ctrl)
                {
                    // 创建Button控件
                    var btn = new Button();
                    btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                    btn.Content = $"▲{tup.Item1}";
                    btn.HorizontalContentAlignment = HorizontalAlignment.Left;
                    //btn.Tag = new Tuple<string, FrameworkElement, bool>(tup.Item1, tup.Item2, false);
                    btn.Tag = tup;
                    btn.Click += onButton_Click;

                    // 设置内容控件
                    tup.Item2.Visibility = Visibility.Collapsed;
                    tup.Item2.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tup.Item2.VerticalAlignment = VerticalAlignment.Stretch;

                    // 添加到窗口和集合中
                    CurCtrl.Add(btn);
                    stackPanel.Children.Add(btn);
                    stackPanel.Children.Add(tup.Item2);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 按钮被单击处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onButton_Click(object sender, EventArgs e)
        {
            // 获取相关信息
            Button btn = (Button)sender;
            if (btn.Tag is Tuple<string, FrameworkElement, bool>)
            {
                string name = ((Tuple<string, FrameworkElement, bool>)btn.Tag).Item1;
                FrameworkElement ctl = ((Tuple<string, FrameworkElement, bool>)btn.Tag).Item2;
                bool isShow = ((Tuple<string, FrameworkElement, bool>)btn.Tag).Item3;

                if (!isShow)
                {
                    ctl.Visibility = Visibility.Visible;// 显示控件
                    btn.Content = $"▼{name}";
                }
                else
                {
                    ctl.Visibility = Visibility.Collapsed;// 隐藏控件
                    btn.Content = $"▲{name}";
                }
                btn.Tag = new Tuple<string, FrameworkElement, bool>(name, ctl, !isShow);
            }
        }
    }
}
