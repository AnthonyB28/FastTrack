using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrack.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using FastTrack.Model;
    using FastTrack.Utilities;

    class FastEntryViewModel : NotifyPropertyChangedBase
    {
        private readonly FastEntry m_Model;
        private ObservableCollection<FloatViewModel> m_Weights;
        private double m_DaysFasted = 0.0;

        public FastEntryViewModel(FastEntry model)
        {
            m_Model = model; 
            m_Weights = new ObservableCollection<FloatViewModel>();
            foreach (float weight in model.Weights)
            {
                FloatViewModel vm = new FloatViewModel(weight);
                vm.PropertyChanged += OnWeightValueChanged;
                m_Weights.Add(vm);
            }

            // Better ways of doing this, but yolo
            m_Weights.CollectionChanged += (sender, args) =>
            {
                m_Model.Weights = m_Weights.Select(f => f.Value).ToList();
                OnPropertyChanged(nameof(StartWeight));
                OnPropertyChanged(nameof(EndWeight));
            };

            RefreshWeights();
            RefreshDaysFasted();
        }

        public FastEntry Model => m_Model;

        public DateTime StartTime
        {
            get { return m_Model.StartTime; }
            set
            {
                m_Model.StartTime = value;
                OnPropertyChanged("StartTime");
                RefreshWeights();
                RefreshDaysFasted();
            }
        }

        public DateTime EndTime
        {
            get { return m_Model.EndTime; }
            set
            {
                m_Model.EndTime = value;
                OnPropertyChanged("EndTime");
                RefreshWeights();
                RefreshDaysFasted();
            }
        }

        public double DaysFasted { get { return m_DaysFasted; } private set { SetProperty(ref m_DaysFasted, value); } }

        public ObservableCollection<FloatViewModel> Weights
        {
            get
            {
                return m_Weights;
            }

            set
            {
                if (SetProperty(ref m_Weights, value))
                {
                    m_Model.Weights = m_Weights.Select(f => f.Value).ToList();
                }

                OnPropertyChanged(nameof(StartWeight));
                OnPropertyChanged(nameof(EndWeight));
            }
        }

        public float StartWeight
        {
            get
            {
                if (Weights.Count > 0)
                {
                    return Weights.First().Value;
                }

                return float.NaN;
            }
        }

        public float EndWeight
        {
            get
            {
                if (Weights.Count > 0)
                {
                    return Weights.Last().Value;
                }

                return float.NaN;
            }
        }

        private void RefreshDaysFasted()
        {
            if (StartTime > EndTime)
            {
                DaysFasted = 0;
            }
            else
            {
                DaysFasted = (EndTime - StartTime).TotalDays;
            }
        }

        private void RefreshWeights()
        {
            int numOfWeightSlots = 1;
            if (EndTime >= StartTime)
            {
                numOfWeightSlots = (int)(EndTime - StartTime).TotalDays + 1;
            }
            else
            {
                numOfWeightSlots = (int)(DateTime.Now - StartTime).TotalDays + 1;
            }

            ObservableCollection<FloatViewModel> newWeights = new ObservableCollection<FloatViewModel>();
            float lastValue = float.NaN;
            for (int i = 0; i < numOfWeightSlots; ++i)
            {
                if (m_Weights.Count != 0 && i < m_Weights.Count)
                {
                    lastValue = m_Weights[i].Value;
                    newWeights.Add(m_Weights[i]);
                }
                else
                {
                    FloatViewModel vm = new FloatViewModel(lastValue);
                    vm.PropertyChanged += OnWeightValueChanged;
                    newWeights.Add(vm);
                }
            }

            for (int i = numOfWeightSlots; i < m_Weights.Count; ++i)
            {
                m_Weights[i].PropertyChanged -= OnWeightValueChanged;
            }

            Weights = newWeights;
        }

        private void OnWeightValueChanged(object o, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnPropertyChanged(nameof(StartWeight));
            OnPropertyChanged(nameof(EndWeight));
        }
    }
}
