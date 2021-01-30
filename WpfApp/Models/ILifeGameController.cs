using DynamicData;
using System;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public interface ILifeGameController
    {
        Task InitializeAsync(int columns, int rows);

        IObservable<IChangeSet<Cell>> Connect();
    }
}
