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
            ViewModel = DataContext as CellPanelViewModel;

            this.OneWayBind(ViewModel, vm => vm.PositionX, v => v.PositionX).DisposeWith(d);
            this.OneWayBind(ViewModel, vm => vm.PositionY, v => v.PositionY).DisposeWith(d);
        }

        private void HandleDeactivation()
        {
        }
    }
}
