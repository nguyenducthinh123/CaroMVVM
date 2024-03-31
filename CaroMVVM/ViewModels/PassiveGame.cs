//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace System
//{
//    public class PassiveGame : Game
//    {
//        public event Action<Document> Success;
//        public event Action<Document> FoundCallback;
//        public event Action<Document> ReadyCallback;

//        public bool my_turn = false;
//        static public string rival_id;

//        public void Connect()
//        {
//            Broker.Connect();
//        }

//        public PassiveGame()
//        {
//            Flag = false; // passive game nên Flag = false
//            Connect();

//            Broker.Connected += () =>
//            {
//                Caption = "Success Connect to MQTT !!!";
//                Success?.Invoke(this);
//                Caption = "Choose Player";
//            };
//        }

//        static PassiveGame game;
//        public static PassiveGame Game
//        {
//            get
//            {
//                if (game == null)
//                {
//                    game = new PassiveGame();
//                }
//                return game;
//            }
//        }

//        public override void Start()
//        {
//            base.Start();
//            IsWin = false;
//            Broker.Listen("new-game", (doc) => {
//                FoundCallback?.Invoke(doc);
//            });
//        }

//        public void SendJoin(string id)
//        {
//            ObjectId = "12345";
//            Name = "Passive Debug";
//            Broker.Send("join/" + id, this);
//            Broker.Listen("ready/" + ObjectId, (doc) => {
//                Caption = "Play Passive Game";
//                ReadyCallback?.Invoke(doc);

//                rival_id = doc.ObjectId;
//            });
//        }

//        public void PlayPassiveGame(string id)
//        {
//            Player = new Player { ObjectId = this.ObjectId, Icon = 'x' };
//            Player.Rival = new Player { ObjectId = id, Icon = 'o' };
//            string play_topic = "play/" + id;
//            Broker.Listen(play_topic, (doc) => {
//                PutAndCheckOver(doc.Row, doc.Column);
//                if (doc.IsWin) RaiseGameOver(doc);
//                Caption = $"Row = {doc.Row}, Col = {doc.Column}";
//                my_turn = true;
//            });
           
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
