using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class CellPanelViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        private ReadOnlyObservableCollection<Cell> _cells;
        public ReadOnlyObservableCollection<Cell> Cells => _cells;

        [Reactive]
        public bool IsAlive { get; set; }

        public int PositionY { get; set; }

        public int PositionX { get; set; }

        public CellPanelViewModel()
        {
            Activator = new ViewModelActivator();

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
            Locator.Current.GetService<ILifeGameController>()
                .CellListener(PositionX, PositionY)
                .Subscribe(e =>
                {
                    var cell = e.Sender as Cell;
                    IsAlive = cell.IsAlive;
                })
                .DisposeWith(d);
        }

        private void HandleDeactivation()
        {
        }
    }
}