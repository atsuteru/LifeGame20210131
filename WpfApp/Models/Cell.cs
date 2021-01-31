using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace WpfApp.Models
{
    public class Cell : ReactiveObject
    {
        private struct AliveStatus
        {
            public int Generation { get; set; }

            public bool IsAlive { get; set; }
        }

        public int PositionX { get; }

        public int PositionY { get; }

        public int CurrentGeneration { get; private set; }

        [Reactive]
        public bool IsAlive { get; set; }

        private ConcurrentDictionary<Tuple<int,int>, AliveStatus> Ajacents { get; }

        public Cell()
        {
            Ajacents = new ConcurrentDictionary<Tuple<int, int>, AliveStatus>();
            CurrentGeneration = 0;
        }

        public Cell(int positionX, int positionY) : this()
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public Cell(int positionX, int positionY, IObservable<int> generation) : this(positionX, positionY)
        {
            generation.Subscribe(generationNumber =>
            {
                CurrentGeneration = generationNumber;

                var adjacentAlives = Ajacents.Values.Count(alives => 
                        (alives.Generation < generationNumber && alives.IsAlive == true) ||
                        (alives.Generation == generationNumber && alives.IsAlive == false));
                if (adjacentAlives == 3) // 誕生
                {
                    IsAlive = true;
                }
                else if (adjacentAlives < 2) // 過疎
                {
                    IsAlive = false;
                }
                else if (adjacentAlives > 3) // 過密
                {
                    IsAlive = false;
                }
            });
        }

        public void SetAjacent(Cell ajacentCell)
        {
            AddOrUpdateAjacents(ajacentCell);
            ajacentCell.Changed.Subscribe(e =>
            {
                AddOrUpdateAjacents(e.Sender as Cell);
            });
        }

        private void AddOrUpdateAjacents(Cell cell)
        {
            Ajacents.AddOrUpdate(
                Tuple.Create(cell.PositionX, cell.PositionY),
                new AliveStatus() { Generation = 0, IsAlive = cell.IsAlive },
                (k, v) =>
                {
                    v.Generation = cell.CurrentGeneration;
                    v.IsAlive = cell.IsAlive;
                    return v;
                });
        }

        public override string ToString()
        {
            var builder = new StringBuilder(base.ToString());
            builder.Append($",{nameof(PositionX)}:{PositionX}");
            builder.Append($",{nameof(PositionY)}:{PositionY}");
            builder.Append($",{nameof(CurrentGeneration)}:{CurrentGeneration}");
            builder.Append($",{nameof(IsAlive)}:{IsAlive}");
            return builder.ToString();
        }

    }
}
