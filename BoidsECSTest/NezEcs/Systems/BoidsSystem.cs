namespace BoidsECSTest.NezEcs.Systems
{
	using BoidsECSTest.NezEcs.Components;
	using Nez;
	using Microsoft.Xna.Framework;
	using System.Collections.Generic;

	public sealed class BoidsSystem : EntitySystem
	{
		public BoidsSystem()
			: base(new Matcher().All(typeof(DrawInfo), typeof(Acceleration), typeof(Velocity)))
		{
		}

		protected override void Process(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				Vector2 position = entity.GetComponent<DrawInfo>().Position;
				Vector2 velocity = entity.GetComponent<Velocity>().Value;

				Vector2 separation = Vector2.Zero;
				Vector2 alignment = Vector2.Zero;
				Vector2 cohesion = Vector2.Zero;
				int neighborCount = 0;

				var behavior = entity.GetComponent<Behavior>();
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
		}
	}
}
