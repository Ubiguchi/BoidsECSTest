using System;
using System.Collections;
using System.Collections.Generic;
using DefaultEcs;
using Microsoft.Xna.Framework;

namespace BoidsECSTest.DefaultEcs.Components
{
    public readonly struct CellId : IEquatable<CellId>
    {
        public readonly int X;
        public readonly int Y;

        public CellId(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode() => X + (Y * 1000);

        public override bool Equals(object obj) => obj is CellId other && Equals(other);

        public bool Equals(CellId other) => X == other.X && Y == other.Y;

        public override string ToString() => $"{X} {Y}";
    }

    public static class GridIdExtension
    {
        private const int _width = TestConfiguration.CellColumnCount;
        private const int _height = TestConfiguration.CellRowCount;
        private const int _cellWidth = TestConfiguration.CellWidth;
        private const int _cellHeight = TestConfiguration.CellHeight;

        public struct Enumerable : IEnumerable<CellId>
        {
            private readonly CellId _id;

            public Enumerable(CellId id)
            {
                _id = id;
            }

            public Enumerator GetEnumerator() => new Enumerator(_id);

            IEnumerator<CellId> IEnumerable<CellId>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public struct Enumerator : IEnumerator<CellId>
        {
            private readonly int minI;
            private readonly int minJ;
            private readonly int maxI;
            private readonly int maxJ;

            private int currentI;
            private int currentJ;

            public Enumerator(CellId id)
            {
                minI = currentI = Math.Max(0, id.X - 1);
                minJ = currentJ = Math.Max(0, id.Y - 1);

                --currentI;
                Current = default;

                maxI = Math.Min(_width - 1, id.X + 1);
                maxJ = Math.Min(_height - 1, id.Y + 1);
            }

            public bool MoveNext()
            {
                if (++currentI > maxI)
                {
                    if (++currentJ > maxJ)
                    {
                        return false;
                    }

                    currentI = minI;
                }

                Current = new CellId(currentI, currentJ);

                return true;
            }

            public void Reset()
            {
                currentI = minI - 1;
                currentJ = minJ;
            }

            public void Dispose()
            { }

            public CellId Current { get; private set; }

            object IEnumerator.Current => Current;
        }

        public static Enumerable GetNeighbors(this CellId id) => new Enumerable(id);

        public static CellId ToGridId(this Vector2 position) => new CellId((int)Math.Clamp(position.X / _cellWidth, 0, _width - 1), (int)Math.Clamp(position.Y / _cellHeight, 0, _height - 1));

        public static void CreateBehaviors(this World world)
        {
            for (int i = 0; i < _width; ++i)
            {
                for (int j = 0; j < _height; ++j)
                {
                    Entity entity = world.CreateEntity();
                    entity.Set(new CellId(i, j));
                    entity.Set<Behavior>();
                }
            }
        }
    }
}
