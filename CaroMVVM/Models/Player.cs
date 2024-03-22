using MQTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class Player : Document
    {
        public Player Rival { get { return GetObject<Player>(nameof(Rival)); } set => Push(nameof(Rival), value); }

        public Player()
        {
            Icon = 'x';
        }

        //public string GetTopic(string);
    }
}
