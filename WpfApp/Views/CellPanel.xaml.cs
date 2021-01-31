using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;
using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// CellPanel.xaml の相互作用ロジック
    /// </summary>
    public partial class CellPanel : ReactiveUserControl<CellPanelViewModel>
    {
        [Reactive]
        public bool IsAlive { get; set; }

        public int PositionX
        {
            get
            {
                return _positionX;
            }
            set
            {
                _positionX = value;
                SetValue(Grid.ColumnProperty, _positionX);
            }
        }
        private int _positionX;

        public int PositionY
        {
            get
            {
                return _positionY;
            }
            set
            {
                _positionY = value;
                SetValue(Grid.RowProperty, _positionY);
            }
        }
        private int _positionY;

        public CellPanel()
        {
            InitializeComponent();

            ViewModel = new CellPanelViewModel();
            DataContext = ViewModel;

            this.WhenActivated(d =>
            {
                HandleActivation(d);
                Disposable
                    .Create(() => this.HandleDeactivation())
                    .DisposeWith(d);
            });
        }

        private void HandleActivation(CompositeDisposable d)
        {
            ViewModel.PositionX = PositionX;
            ViewModel.PositionY = PositionY;

            this.Bind(ViewModel, vm => vm.IsAlive, v => v.IsAlive).DisposeWith(d);
        }

        private void HandleDeactivation()
        {
        }
    }
}
