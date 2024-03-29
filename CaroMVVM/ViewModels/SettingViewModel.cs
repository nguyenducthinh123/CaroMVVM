using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    internal class SettingViewModel : ViewModelBase
    {
        public override object GetBindingData()
        {
            return Copy(Setting);
        }

        public override void Start()
        {
            Caption = "";
        }

        public override void Dispose()
        {
            Move(Setting);
        }

        public void Save() => Setting.Save(this);
    }
}
