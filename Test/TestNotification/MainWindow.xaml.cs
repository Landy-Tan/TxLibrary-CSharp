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

namespace TestNotification
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public static MainWindow Instance { get; private set; }
        A a = new A();
        B b = new B();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void AAsync_Click(object sender, RoutedEventArgs e)
        {
            a.AsyncSend();
        }

        private void ASend_Click(object sender, RoutedEventArgs e)
        {
            a.Send();
        }

        private void BAsync_Click(object sender, RoutedEventArgs e)
        {
            b.AsyncSend();
        }

        private void BSend_Click(object sender, RoutedEventArgs e)
        {
            b.Send();
        }
    }

    interface M
    {
        void Send();
        void AsyncSend();
    }

    class A : M
    {
        public A()
        {
            TxLibrary.Notification.TxNotification.Instance.NotificationEvent += Instance_NotificationEvent;
        }

        private void Instance_NotificationEvent(TxLibrary.Notification.TxNotification.Notification notification, object[] args)
        {
            MainWindow.Instance.Dispatcher.Invoke(()=> MainWindow.Instance.txtLog.Text += "Class A 接收到通知\n");
            System.Threading.Thread.Sleep(1000 * 10);
        }

        public void AsyncSend()
        {
            MainWindow.Instance.txtLog.Text += "Class A 发送异步通知\n";
            TxLibrary.Notification.TxNotification.Instance.SendAsyncNotification(TxLibrary.Notification.TxNotification.Notification.Initialize, null);
        }

        public void Send()
        {
            MainWindow.Instance.txtLog.Text += "Class A 发送通知\n";
            TxLibrary.Notification.TxNotification.Instance.SendNotification(TxLibrary.Notification.TxNotification.Notification.Initialize, null);
        }
    }

    class B : M
    {
        public B()
        {
            TxLibrary.Notification.TxNotification.Instance.NotificationEvent += Instance_NotificationEvent;
        }

        private void Instance_NotificationEvent(TxLibrary.Notification.TxNotification.Notification notification, object[] args)
        {
            MainWindow.Instance.Dispatcher.Invoke(() => MainWindow.Instance.txtLog.Text += "Class B 接收到通知\n");
        }
        public void AsyncSend()
        {
            MainWindow.Instance.txtLog.Text += "Class B 发送异步通知\n";
            TxLibrary.Notification.TxNotification.Instance.SendAsyncNotification(TxLibrary.Notification.TxNotification.Notification.Initialize, null);
        }

        public void Send()
        {
            MainWindow.Instance.txtLog.Text += "Class B 发送通知\n";
            TxLibrary.Notification.TxNotification.Instance.SendNotification(TxLibrary.Notification.TxNotification.Notification.Initialize, null);
        }
    }

}
