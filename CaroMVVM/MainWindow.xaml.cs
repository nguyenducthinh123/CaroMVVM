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
    using System.Security.RightsManagement;
    using System.Windows.Media.Animation;
    using Views;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static event Action<object> dataContextChanged;
        public void RaiseDataContextChanged(object vm) => dataContextChanged?.Invoke(vm);
        public static event Action<object> openListPlayer;
        public void RaiseOpenListPlayer(object vm) => openListPlayer?.Invoke(vm);
        public static event Action<object> openOnlineBoard;
        public void RaiseOpenOnlineBoard(object vm) => openOnlineBoard?.Invoke(vm);

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

            vm.CaptionChanged += () =>
            {
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

            if (!NetworkHelper.HasInternetAccess)
            {
                gameOnline.Caption = "Lost connection to internet. Can't play online mode";
                return;
            }

            ViewModelBase.Broker.Connect();

            gameOnline.Caption = "Choose a player";
            Dispatcher.InvokeAsync(() => {
                MainContent.Child = new ListPlayer();
                RaiseOpenListPlayer(DataContext);
            });

            if (gameOnline.IsProactive)
            {
                gameOnline.ReadyPlay += (doc) =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        MainContent.Child = new OnlineBoard();
                        RaiseOpenOnlineBoard(DataContext);
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
                        RaiseOpenOnlineBoard(DataContext);
                    });
                };
            }

        }
        public MainWindow()
        {
            InitializeComponent();
            ShowSetting();
            if (!NetworkHelper.HasInternetAccess)
            {
                MessageBox.Show("No internet. Can't play online mode");
            }
            else
            {
                ViewModelBase.Broker.Connect();
            }
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
            var settingsPopup = new SettingPopup();
            settingsPopup.ShowDialog();
            Render(new GameOnline(), new Grid()); 
        }

        public void ShowFindGame()
        {
            Render(new GameOnline(false), new Grid()); 
        }
    }
}