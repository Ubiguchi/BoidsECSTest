namespace BoidsECSTest.NezEcs.Systems
{
	using BoidsECSTest.NezEcs.Components;
	using Nez;
	using Microsoft.Xna.Framework;
	using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class BoidsSystem : EntitySystem
	{
		private BehaviorSystem behaviorSystem;
		private readonly ParallelOptions parallelOptions;

		public BoidsSystem(BehaviorSystem behaviorSystem)
			: base(new Matcher().All(typeof(DrawInfo), typeof(Acceleration), typeof(Velocity)))
		{
			this.behaviorSystem = behaviorSystem;
			parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = TestConfiguration.DegreeOfParallelism
			};
		}

		protected override void Process(List<Entity> entities)
		{
			void ProcessEntity(int index)
			{
				var entity = entities[index];
				Vector2 position = entity.GetComponent<DrawInfo>().Position;
				Vector2 velocity = entity.GetComponent<Velocity>().Value;

				Vector2 separation = Vector2.Zero;
				Vector2 alignment = Vector2.Zero;
				Vector2 cohesion = Vector2.Zero;
				int neighborCount = 0;

				var cellId = entity.GetComponent<CellId>();
				var behavior = behaviorSystem.GetCellBehavior(cellId.Y, cellId.X);

				if (behavior.Count > 0)
				{
					Vector2 offset = (position * behavior.Count) - behavior.Center;
					if (offset != Vector2.Zero)
					{
						separation += Vector2.Normalize(offset);
					}
				}

				alignment += behavior.Direction;
				cohesion += behavior.Center;
				neighborCount += behavior.Count;

				if (neighborCount > 1)
				{
					alignment = (alignment / neighborCount) - velocity;
					cohesion = position - (cohesion / neighborCount);
				}

				var acceleration =
					(separation * TestConfiguration.BehaviorSeparationWeight)
					+ (alignment * TestConfiguration.BehaviorAlignmentWeight)
					+ (cohesion * TestConfiguration.BehaviorCohesionWeight);

				entity.GetComponent<Acceleration>().Value = acceleration;
			}

			Parallel.For(0, entities.Count, parallelOptions, ProcessEntity);
		}
	}
}
