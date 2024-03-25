using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class PassiveGame : Game
    {
        public event Action<Document> Success;
        public event Action<Document> FoundCallback;
        public event Action<Document> ReadyCallback;
        public void Connect()
        {
            Broker.Connect();
        }

        public PassiveGame()
        {
            Flag = false; // passive game nên Flag = false
            Connect();

            Broker.Connected += () =>
            {
                Caption = "Success Connect to MQTT !!!";
                Success?.Invoke(this);
                Caption = "Choose Player";
            };
        }

        static PassiveGame game;
        public static PassiveGame Game
        {
            get
            {
                if (game == null)
                {
                    game = new PassiveGame();
                }
                return game;
            }
        }

        public override void Start()
        {
            base.Start();
            Broker.Listen("new-game", (doc) => {
                FoundCallback?.Invoke(doc);
            });
        }

        public void SendJoin()
        {
            ObjectId = Broker.ID;
            Name = "Passive Debug";
            Broker.Send("join", this);
            Broker.Listen("ready/" + ObjectId, (doc) => {
                Caption = "Play Passive Game";
                ReadyCallback?.Invoke(doc);
            });
        }
    }
}
