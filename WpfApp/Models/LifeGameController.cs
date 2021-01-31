using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WpfApp.Models
{
    public class LifeGameController : ILifeGameController
    {
        protected SourceList<Cell> Cells { get; }

        public IObservable<int> Generation { get; }

        protected IObserver<int> GenerationObserver { get; set; }

        public Timer GenerationTimer { get; }

        public int Generations { get; protected set; }

        public LifeGameController()
        {
            Cells = new SourceList<Cell>();

            Generation = Observable.Create<int>(observer =>
            {
                GenerationObserver = observer;
                return Task.CompletedTask;
            });

            GenerationTimer = new Timer();
        }

        IObservable<IChangeSet<Cell>> ILifeGameController.Connect()
        {
            return Cells.Connect();
        }

        Task ILifeGameController.InitializeAsync(int columns, int rows)
        {
            return Task.Run(() =>
            {
                var cells = new Dictionary<Tuple<int, int>, Cell>();
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        cells.Add(Tuple.Create(x, y), new Cell(x, y, Generation));
                    }
                }

                foreach (var cell in cells.Values)
                {
                    if (cells.TryGetValue(Tuple.Create(cell.PositionX - 1, cell.PositionY), out var leftCell))
                    {
                        cell.SetLeft(leftCell);
                    }
                    if (cells.TryGetValue(Tuple.Create(cell.PositionX + 1, cell.PositionY), out var rightCell))
                    {
                        cell.SetRight(rightCell);
                    }
                    if (cells.TryGetValue(Tuple.Create(cell.PositionX, cell.PositionY - 1), out var upCell))
                    {
                        cell.SetUp(upCell);
                    }
                    if (cells.TryGetValue(Tuple.Create(cell.PositionX, cell.PositionY + 1), out var downCell))
                    {
                        cell.SetDown(downCell);
                    }
                }
           });
        }

        void ILifeGameController.Start(double generationInterval, params Cell[] initialAlives)
        {
            GenerationTimer.Interval = generationInterval;

            GenerationTimer.Elapsed += (s, e) =>
            {
                Generations++;
                GenerationObserver.OnNext(Generations);
            };

            Generations = 0;
            GenerationTimer.Start();
        }
    }
}
