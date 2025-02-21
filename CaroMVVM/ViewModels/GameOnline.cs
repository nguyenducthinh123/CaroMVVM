using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace System
{
    public class GameOnline : Game
    {

        // event dùng cho Proactive
        public event Action<Document> JoinCallback;
        public event Action<Document> ReadyPlay;

        // event dùng cho Passive
        public event Action<Document> FoundCallback;
        public event Action<Document> ReadyCallback;

        public bool IsProactive;
        bool send_flag = true;
        public bool MyTurn = true;
        public bool FirstMove = true;
        
        public string rival_id;

        public GameOnline(bool isProactive = true)
        {
            IsProactive = isProactive;
            ObjectId = IsProactive ? "proactive" : "passive"; // dùng để debug thôi, sau này thay bằng Broker.ID
            Name = IsProactive ? "pro debug" : "passive debug"; // dùng để debug thôi, sau này gán bằng Setting.Name
        }

        public override object GetBindingData()
        {
            var gameOnline = (GameOnline)(Copy(Setting));
            //gameOnline.IsProactive = IsProactive;
            //gameOnline.ObjectId = ObjectId;
            //gameOnline.Name = Name;

            return gameOnline;
        }

        public override void Start()
        {
            base.Start();
            IsWin = false;

            if (IsProactive)
            {
                Broker.Listen("join/" + ObjectId, (doc) =>
                {
                    JoinCallback?.Invoke(doc);

                });
                Task.Run(() => {
                    while (true)
                    {
                        var doc = new Document { ObjectId = this.ObjectId, Name = this.Name };
                        Broker.Send("new-game", doc);
                        Thread.Sleep(1000);
                        if (!send_flag)
                        {
                            break;
                        }
                    }
                });
            }
            else
            {
                Broker.Listen("new-game", (doc) => {
                    FoundCallback?.Invoke(doc);
                });
            }
        }

        // hàm này cho Proactive
        public void SendReady(string id)
        {
            send_flag = false;
            string topic = "ready/" + id;
            var doc = new Document { ObjectId = this.ObjectId, Name = this.Name };
            Broker.Send(topic, doc);

            Caption = "Playing Proactive Game";
            ReadyPlay?.Invoke(doc);

            rival_id = id;
        }

        // hàm này cho Passive
        public void SendJoin(string id)
        {
            var doc = new Document { ObjectId = this.ObjectId, Name = this.Name };
            Broker.Send("join/" + id, doc);

            Broker.Listen("ready/" + ObjectId, (doc) => {
                Caption = "Play Passive Game";
                ReadyCallback?.Invoke(doc);

                rival_id = doc.ObjectId;
            });
        }

        public void PlayWith(string id)
        {
            string play_topic = "play/" + id;
            Broker.Listen(play_topic, (doc) => {
                Task.Run(() => {
                    PutAndCheckOver(doc.Row, doc.Column);
                    Caption = $"Row = {doc.Row}, Col = {doc.Column}";
                    if (doc.IsWin)
                    {
                        RaiseGameOver(doc);
                        return;
                    }
                    MyTurn = true;
                });
            });
            if (IsProactive)
            {
                Player = new Player { ObjectId = this.ObjectId, Icon = 'x' };
                Player.Rival = new Player { ObjectId = id, Icon = 'o' };
                CellMatrix = new CellMatrix(Setting.SizeOnline, Setting.ConsecutiveCountOnline);
                PutFirstPlayer();
                FirstMove = false;
            }
            else
            {
                Player = new Player { ObjectId = this.ObjectId, Icon = 'o' };
                Player.Rival = new Player { ObjectId = id, Icon = 'x' };

                PutFirstByRival(Player.Rival);
                Caption = $"Row = {Setting.Size >> 1}, Col = {Setting.Size >> 1}";
                FirstMove = false;
            }

        }

        public void SendMove(int r, int c, bool isWin)
        {
            var doc = new Document { Row = r, Column = c, IsWin = isWin };
            Broker.Send("play/" + ObjectId, doc);
        }

        protected override void SwitchPlayer()
        {
            var temp = Player;
            Player = Player.Rival;
            Player.Rival = temp;

            MyTurn = MyTurn ? false : true;
        }
    }
}
