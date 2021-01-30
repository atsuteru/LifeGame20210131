using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;

namespace WpfApp.ViewModels
{
    public class LifeGameCanvasViewModel : IActivatableViewModel
    {
        ViewModelActivator IActivatableViewModel.Activator => new ViewModelActivator();

        public LifeGameCanvasViewModel()
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
        }

        private void HandleDeactivation()
        {
        }
    }
}
