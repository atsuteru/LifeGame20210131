using ReactiveUI;
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
            ViewModel = DataContext as MainViewModel;

            this.WhenActivated(d =>
            {
            });
        }
    }
}
