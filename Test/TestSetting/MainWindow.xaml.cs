using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TxLibrary.Config.Page;

namespace TestSetting
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Setting setting = new Setting();
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            this.Content = setting.GetControl(new ButtonPage(), true);    // OK
            //this.Content = setting.GetControl(new TablePage(), true);   // OK
            //this.Content = setting.GetControl(new NotPage(), true);   // OK
            //this.Content = setting.GetControl(new ButtonPage(), false);    // OK
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            string show = string.Empty;
            foreach (var propertie in setting.GetType().GetProperties())
            {
                show += $"{propertie.Name}:{propertie.GetValue(setting)}\n";
            }
            MessageBox.Show(show);
        }
    }
}
