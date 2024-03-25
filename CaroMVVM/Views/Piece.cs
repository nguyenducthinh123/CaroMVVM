using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System
{
    internal class Piece : Canvas
    {
        public int Row
        {
            get => (int)GetValue(Grid.RowProperty);
            set => SetValue(Grid.RowProperty, value);
        }
        public int Column
        {
            get => (int)GetValue(Grid.ColumnProperty);
            set => SetValue(Grid.ColumnProperty, value);
        }
        public bool IsEmpty => Children.Count == 0;
        public void Clear() => Children.Clear();
        public void DrawO() {
            double sz = this.ActualWidth - 4;

            if (sz < 0) sz = ViewModelBase.Setting.CellSize - 5;
            Children.Add(new Ellipse
            {
                Width = sz,
                Height = sz,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Margin = new Thickness(2),
            });
        }
        public void DrawX()
        {
            const double d = 4;
            double sz = this.ActualWidth - d;
            // :))
            if (sz < 0) sz = ViewModelBase.Setting.CellSize - 5;
            Children.Add(new Line
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                X1 = d,
                Y1 = d,
                X2 = sz,
                Y2 = sz,
            });
            Children.Add(new Line
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                X1 = d,
                Y1 = sz,
                X2 = sz,
                Y2 = d,
            });
        }

        public void Put(char icon)
        {
            if (icon == 'o') DrawO();
            else if (icon == 'x') DrawX();
        }

        public Piece(int r, int c)
        {
            Row = r;
            Column = c;
            Background = Brushes.Black;
            Margin = new Thickness(0.5);

            //game.Changed += (s, e) => {
            //    // do có rất nhiều canvas nhận sự kiện này nên phải xử lý như bên dòng dưới
            //    if (e.Document.Row != r || e.Document.Column != c) return;

            //    switch (e.Document.Icon)
            //    {
            //        case '\0': Clear(); break;
            //        case 'o': DrawO(); break;
            //        case 'x': DrawX(); break;
            //    }
            //};
            //game.GameOver += (s, e) => {
            //    Background = Brushes.Gray;
            //    IsEnabled = false;
            //};

            //PreviewMouseLeftButtonUp += (s, e) => {
            //    // có rồi thì không nhét nữa
            //    if (Children.Count > 0) return;
            //    game.PutAndCheckOver(r, c);
            //};
        }

    }
}
