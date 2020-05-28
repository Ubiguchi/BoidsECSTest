namespace BoidsECSTest.NezEcs.Systems
{
	using BoidsECSTest.NezEcs.Components;
    using Microsoft.Xna.Framework;
    using Nez;
    using System;
    using System.Threading.Tasks;

    public struct CellBehavior
	{
		public readonly Vector2 Center;
		public readonly Vector2 Direction;
		public readonly int Count;

		public CellBehavior(Vector2 center, Vector2 direction, int count)
		{
			Center = center;
			Direction = direction;
			Count = count;
		}
	}

	public class BehaviorSystem : ProcessingSystem
	{
		private CellBehavior[,] grid;
		private readonly int cellSize;
		private readonly int columnCount;
		private readonly int rowCount;
		private readonly ParallelOptions parallelOptions;

		public BehaviorSystem(int rowCount, int columnCount, int cellSize)
		{
			this.cellSize = cellSize;
			this.columnCount = columnCount;
			this.rowCount = rowCount;
			grid = new CellBehavior[rowCount, columnCount];

			parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = TestConfiguration.DegreeOfParallelism
			};
		}

		public override void Process()
		{
			CalculateCellBehavior();
			var aggregatedGrid = CreateAggregatedGrid();
			grid = aggregatedGrid;
		}

		private void CalculateCellBehavior()
		{
			void CaclulateRow(int row)
			{
				for (int column = 0; column < columnCount; column++)
				{
					// Get boids in the cell and aggregate
					var center = Vector2.Zero;
					var direction = Vector2.Zero;
					int count = 0;

					var boundingBox = new RectangleF(column * cellSize, row * cellSize, cellSize, cellSize);
					var neighbors = Physics.BoxcastBroadphase(boundingBox);

					foreach (var neighbor in neighbors)
					{
						center += neighbor.GetComponent<DrawInfo>().Position;
						direction += neighbor.GetComponent<Velocity>().Value;
						count++;

						var cellId = neighbor.Entity.GetComponent<CellId>();
						cellId.X = column;
						cellId.Y = row;
					}

					grid[row, column] = new CellBehavior(center, direction, count);
				}
			}

			var singleThreadedOptions = new ParallelOptions()
			{
				MaxDegreeOfParallelism = 1
			};

			Parallel.For(0, rowCount, singleThreadedOptions, CaclulateRow);
		}

		private CellBehavior[,] CreateAggregatedGrid()
		{
			var aggregatedGrid = new CellBehavior[rowCount, columnCount];

			void CaclulateRow(int row)
			{
				var minRow = Math.Max(0, row - 1);
				var maxRow = Math.Min(rowCount - 1, row + 1);

				for (int column = 0; column < columnCount; column++)
				{
					var minColumn = Math.Max(0, column - 1);
					var maxColumn = Math.Min(columnCount - 1, column + 1);

					aggregatedGrid[row, column] = AggregateCellBehaviors(minRow, maxRow, minColumn, maxColumn);
				}
			}

			Parallel.For(0, rowCount, parallelOptions, CaclulateRow);

			return aggregatedGrid;
		}

		private CellBehavior AggregateCellBehaviors(int minRow, int maxRow, int minColumn, int maxColumn)
		{
			var center = Vector2.Zero;
			var direction = Vector2.Zero;
			int count = 0;

			for (int row = minRow; row <= maxRow; row++)
			{
				for (int column = minColumn; column <= maxColumn; column++)
				{
					var cell = grid[row, column];
					center += cell.Center;
					direction += cell.Direction;
					count += cell.Count;

				}
			}

			return new CellBehavior(center, direction, count);
		}

		public CellBehavior GetCellBehavior(int row, int column)
		{
			return grid[row, column];
		}
	}
}