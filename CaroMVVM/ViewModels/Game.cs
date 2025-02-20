using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    //public class GameEventArg : EventArgs
    //{
    //    public Document Document { get; set; }
    //}

    public class Game : ViewModelBase
    {
        Player current;
        CellMatrix cellMatrix;

        public event Action<Document> Changed;
        public event Action<Document> GameOver;
        //public event EventHandler<GameEventArg> GameOver;

        //public static bool Flag { get; set; } = true;

        public Player Player
        {
            get
            {
                if (current == null)
                {
                    current = new Player();
                }
                return current;
            }
            set
            {
                current = value;
            }
        }

        public CellMatrix CellMatrix
        {
            get
            {
                if (cellMatrix == null)
                {
                    cellMatrix = new CellMatrix(Size, ConsecutiveCount);
                }
                return cellMatrix;
            }
            set
            {
                cellMatrix = value;
            }
        }

        public override void Start() { }

        public void PutAndCheckOver(int row, int col)
        {
            if (!CellMatrix.IsCellEmpty(row, col)) return; // cell không trống thì mới điền
            var move = CellMatrix.SetCell(row, col, Player, true);
            RaiseChanged(move);
            if (move.Value == null) // nếu đã Win thì Value khác null
            {
                SwitchPlayer();
            }
            else
            {
                RaiseGameOver(move);
                Player.Icon = '\0';
            }
        }

        public void PutFirstPlayer()
        {
            CellMatrix.SetCenter(Player);
            RaiseChanged(CellMatrix);
            SwitchPlayer();
        }

        public void PutFirstByRival(Player player)
        {
            SwitchPlayer();
            PutFirstPlayer();
        }

        protected void RaiseChanged(Document doc) => Changed?.Invoke(doc);
        protected void RaiseGameOver(Document doc) => GameOver?.Invoke(doc);

        //protected void RaiseGameOver(Document doc) => GameOver?.Invoke(this, new GameEventArg { Document = doc });
        protected virtual void SwitchPlayer() { }
      
    }
}
