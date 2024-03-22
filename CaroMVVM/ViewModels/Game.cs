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

        public event Action<Document> Changed;
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
            var move = cellMatrix.SetCell(row, col, current, true);
            RaiseChanged(move);
            if (move.Value == null)
            {
                SwitchPlayer();
            }
        }

        public void PutFirstPlayer()
        {
            cellMatrix.SetCenter(current);
            RaiseChanged(cellMatrix);
            SwitchPlayer();
        }

        protected void RaiseChanged(Document doc) => Changed?.Invoke(doc);
        protected void RaiseGameOver(Document doc) => GameOver?.Invoke(this, new GameEventArg { Document = doc });
        protected virtual void SwitchPlayer()
        {

        }
    }
}
