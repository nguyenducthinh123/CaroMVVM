using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class GameOffline : Game
    {
        public override object GetBindingData()
        {
            var gameOffline = (GameOffline)(Copy(Setting));
            return gameOffline;
        }

        public override void Start()
        {
            base.Start();
            Caption = "Play Single Game !!!";
            Player.Rival = new Player { Icon = 'o'}; // C# mới có kiểu khởi tạo thế này
            //Player.Rival.Icon = 'o';
            PutFirstPlayer();
        }

        //static SinglePlayer game;
        //public static SinglePlayer Game
        //{
        //    get
        //    {
        //        if (game == null)
        //        {
        //            game = new SinglePlayer();
        //        }
        //        return game;
        //    }
        //}

        protected override void SwitchPlayer()
        {
            var temp = Player;
            Player = Player.Rival;
            Player.Rival = temp;
        }
    }
}
