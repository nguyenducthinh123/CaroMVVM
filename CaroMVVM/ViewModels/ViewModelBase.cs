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

        static Broker broker;
        static public Broker Broker
        {
            get
            {
                if (broker == null)
                {
                    broker = new Broker();
                }
                return broker;
            }
        }

        public virtual object GetBindingData() => this;
        public abstract void Start();
        public virtual void Dispose() { }

    }
}
