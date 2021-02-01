using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class MainViewModel : IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        public MainViewModel()
        {
            Activator = new ViewModelActivator();

            Locator.Current.GetService<ILifeGameController>()?
                .InitializeAsync(columns: 40, rows: 40).Wait();

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
            var aliveCells = new List<Cell>();
            var random = new Random();
            for (int i = 0; i < 300; i++)
            {
                aliveCells.Add(new Cell(random.Next(1, 40), random.Next(1, 40)));
            }

            Locator.Current.GetService<ILifeGameController>()?
                .Start(
                    100.0,
                    aliveCells.ToArray()
                );
        }

        private void HandleDeactivation()
        {
        }

    }
}
