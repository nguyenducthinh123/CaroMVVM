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
using System.Windows.Shapes;

namespace CaroMVVM.Views
{
    /// <summary>
    /// Interaction logic for SettingPopup.xaml
    /// </summary>
    public partial class SettingPopup : Window
    {
        public SettingPopup()
        {
            InitializeComponent();
            DataContext = ViewModelBase.Setting;

            btnSave.Click += (s, e) =>
            {
                ViewModelBase.Setting.Save((Setting)DataContext);
                Close();
            };
        }

        
    }
}
