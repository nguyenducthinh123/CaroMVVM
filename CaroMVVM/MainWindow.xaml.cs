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

        void Render(object viewModel, UIElement view)
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

            if (Game.Flag)
            {
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

                    pro_game.ReadyPlay += (doc) =>
                    {
                        Dispatcher.InvokeAsync(() =>
                        {
                            Thread.Sleep(500); // đợi 1 chút cho thằng passive game nó bật bàn cờ
                            MainContent.Child = new OnlineBoard();
                        });
                    };
                }
            }
            else
            {
                var passive_game = viewModel as PassiveGame;

                if (passive_game != null)
                {
                    passive_game.Success += (doc) =>
                    {
                        Thread.Sleep(1000);
                        Dispatcher.InvokeAsync(() =>
                        {
                            MainContent.Child = new ListPlayer();
                        });
                    };

                    passive_game.ReadyCallback += (doc) =>
                    {
                        Dispatcher.InvokeAsync(() =>
                        {
                            MainContent.Child = new OnlineBoard();
                        });
                    };
                }
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
            Render(new SettingViewModel(), new SettingView());
        }

        public void ShowSinglePlayer()
        {
            Render(new SinglePlayer(), new Board()); // Khởi tạo 2 lần nhưng vẫn ok do đã xử lý
        }

        public void ShowCreateGame()
        {
            Render(ProactiveGame.Game, new Grid()); // Đã fix khởi tạo game 2 lần
        }

        public void ShowFindGame()
        {
            Render(PassiveGame.Game, new Grid()); // Đã fix khởi tạo 2 lần
        }

    }
}