using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTrack.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Windows.Threading;
    using FastTrack.Model;
    using FastTrack.Utilities;
    class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private bool m_IsFasting;
        private String m_FastButtonText;
        private String m_CurrentFastTimeText = "00:00:00";
        private ObservableCollection<FastEntryViewModel> m_FastEntries;
        private readonly DispatcherTimer m_CurrentFastTimer = new DispatcherTimer();

        public MainWindowViewModel(IEnumerable<FastEntryViewModel> fastEntries)
        {
            m_CurrentFastTimer.Tick += CurrentFastTimerOnTick;
            m_CurrentFastTimer.Interval = TimeSpan.FromSeconds(1);
            m_CurrentFastTimer.Start();
            m_FastEntries = new ObservableCollection<FastEntryViewModel>(fastEntries);
            m_FastEntries.CollectionChanged += (sender, args) => { OnPropertyChanged(nameof(FastEntriesReverse)); };
            if (m_FastEntries.Count > 0)
            {
                if (m_FastEntries.Last().EndTime == default(DateTime))
                {
                    m_IsFasting = true;
                }
            }
            else
            {
                m_IsFasting = false;
            }

            RefreshFastButtonText();
            RefreshCurrentFastTimeText();
        }

        public ObservableCollection<FastEntryViewModel> FastEntries { get { return m_FastEntries; } private set { SetProperty(ref m_FastEntries, value); OnPropertyChanged(nameof(FastEntriesReverse)); } }
        public IEnumerable<FastEntryViewModel> FastEntriesReverse => m_FastEntries.Reverse();
        public String FastButtonText { get { return m_FastButtonText; } private set { SetProperty(ref m_FastButtonText, value); } }
        public String CurrentFastTimeText { get { return m_CurrentFastTimeText; } private set { SetProperty(ref m_CurrentFastTimeText, value); } }

        private void CurrentFastTimerOnTick(object sender, EventArgs eventArgs)
        {
            RefreshCurrentFastTimeText();
        }

        public void OnFastButtonPressed()
        {
            if (m_IsFasting)
            {
                if (m_FastEntries.Count > 0)
                {
                    m_FastEntries.Last().EndTime = DateTime.Now;
                }
            }
            else
            {
                FastEntryViewModel vm = new FastEntryViewModel(new FastEntry());
                FastEntries.Add(vm);
                vm.StartTime = DateTime.Now;
            }

            m_IsFasting = !m_IsFasting;
            RefreshFastButtonText();
        }

        private void RefreshFastButtonText()
        {
            FastButtonText = m_IsFasting ? "End Fast" : "Start Fast";
        }

        private void RefreshCurrentFastTimeText()
        {
            if (m_IsFasting && m_FastEntries.Count > 0)
            {
                TimeSpan time = m_FastEntries.Last().StartTime - DateTime.Now;
                CurrentFastTimeText = time.ToString(@"hh\:mm\:ss");
            }
            else
            {
                CurrentFastTimeText = "00:00:00";
            }
        }
    }
}
