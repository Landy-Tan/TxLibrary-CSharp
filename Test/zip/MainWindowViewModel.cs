using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TxLibrary.IO;
using TxLibrary.WPF;

namespace zip
{
    class MainWindowViewModel : TxPropertyBase
    {
        private string _zip = @"C:\Users\Landy\Desktop\bin.zip";
        private string _path = @"C:\Users\Landy\Desktop\新建文件夹";

        public string Zip
        {
            get { return _zip; }
            set { SetField(ref _zip, value); }
        }

        public string Path
        {
            get { return _path; }
            set { SetField(ref _path, value); }
        }

        public TxDelegateCommand OpenZipCommand { get; private set; }
        public TxDelegateCommand OpenPathCommand { get; private set; }
        public TxDelegateCommand ZipCommand { get; private set; }
        public TxDelegateCommand UnZipCommand { get; private set; }

        public MainWindowViewModel()
        {
            OpenZipCommand = new TxDelegateCommand(OpenZip);
            OpenPathCommand = new TxDelegateCommand(OpenPath);
            ZipCommand = new TxDelegateCommand(onZip, onCanZip);
            UnZipCommand = new TxDelegateCommand(onUnZip, onCanUnZip);
        }

        private void onUnZip()
        {
            bool rst = TxZip.UnZip(Zip, Path);
            MessageBox.Show($"解压缩文件：\"{Zip}\"到\"{Path}\"！{(rst ? "成功" : "失败")}");
        }

        private bool onCanUnZip()
        {
            return !string.IsNullOrWhiteSpace(Zip) && File.Exists(Zip);
        }

        private void onZip()
        {
            bool rst = TxZip.Zip(Path, Zip);
            MessageBox.Show($"压缩文件：\"{Path}\"到\"{Zip}\"！{(rst ? "成功" : "失败")}");
        }

        private bool onCanZip()
        {
            return !string.IsNullOrWhiteSpace(Path) && Directory.Exists(Path);
        }

        private void OpenPath()
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            Path = dlg.SelectedPath;
        }

        private void OpenZip()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "(*.zip)|*.zip";
            if (dlg.ShowDialog() != DialogResult.OK) return;
            Zip = dlg.FileName;
        }
    }
}
