using MQTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class ProactiveGame : Game
    {
        public event Action<Document> Success;
        public event Action<Document> JoinCallback;
        public event Action<Document> ReadyPlay;
        bool send_flag = true;

        public void Connect()
        {
            Broker.Connect();
        }

        public ProactiveGame()
        {
            Connect();
            Broker.Connected += () =>
            {
                Caption = "Success Connect to MQTT !!!";
                Success?.Invoke(this);
                Caption = "Choose Player";
            };
        }

        static ProactiveGame game;
        public static ProactiveGame Game
        {
            get
            {
                if (game == null)
                {
                    game = new ProactiveGame();
                }
                return game;
            }
        }

        public override void Start()
        {
            base.Start();
            Broker.Listen("join", (doc) =>
            {
                JoinCallback?.Invoke(doc);
            });
            Task.Run(() => { 
                while (true)
                {
                    var doc = new Document { ObjectId = Broker.ID, Name = "Debug" };
                    Broker.Send("new-game", doc);
                    Thread.Sleep(1000);
                    if (!send_flag)
                    {
                        break;
                    }
                }
            });
        }

        public void SendReady(string id)
        {
            send_flag = false;
            ObjectId = Broker.ID;
            Name = "Debug";
            string topic = "ready/" + id;
            Broker.Send(topic, this);

            Caption = "Playing Proactive Game";
            ReadyPlay?.Invoke(this);
        }

    }
}
