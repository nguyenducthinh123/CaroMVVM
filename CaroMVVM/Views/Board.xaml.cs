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
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        public Board()
        {
            InitializeComponent();
            CreateGame();
        }

        public void CreateGame()
        {
            var game = new SinglePlayer();
            int cell_size = ViewModelBase.Setting.CellSize;
            int size = ViewModelBase.Setting.Size;

            int w = size * cell_size;
            var grid = new Grid()
            {
                Background = Brushes.Blue,
                Width = w,
                Height = w,
            }; //add new grid into the border
            board.Child = grid;

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
                    var piece = new Piece(game, r, c);
                    grid.Children.Add(piece);
                }
            }

            game.Changed += (doc) => {
                int index = doc.Row * game.Size + doc.Column;
                var cell = grid.Children[index] as Piece;
                cell.Put(doc.Icon);
            };

            game.GameOver += (doc) =>
            {
                foreach(Piece p in grid.Children)
                {
                    p.Background = Brushes.Gray;
                }

                doc.Icon -= ' ';
                MessageBox.Show(doc.Icon + " Win");
            };

            PreviewMouseLeftButtonUp += (s, e) => {
                var p = e.GetPosition(grid);
                int r = (int)(p.Y / cell_size);
                int c = (int)(p.X / cell_size);
                game.PutAndCheckOver(r, c);
            };

            game.Start();

            // để game xử lí
            //game.PutFirsrPlayer();

            //game.GameOver += (s, e) => {
            //    var ts = new Thread(new ThreadStart(() =>
            //    {
            //        MessageBox.Show(e.Document.Icon.ToString().ToUpper() + " Win");
            //    }));
            //    ts.Start();
            //};
        }
    }
}
