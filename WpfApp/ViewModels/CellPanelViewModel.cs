using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
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

        public ReactiveCommand<bool, Unit> ChangeAliveCommand { get; }

        [Reactive]
        public bool IsAlive { get; set; }

        public int PositionY { get; set; }

        public int PositionX { get; set; }

        public CellPanelViewModel()
        {
            Activator = new ViewModelActivator();

            ChangeAliveCommand = ReactiveCommand.CreateFromTask<bool>(isAlive =>
            {
                return Locator.Current.GetService<ILifeGameController>()?
                    .SetCellAlive(PositionX, PositionY, isAlive);
            });

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
            Locator.Current.GetService<ILifeGameController>()?
                .CellWatcher(PositionX, PositionY)
                .Subscribe(e =>
                {
                    IsAlive = ((Cell)e.Sender).IsAlive;
                })
                .DisposeWith(d);
        }

        private void HandleDeactivation()
        {
        }
    }
}