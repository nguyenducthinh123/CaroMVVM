using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class SinglePlayer : Game
    {
        public override void Start()
        {
            base.Start();
            Player.Rival = new Player();
            Player.Rival.Icon = 'o';
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
