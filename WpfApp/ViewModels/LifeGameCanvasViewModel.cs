using DynamicData;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class LifeGameCanvasViewModel : IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        public ReadOnlyObservableCollection<Cell> Cells => _cells;
        private ReadOnlyObservableCollection<Cell> _cells;

        public LifeGameCanvasViewModel()
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
            Locator.Current.GetService<ILifeGameController>()?
                .CellsWatcher().Bind(out _cells).Subscribe();
        }

        private void HandleDeactivation()
        {
        }
    }
}
