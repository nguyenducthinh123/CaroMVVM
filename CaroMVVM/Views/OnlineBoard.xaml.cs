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

            var game = ProactiveGame.Game;

        }
    }
}
