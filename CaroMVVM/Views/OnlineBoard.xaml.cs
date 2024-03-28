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
    /// Interaction logic for OnlineBoard.xaml
    /// </summary>
    public partial class OnlineBoard : UserControl
    {
        public OnlineBoard()
        {
            InitializeComponent();
            CreateBoard();
        }

        public void CreateBoard()
        {
            int cell_size = ViewModelBase.Setting.CellSize;
            int size = ViewModelBase.Setting.Size;

            int w = size * cell_size;
            var grid = new Grid()
            {
                Background = Brushes.Blue,
                Width = w,
                Height = w,
            }; //add new grid into the border
            onl_board.Child = grid;

            // Vẽ hàng và cột
            for (int i = 0; i < size; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
            }
            // Đặt các quân lên hàng và cột
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    var piece = new Piece(r, c);
                    grid.Children.Add(piece);
                }
            }

            if (Game.Flag)
            {
                var game = ProactiveGame.Game;

                game.Changed += (doc) => 
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        int index = doc.Row * size + doc.Column;
                        var cell = grid.Children[index] as Piece;
                        cell.Put(doc.Icon);
                    });
                    game.SendMove(doc.Row, doc.Column, doc.Value != null);
                };

                game.GameOver += (doc) =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        foreach (Piece piece in grid.Children)
                        {
                            piece.Background = Brushes.Gray;
                        }

                        if (doc.Icon != '\0')
                        {
                            doc.Icon -= ' ';
                            MessageBox.Show(doc.Icon + " Win");
                        }
                    });
                   
                };

                PreviewMouseLeftButtonUp += (s, e) =>
                {
                    if (!game.my_turn) return;
                    var p = e.GetPosition(grid);
                    int r = (int)(p.Y / cell_size);
                    int c = (int)(p.X / cell_size);

                    game.PutAndCheckOver(r, c);
                };
            }
            else
            {
                var game = PassiveGame.Game;

                game.Changed += (doc) =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        int index = doc.Row * size + doc.Column;
                        var cell = grid.Children[index] as Piece;
                        cell.Put(doc.Icon);
                    });
                    
                    game.SendMove(doc.Row, doc.Column, doc.Value != null); // Value khác null là win, tức là IsWin = true
                };

                game.GameOver += (doc) =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        foreach (Piece piece in grid.Children)
                        {
                            piece.Background = Brushes.Gray;
                        }

                        if (doc.Icon != '\0')
                        {
                            doc.Icon -= ' ';
                            MessageBox.Show(doc.Icon + " Win");
                        }
                    });
                   
                };

                PreviewMouseLeftButtonUp += (s, e) =>
                {
                    if (!game.my_turn) return;
                    var p = e.GetPosition(grid);
                    int r = (int)(p.Y / cell_size);
                    int c = (int)(p.X / cell_size);

                    game.PutAndCheckOver(r, c);
                };
            }
        }
    }
}
