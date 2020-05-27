namespace BoidsECSTest.NezEcs.Systems
{
	using System;
	using BoidsECSTest.NezEcs.Components;
	using Nez;
	using Microsoft.Xna.Framework;
	using System.Collections.Generic;

	public sealed class MoveSystem : EntitySystem
    {
        public MoveSystem()
			: base(new Matcher().All(typeof(DrawInfo), typeof(Velocity), typeof(Acceleration)))
		{
		}

		protected override void Process(List<Entity> entities)
		{
			float state = Time.DeltaTime;
			
			foreach (Entity entity in entities)
			{
				DrawInfo drawInfo = entity.GetComponent<DrawInfo>();

				Vector2 acceleration = entity.GetComponent<Acceleration>().Value;
				Vector2 velocity = entity.GetComponent<Velocity>().Value;

				velocity += acceleration * state;

				Vector2 direction = Vector2.Normalize(velocity);
				float speed = velocity.Length();

				velocity = Math.Clamp(speed, TestConfiguration.MinVelocity, TestConfiguration.MaxVelocity) * direction;

				Vector2 newPosition = drawInfo.Position + (velocity * state);

				drawInfo.Position = newPosition;
				entity.Position = newPosition;

				if ((drawInfo.Position.X < 0 && velocity.X < 0)
					|| (drawInfo.Position.X > TestConfiguration.ResolutionWidth && velocity.X > 0))
				{
					velocity.X *= -1;
				}
				if ((drawInfo.Position.Y < 0 && velocity.Y < 0)
					|| (drawInfo.Position.Y > TestConfiguration.ResolutionHeight && velocity.Y > 0))
				{
					velocity.Y *= -1;
				}

				entity.GetComponent<Velocity>().Value = velocity;

				drawInfo.Rotation = MathF.Atan2(velocity.X, -velocity.Y);
			}
		}
	}
}
