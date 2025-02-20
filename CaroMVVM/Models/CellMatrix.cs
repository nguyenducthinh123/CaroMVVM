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

        public CellMatrix(int size, int consecutiveCount)
        {
            Size = size; // property đã có setter
            ConsecutiveCount = consecutiveCount;
            cells = new char[size, size];
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
            cells[row, col] = Icon = player.Icon; // Không nên dùng cells[Row, Column] vì nó phải ép Row, Column từ obj về int mỗi lần dùng

            if (check) 
            {
                Func<int, int, bool> over = (dr, dc) => calculate(dr, dc, true) >= ConsecutiveCount - 1;
                if (over(0, 1) || over(1, 0) || over(1, 1) || over(-1, 1))
                {
                    Value = player;
                }
            }

            return this;
        }

        public Document SetCenter(Player player)
        {
            int sz = Size;
            return SetCell(sz >> 1, sz >> 1, player, false);
        }

        int calculate(int dr, int dc, bool invert) // cho thành private là hợp lý
        {
            int r = Row + dr;
            int c = Column + dc;
            int s = 0;
            int sz = Size;
            while (r >= 0 && r < sz && c >= 0 && c < sz && cells[r, c] == Icon)
            {
                ++s;
                r += dr;
                c += dc;
            }
            if (invert) s += calculate(-dr, -dc, false); // cộng với invert
            return s;
        }
    }
}
