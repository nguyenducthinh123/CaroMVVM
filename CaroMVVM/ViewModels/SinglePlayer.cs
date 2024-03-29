﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class SinglePlayer : Game
    {
        public override object GetBindingData()
        {
            return Copy(Setting);
        }

        public override void Start()
        {
            base.Start();
            Caption = "Play Single Game !!!";
            Player.Rival = new Player { Icon = 'o'}; // C# mới có kiểu khởi tạo thế này
            //Player.Rival.Icon = 'o';
            PutFirstPlayer();
        }

        public override void Dispose()
        {
            Move(Setting);
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
