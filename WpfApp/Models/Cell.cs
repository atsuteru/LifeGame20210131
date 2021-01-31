using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace WpfApp.Models
{
    public class Cell : ReactiveObject
    {
        [Reactive]
        public bool IsAlive { get; set; }

        public int PositionX { get; }

        public int PositionY { get; }

        protected SourceList<Cell> AdjacentList { get; }

        private readonly ReadOnlyObservableCollection<Cell> _ajacents;
        public ReadOnlyObservableCollection<Cell> Ajacents => _ajacents;

        public int AdjacentAlives { get; protected set; }

        public Cell()
        {
            AdjacentList = new SourceList<Cell>();

            AdjacentList
                .Connect()
                .Bind(out _ajacents)
                .Subscribe();

            Ajacents
                .ToObservableChangeSet()
                .ToCollection()
                .Subscribe(ajacents =>
                {
                    AdjacentAlives = ajacents.Count(x => x.IsAlive);
                });
        }

        public Cell(int positionX, int positionY) : this()
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public Cell(int positionX, int positionY, IObservable<int> generation) : this(positionX, positionY)
        {
            generation.Subscribe(x =>
            {
                if (AdjacentAlives >= 3)
                {
                    IsAlive = true;
                }
                else if (AdjacentAlives < 2)
                {
                    IsAlive = false;
                }
                Debug.WriteLine($"[{DateTime.Now: HH:mm:ss}]Cell_Subscribe1[{x},({PositionX},{PositionY}),{IsAlive}]");
            });
        }

        public void SetLeft(Cell leftCell)
        {
            AdjacentList.Add(leftCell);
        }

        public void SetRight(Cell rightCell)
        {
            AdjacentList.Add(rightCell);
        }

        public void SetUp(Cell upCell)
        {
            AdjacentList.Add(upCell);
        }

        public void SetDown(Cell downCell)
        {
            AdjacentList.Add(downCell);
        }
    }
}
