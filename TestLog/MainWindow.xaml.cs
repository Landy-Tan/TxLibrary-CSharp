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
using TxLibrary.Log;

namespace TestLog
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            CLog.Info("Info");
            CLog.Warning(0, "Warning");
            CLog.Error(1, "Error");
            this.Content = CLog.Instance;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            CLog.Release();
        }
    }
}
