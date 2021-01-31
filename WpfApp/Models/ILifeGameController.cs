using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public interface ILifeGameController
    {
        Task InitializeAsync(int columns, int rows);

        IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> CellListener(int positionX, int positionY);

        void Start(double generationInterval, params Cell[] initialAlives);
    }
}
