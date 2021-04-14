using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TxLibrary.WPF;

namespace testPropertyChanged
{
    class MainWindowViewModule : TxPropertyBase
    {
        private int _age;

        public int Age
        {
            get { return GetField(ref _age); }
            set { SetField(ref _age, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        public ICommand RandomUpdateCommand { get; private set; }
        public ICommand ShowCommand { get; private set; }

        public MainWindowViewModule()
        {
            RandomUpdateCommand = new TxDelegateCommand(RandomUpdate);
            ShowCommand = new TxDelegateCommand(ShowPreperty);
        }

        private void ShowPreperty()
        {
            MessageBox.Show($"Age:{Age}\nName:{Name}");
        }

        private void RandomUpdate()
        {
            Name = Guid.NewGuid().ToString();
            Age = new Random().Next();
        }
    }
}
