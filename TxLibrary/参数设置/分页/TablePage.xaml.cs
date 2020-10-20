/**********************************************************************
 * File name:		TablePage.xaml.cs
 * Author:			txl
 * Create date:		
 * Description:		Table页
 * History:			
 * <Date>			<Author>	<Description>
 * 2020年9月10日    txl         迁移到WPF，测试完成
 **********************************************************************/
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TxLibrary.Config.Page

{
    /// <summary>
    /// 以Table页的方式分页
    /// </summary>
    public partial class TablePage : IPage
    {
        List<Tuple<string, FrameworkElement, bool>> Ctrl = new List<Tuple<string, FrameworkElement, bool>>();
        List<FrameworkElement> CurCtrl = new List<FrameworkElement>();

        /// <summary>
        /// 创建对象
        /// </summary>
        public TablePage()
        {
            InitializeComponent();
            this.SizeChanged += TablePage_SizeChanged;
        }

        private void TablePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // 设置控件的宽度，高度默认即可
            foreach (var btn in CurCtrl)
            {
                btn.Width = this.Width;
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
            tabControl.Items.Clear();
            CurCtrl.Clear();
            try
            {
                foreach (var tup in Ctrl)
                {
                    // 创建Tab页
                    TabItem item = new TabItem();
                    item.Header = tup.Item1;
                    item.Content = tup.Item2;
                    item.HorizontalAlignment = HorizontalAlignment.Stretch;
                    item.VerticalAlignment = VerticalAlignment.Stretch;


                    // 设置内容控件
                    tup.Item2.HorizontalAlignment = HorizontalAlignment.Left;
                    tup.Item2.VerticalAlignment = VerticalAlignment.Top;

                    // 添加到窗口和集合中
                    CurCtrl.Add(tup.Item2);
                    tabControl.Items.Add(item);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
    }
}
