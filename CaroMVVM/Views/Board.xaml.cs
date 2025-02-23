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
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        public Board()
        {
            InitializeComponent();
            MainWindow.dataContextChanged += (vm) =>
            {
                var game = vm as GameOffline;
                if (game == null) return;
                Setup(game);
                game.Start();
            };
        }

        private void Setup(object vm)
        {
            var game = vm as GameOffline;
            int size = game.Size;
            int cell_size = game.CellSize;

            var grid = CreateBoard(size, cell_size);
            board.Child = grid;
                       
            game.Changed += (doc) =>
            {
                int index = doc.Row * size + doc.Column;
                var cell = grid.Children[index] as Piece;
                cell.Put(doc.Icon);
            };

            game.GameOver += (doc) =>
            {
                foreach (Piece p in grid.Children)
                {
                    p.Background = Brushes.Gray;
                }

                Task.Run(() => {
                    if (doc.Icon == 'x' || doc.Icon == 'o')
                    {
                        doc.Icon -= ' '; // upcase
                        MessageBox.Show(doc.Icon + " Win");
                    }
                });
                

            };

            PreviewMouseLeftButtonUp += (s, e) => {
                var p = e.GetPosition(grid);
                int r = (int)(p.Y / cell_size);
                int c = (int)(p.X / cell_size);
                game.PutAndCheckOver(r, c); // Thằng này gọi sự kiện Changed
            };

        }

        Grid CreateBoard(int size, int cell_size)
        {
            int w = size * cell_size;
            var grid = new Grid()
            {
                Background = Brushes.Blue,
                Width = w,
                Height = w,
            }; //add new grid into the border

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

            return grid;

        }

    }
}
