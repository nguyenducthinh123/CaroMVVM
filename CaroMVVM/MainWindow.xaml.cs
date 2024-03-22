using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaroMVVM
{

    using ViewModels;
    using Views;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        void ApplyMenu(ItemCollection items)
        {
            foreach (MenuItem item in items)
            {
                var name = "Show" + item.Header.ToString().Replace(" ", "");
                var method = this.GetType().GetMethod(name);

                if (method != null)
                {
                    item.Click += (s, e) => method.Invoke(this, new object[] { });
                }
                ApplyMenu(item.Items);
            }
        }

        void Render(UIElement view, object viewModel)
        {
            DataContext = viewModel;
            MainContent.Child = view;

            var vm = viewModel as ViewModelBase;
            if (vm != null)
            {
                vm.CaptionChanged += () => {
                    Banner.DataContext = null;
                    Banner.DataContext = vm;
                };
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            ShowSetting();

            ApplyMenu(MainMenu.Items);
        }

        public void ShowSetting()
        {
            Render(new SettingView(), new SettingViewModel());
        }

        public void ShowSinglePlayer()
        {
            Render(new Board(), new SinglePlayer());
        }


    }
}