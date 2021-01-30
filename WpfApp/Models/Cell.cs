using ReactiveUI.Fody.Helpers;

namespace WpfApp.Models
{
    public class Cell
    {
        [Reactive]
        public int PositionX { get; internal set; }

        [Reactive]
        public int PositionY { get; internal set; }
    }
}
