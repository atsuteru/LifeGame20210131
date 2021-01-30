using ReactiveUI.Fody.Helpers;

namespace WpfApp.ViewModels
{
    public class CellPanelViewModel
    {
        [Reactive]
        public bool IsAlive { get; set; }
    }
}