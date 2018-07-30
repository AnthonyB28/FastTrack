using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrack.ViewModel
{
    using FastTrack.Utilities;

    class FloatViewModel : NotifyPropertyChangedBase
    {
        private float m_Value;

        public FloatViewModel(float value)
        {
            m_Value = value;
        }

        public float Value { get { return m_Value; } set { SetProperty(ref m_Value, value); } }
    }
}
