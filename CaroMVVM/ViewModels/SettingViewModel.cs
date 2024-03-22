using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaroMVVM.ViewModels
{
    internal class SettingViewModel : ViewModelBase
    {
        public SettingViewModel() { Copy(Setting); }
        public override void Start()
        {
        }
        public override void Dispose()
        {
            Move(Setting);
        }

        public void Save() => Setting.Save(this);
    }
}
