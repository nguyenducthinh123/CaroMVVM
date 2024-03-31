//using MQTT;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Threading;

//namespace System
//{
//    public class ProactiveGame : Game
//    {
//        public event Action<Document> Success;
//        public event Action<Document> JoinCallback;
//        public event Action<Document> ReadyPlay;
//        bool send_flag = true;
//        public bool my_turn = true;
//        public static string rival_id;

//        public void Connect()
//        {
//            Broker.Connect();
//        }

//        public ProactiveGame()
//        {
//            Connect();
//            Broker.Connected += () =>
//            {
//                Caption = "Success Connect to MQTT !!!";
//                Success?.Invoke(this);
//                Caption = "Choose Player";
//            };
//        }

//        static ProactiveGame game;
//        public static ProactiveGame Game
//        {
//            get
//            {
//                if (game == null)
//                {
//                    game = new ProactiveGame();
//                }
//                return game;
//            }
//        }

//        public override void Start()
//        {
//            base.Start();
//            IsWin = false;
//            ObjectId = "23456";
//            Broker.Listen("join/" + ObjectId, (doc) =>
//            {
//                JoinCallback?.Invoke(doc);
//            });
//            Task.Run(() => { 
//                while (true)
//                {
//                    var doc = new Document { ObjectId = "23456", Name = "Debug" };
//                    Broker.Send("new-game", doc);
//                    Thread.Sleep(1000);
//                    if (!send_flag)
//                    {
//                        break;
//                    }
//                }
//            });
//        }

//        public void SendReady(string id)
//        {
//            send_flag = false;
//            Name = "Debug";
//            string topic = "ready/" + id;
//            Broker.Send(topic, this);

//            Caption = "Playing Proactive Game";
//            ReadyPlay?.Invoke(this);

//            rival_id = id;
//        }

//        public void PlayProactiveGame(string id)
//        {
//            Player = new Player { ObjectId = id, Icon = 'x' };
//            Player.Rival = new Player { ObjectId = this.ObjectId, Icon = 'o' };
//            string play_topic = "play/" + id;
//            Broker.Listen(play_topic, (doc) => {
//                PutAndCheckOver(doc.Row, doc.Column);
//                if (doc.IsWin) RaiseGameOver(doc);
//                Caption = $"Row = {doc.Row}, Col = {doc.Column}";
//                my_turn = true;
//            });
//            PutFirstPlayer();

//        }

//        protected override void SwitchPlayer()
//        {
//            base.SwitchPlayer();

//            var temp = Player;
//            Player = Player.Rival;
//            Player.Rival = temp;

//            my_turn = false;
//        }

//        public void SendMove(int r, int c, bool isWin)
//        {
//            var doc = new Document { Row = r, Column = c, IsWin = isWin };
//            Broker.Send("play/" + ObjectId, doc);
//        }
//    }
//}
