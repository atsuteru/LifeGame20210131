using DynamicData;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public interface ILifeGameController
    {
        Task InitializeAsync(int columns, int rows);

        IObservable<IChangeSet<Cell, Tuple<int, int>>> CellsWatcher();

        IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> CellWatcher(int positionX, int positionY);

        void Start(double generationInterval, params Cell[] initialAlives);

        Task SetCellAlive(int positionX, int positionY, bool isAlive);
    }
}
