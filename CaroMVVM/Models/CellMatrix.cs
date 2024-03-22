using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class CellMatrix : Document
    {
        char[,] cells;
        public CellMatrix()
        {
            int size = ViewModelBase.Setting.Size;
            cells = new char[size, size];
        }

        public void SetSize(int size)
        {
            Size = size;
        }

        public bool IsCellEmpty(int row, int col)
        {
            return (cells[row, col] == '\0');
        }

        public Document SetCell(int row, int col, Player player) // khả năng phục vụ cho đánh online
        {
            Row = row;
            Column = col;
            Icon = player.Icon;
            cells[Row, Column] = Icon;

            return this;
        }

        public Document SetCenter(Player player)
        {
            int sz = ViewModelBase.Setting.Size;
            return SetCell(sz >> 1, sz >> 1, player);
        }

        public int Calculate(Player player, int row, int col, int dr, int dc, bool invert)
        {
            int r = row + dr;
            int c = col + dc;
            int s = 0;
            int sz = ViewModelBase.Setting.Size;
            while (r >= 0 && r < sz && c >= 0 && c < sz && cells[r, c] == player.Icon)
            {
                ++s;
                r += dr;
                c += dc;
            }
            if (invert) s += Calculate(player, row, col, -dr, -dc, false); // cộng với invert
            return s;
        }

        public bool IsWin(Player player, int row, int col)
        {
            Func<int, int, bool> over = (dr, dc) => Calculate(player, row, col, dr, dc, true) >= 4;
            return over(0, 1) || over(1, 0) || over(1, 1) || over(-1, 1);
        }
    }
}
