using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class GameEventArg : EventArgs
    {
        public Document Document { get; set; }
    }

    public class Game : ViewModelBase
    {
        Player current;
        CellMatrix cellMatrix;

        public event EventHandler<GameEventArg> Changed;
        public event EventHandler<GameEventArg> GameOver;

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

        public override void Start()
        {
            Size = Setting.Size;
            cellMatrix = new CellMatrix();
        }

        public void PutAndCheckOver(int row, int col)
        {
            cellMatrix.SetCell(row, col, current);
            RaiseChanged(cellMatrix);

            if (cellMatrix.IsWin(current, row, col))
            {
                RaiseGameOver(cellMatrix);
                return;
            }

            SwitchPlayer();
        }

        public void PutFirsrPlayer()
        {
            cellMatrix.SetCenter(current);
            RaiseChanged(cellMatrix);
            SwitchPlayer();
        }

        protected void RaiseChanged(Document Doc) => Changed?.Invoke(this, new GameEventArg { Document = Doc});
        protected void RaiseGameOver(Document Doc) => GameOver?.Invoke(this, new GameEventArg { Document= Doc });
        protected virtual void SwitchPlayer()
        {
            
        }
    }
}
