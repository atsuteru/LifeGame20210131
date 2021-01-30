using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public class LifeGameController : ILifeGameController
    {
        protected SourceList<Cell> Cells { get; }

        public LifeGameController()
        {
            Cells = new SourceList<Cell>();
        }

        IObservable<IChangeSet<Cell>> ILifeGameController.Connect()
        {
            return Cells.Connect();
        }

        Task ILifeGameController.InitializeAsync(int columns, int rows)
        {
            return Task.Run(() =>
            {
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        Cells.Add(new Cell()
                        {
                            PositionX = x,
                            PositionY = y,
                        });
                    }
                }
            });
        }
    }
}
