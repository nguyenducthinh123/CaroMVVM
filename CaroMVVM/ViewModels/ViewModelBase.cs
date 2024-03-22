using MQTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public abstract class ViewModelBase : Document
    {
        static ViewModelBase old;
        public ViewModelBase() {
            old?.Dispose();
            old = this;
        }

        public event Action CaptionChanged;

        static Setting setting;
        static public Setting Setting
        {
            get
            {
                if (setting == null)
                {
                    setting = new Setting();
                }
                return setting;
            }
        }

        static Client client;
        public static Client Client
        {
            get
            {
                if (client == null)
                {
                    client = new Client("broker.emqx.io");
                }
                return client;
            }
        }

        string caption;
        public string Caption
        {
            get => caption;
            set
            {
                caption = value;
                CaptionChanged?.Invoke();
            }
        }

        public abstract void Start();
        public virtual void Dispose()
        {

        }

    }
}
