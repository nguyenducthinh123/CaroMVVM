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
    using System;
    using System.Reflection;
    using Views;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static event Action<object> dataContextChanged;
        public void RaiseDataContextChanged(object vm) => dataContextChanged?.Invoke(vm);

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
            var vm = viewModel as ViewModelBase;
            if (vm == null) return;

            vm.CaptionChanged += () => {
                Dispatcher.InvokeAsync(() =>
                {
                    Banner.DataContext = null;
                    Banner.DataContext = vm;
                });
            };

            DataContext = vm.GetBindingData();
            RaiseDataContextChanged(DataContext);
            MainContent.Child = view;

            var gameOnline = viewModel as GameOnline;
            if (gameOnline == null) return;
          
            gameOnline.Success += (doc) =>
            {
                Thread.Sleep(1000);
                Dispatcher.InvokeAsync(() => {
                    MainContent.Child = new ListPlayer();
                    RaiseDataContextChanged(DataContext);
                });
            };

            if (gameOnline.IsProactive)
            {
                gameOnline.ReadyPlay += (doc) =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        MainContent.Child = new OnlineBoard();
                        RaiseDataContextChanged(DataContext);
                    });
                };
            }
            else
            {
                gameOnline.ReadyCallback += (doc) =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        MainContent.Child = new OnlineBoard();
                        RaiseDataContextChanged(DataContext);
                    });
                };
            }
            //if (Game.Flag)
            //{
            //    var pro_game = viewModel as ProactiveGame;

            //    if (pro_game != null)
            //    {
            //        pro_game.Success += (doc) =>
            //        {
            //            Thread.Sleep(1000);
            //            Dispatcher.InvokeAsync(() =>
            //            {
            //                MainContent.Child = new ListPlayer();
            //            });
            //        };

            //        pro_game.ReadyPlay += (doc) =>
            //        {
            //            Dispatcher.InvokeAsync(() =>
            //            {
            //                MainContent.Child = new OnlineBoard();
            //                pro_game.PlayProactiveGame(ProactiveGame.rival_id);
            //            });
            //        };
            //    }
            //}
            //else
            //{
            //    var passive_game = viewModel as PassiveGame;

            //    if (passive_game != null)
            //    {
            //        passive_game.Success += (doc) =>
            //        {
            //            Thread.Sleep(1000);
            //            Dispatcher.InvokeAsync(() =>
            //            {
            //                MainContent.Child = new ListPlayer();
            //            });
            //        };

            //        passive_game.ReadyCallback += (doc) =>
            //        {
            //            Dispatcher.InvokeAsync(() =>
            //            {
            //                MainContent.Child = new OnlineBoard();
            //                passive_game.PlayPassiveGame(PassiveGame.rival_id);
            //            });
            //        };
            //    }
            //}
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
            Render(new GameOffline(), new Board()); 
        }

        public void ShowCreateGame()
        {
            Render(new GameOnline(), new Grid()); 
        }

        public void ShowFindGame()
        {
            Render(new GameOnline(false), new Grid()); 
        }

    }
}