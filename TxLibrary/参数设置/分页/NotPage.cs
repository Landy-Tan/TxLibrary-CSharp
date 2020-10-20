using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TxLibrary.Config.Page
{
    /// <summary>
    /// 不分页
    /// </summary>
    public class NotPage : IPage
    {
        List<Tuple<string, FrameworkElement>> Ctrl = new List<Tuple<string, FrameworkElement>>();

        /// <summary>
        /// 创建对象
        /// </summary>
        public NotPage()
        {
            this.SizeChanged += NotPage_SizeChanged;
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public override bool AddControl(string name, FrameworkElement ctl)
        {
            Ctrl.Add(new Tuple<string, FrameworkElement>(name, ctl));
            LoadWindow();
            return true;
        }

        private void NotPage_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            // 设置控件的宽度，高度默认即可
            //foreach (var btn in CurCtrl)
            //{
            //    btn.Width = this.Width;
            //}
        }

        private void LoadWindow()
        {
            ((StackPanel)this.Content)?.Children.Clear();
            var stackPanel = new StackPanel() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Orientation = Orientation.Vertical };
            this.Content = stackPanel;
            foreach(var tup in Ctrl)
            {
                stackPanel.Children.Add(tup.Item2);
            }
        }
    }
}
