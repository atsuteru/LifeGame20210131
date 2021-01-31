using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class CellPanelViewModel : IActivatableViewModel
    {
        ViewModelActivator IActivatableViewModel.Activator => new ViewModelActivator();

        private ReadOnlyObservableCollection<Cell> _cells;
        public ReadOnlyObservableCollection<Cell> Cells => _cells;

        [Reactive]
        public bool IsAlive { get; set; }

        [Reactive]
        public int PositionY { get; set; }

        [Reactive]
        public int PositionX { get; set; }

        public CellPanelViewModel()
        {
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
                .Connect()
                .Filter(x => x.PositionX == PositionX && x.PositionY == PositionY)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _cells)
                .Subscribe()
                .DisposeWith(d)
                ;

            Cells
                .ToObservableChangeSet()
                .ToCollection()
                .Subscribe(cells =>
                {
                    var cell = cells.First();
                    IsAlive = cell.IsAlive;
                })
                .DisposeWith(d);
        }

        private void HandleDeactivation()
        {
        }
    }
}