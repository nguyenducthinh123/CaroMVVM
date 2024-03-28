﻿using System;
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
            AddPlayer();
        }

        public void AddPlayer()
        {
            if (Game.Flag)
            {
                var game = ProactiveGame.Game;
                game.Start();
                Dictionary<string, bool> itemName = new Dictionary<string, bool>();
                game.JoinCallback += (doc) =>
                {
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
                                game.SendReady(id);
                            };
                            lstPlayer.Items.Add(lstItem);
                        }
                    });
                };
            }
            else
            {
                var game = PassiveGame.Game;
                game.Start();
                Dictionary<string, bool> itemName = new Dictionary<string, bool>();
                game.FoundCallback += (doc) => {
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
                                game.SendJoin(id);
                            };
                            lstPlayer.Items.Add(lstItem);
                        }
                    });
                };
            }
        }
    }
}
