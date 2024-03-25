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
    using System.Reflection;
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
                    Dispatcher.InvokeAsync(() =>
                    {
                        Banner.DataContext = null;
                        Banner.DataContext = vm;
                    });
                };
            }

            var pro_game = viewModel as ProactiveGame;
            if (pro_game != null)
            {
                pro_game.Success += (doc) =>
                {
                    Thread.Sleep(1000);
                    Dispatcher.InvokeAsync(() =>
                    {
                        MainContent.Child = new ListPlayer();
                    });
                };

                // phải để static không lúc new ListPlayer() nó xóa sự kiện ReadyPlay mất
                pro_game.ReadyPlay += (doc) =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        MainContent.Child = new OnlineBoard();
                    });
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
            Render(new Board(), SinglePlayer.Game); // Đã fix khởi tạo game 2 lần
        }

        public void ShowCreateGame()
        {
            Render(new Grid(), ProactiveGame.Game);
        }

    }
}