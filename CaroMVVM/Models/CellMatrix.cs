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

        // SetCell đã tính win luôn rồi
        public Document SetCell(int row, int col, Player player, bool check) // khả năng phục vụ cho đánh online
        {
            Row = row;
            Column = col;
            cells[row, col] = Icon = player.Icon;

            if (check) 
            {
                Func<int, int, bool> over = (dr, dc) => Calculate(dr, dc, true) >= 4;
                if (over(0, 1) || over(1, 0) || over(1, 1) || over(-1, 1))
                {
                    Value = player;
                }
            }

            return this;
        }

        public Document SetCenter(Player player)
        {
            int sz = ViewModelBase.Setting.Size;
            return SetCell(sz >> 1, sz >> 1, player, false);
        }

        public int Calculate(int dr, int dc, bool invert)
        {
            int r = Row + dr;
            int c = Column + dc;
            int s = 0;
            int sz = ViewModelBase.Setting.Size;
            while (r >= 0 && r < sz && c >= 0 && c < sz && cells[r, c] == Icon)
            {
                ++s;
                r += dr;
                c += dc;
            }
            if (invert) s += Calculate(-dr, -dc, false); // cộng với invert
            return s;
        }
    }
}
