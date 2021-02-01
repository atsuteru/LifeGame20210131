using ReactiveUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public class LifeGameController : ILifeGameController
    {
        protected ConcurrentDictionary<Tuple<int, int>, Cell> Cells { get; }

        protected Subject<int> Generation { get; }

        public System.Timers.Timer GenerationTimer { get; }

        public int Generations { get; protected set; }

        public LifeGameController()
        {
            Cells = new ConcurrentDictionary<Tuple<int, int>, Cell>();

            Generation = new Subject<int>();

            GenerationTimer = new System.Timers.Timer();
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
                    Cells.AddOrUpdate(Tuple.Create(cell.PositionX, cell.PositionY), cell, (k,v) => cell);
                }
            });
        }

        IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> ILifeGameController.CellListener(int positionX, int positionY)
        {
            return Cells[Tuple.Create(positionX,positionY)].Changed;
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
                if (Cells.TryGetValue(Tuple.Create(initialAlive.PositionX, initialAlive.PositionY), out var cell))
                {
                    cell.IsAlive = true;
                }
            }

            Generations = 0;
            GenerationTimer.Start();
        }

        Task ILifeGameController.SetCellAlive(int positionX, int positionY, bool isAlive)
        {
            return Task.Run(() =>
            {
                if (!Cells.TryGetValue(Tuple.Create(positionX, positionY), out var cell))
                {
                    return Task.CompletedTask;
                }
                cell.IsAlive = isAlive;
                return Task.CompletedTask;
            }); 
        }
    }
}
