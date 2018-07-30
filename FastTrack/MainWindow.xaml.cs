using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FastTrack
{
    using FastTrack.Model;
    using FastTrack.Utilities;
    using FastTrack.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            List<FastEntry> entries = DataManager.Load(DataManager.MainDataFileName);
            DataContext = new MainWindowViewModel(entries.Select(e => new FastEntryViewModel(e)));
        }

        private void FastButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel vm = DataContext as MainWindowViewModel;
            vm?.OnFastButtonPressed();
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindowViewModel vm = DataContext as MainWindowViewModel;
            if (vm != null)
            {
                DataManager.Save(vm.FastEntries.Select(f => f.Model), DataManager.MainDataFileName);
            }

            base.OnClosed(e);
        }
    }
}
