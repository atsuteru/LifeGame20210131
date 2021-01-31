using ReactiveUI;
using System.Reactive.Disposables;

namespace WpfApp.ViewModels
{
    public class LifeGameCanvasViewModel : IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

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
        }

        private void HandleDeactivation()
        {
        }
    }
}
