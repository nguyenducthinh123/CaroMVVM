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

namespace CaroMVVM.Views
{
    /// <summary>
    /// Interaction logic for ListPlayer.xaml
    /// </summary>
    public partial class ListPlayer : UserControl
    {
        public ListPlayer()
        {
            InitializeComponent();
            MainWindow.openListPlayer += (vm) =>
            {
                var gameOnline = vm as GameOnline;
                if (gameOnline == null) return;
                AddPlayer(gameOnline);
                gameOnline.Start();
            };
        }

        public void AddPlayer(object vm)
        {
            var gameOnline = vm as GameOnline;
            if (gameOnline == null) return;

            Dictionary<string, bool> itemName = new Dictionary<string, bool>();
            if (gameOnline.IsProactive)
            {
                gameOnline.JoinCallback += (doc) =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        ListViewItem lstItem = new ListViewItem();
                        var name = doc.Name;
                        if (!itemName.ContainsKey(name))
                        {
                            itemName[name] = true; // mark as exist
                            var id = doc.ObjectId;

                            lstItem.Content = name;
                            lstItem.MouseLeftButtonUp += (s, e) =>
                            {
                                gameOnline.SendReady(id);
                            };
                            lstPlayer.Items.Add(lstItem);
                        }
                    });
                };
            }
            else
            {
                gameOnline.FoundCallback += (doc) => {
                    Dispatcher.InvokeAsync(() =>
                    {
                        ListViewItem lstItem = new ListViewItem();
                        var name = doc.Name;
                        if (!itemName.ContainsKey(name))
                        {
                            itemName[name] = true;
                            var id = doc.ObjectId;

                            lstItem.Content = name;
                            lstItem.MouseLeftButtonUp += (s, e) =>
                            {
                                gameOnline.SendJoin(id);
                            };
                            lstPlayer.Items.Add(lstItem);
                        }
                    });
                };
            }
        }
    }
}
