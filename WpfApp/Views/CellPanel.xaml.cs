using ReactiveUI;
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// CellPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class CellPanel : ReactiveUserControl<CellPanelViewModel>
    {
        public CellPanel()
        {
            InitializeComponent();

            ViewModel = new CellPanelViewModel();
            DataContext = ViewModel;
        }
    }
}
