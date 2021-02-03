using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public class LifeGameController : ILifeGameController
    {
        protected SourceCache<Cell, Tuple<int, int>> Cells { get; }

        protected Subject<int> Generation { get; }

        public System.Timers.Timer GenerationTimer { get; }

        public int Generations { get; protected set; }

        public LifeGameController()
        {
            Cells = new SourceCache<Cell, Tuple<int, int>>(cell => Tuple.Create(cell.PositionX, cell.PositionY));

            Generation = new Subject<int>();

            GenerationTimer = new System.Timers.Timer();
        }

        IObservable<IChangeSet<Cell, Tuple<int, int>>> ILifeGameController.CellsWatcher()
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
                    foreach (var offset in new (int, int)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) })
                    {
                        if (cells.TryGetValue(Tuple.Create(cell.PositionX + offset.Item1, cell.PositionY + offset.Item2), out var ajacentCell))
                        {
                            cell.SetAjacent(ajacentCell);
                        }
                    }
                }
                foreach (var cell in cells.Values)
                {
                    Cells.AddOrUpdate(cell);
                }
            });
        }

        IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> ILifeGameController.CellWatcher(int positionX, int positionY)
        {
            return Cells.Lookup(Tuple.Create(positionX, positionY)).Value.Changed;
        }

        void ILifeGameController.Start(double generationInterval, params Cell[] initialAlives)
        {
            GenerationTimer.Interval = generationInterval;

            GenerationTimer.Elapsed += (s, e) =>
            {
                Generations++;
                Generation.OnNext(Generations);
            };

            foreach (var initialAlive in initialAlives)
            {
                var cellRef = Cells.Lookup(Tuple.Create(initialAlive.PositionX,initialAlive.PositionY));
                if (!cellRef.HasValue)
                {
                    continue;
                }
                cellRef.Value.IsAlive = true;
            }

            Generations = 0;
            GenerationTimer.Start();
        }

        Task ILifeGameController.SetCellAlive(int positionX, int positionY, bool isAlive)
        {
            return Task.Run(() =>
            {
                var cellRef = Cells.Lookup(Tuple.Create(positionX, positionY));
                if (!cellRef.HasValue)
                {
                    return Task.CompletedTask;
                }
                cellRef.Value.IsAlive = isAlive;
                return Task.CompletedTask;
            }); 
        }
    }
}
