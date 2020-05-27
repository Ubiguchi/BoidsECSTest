namespace BoidsECSTest.NezEcs.Systems
{
	using BoidsECSTest.NezEcs.Components;
	using Nez;
	using Microsoft.Xna.Framework;
	using System.Collections.Generic;

	public class BehaviorSystem : EntitySystem
	{
		public BehaviorSystem()
			: base(new Matcher().All(typeof(Behavior)))
		{
		}

		protected override void Process(List<Entity> entities)
		{
			foreach (Entity entity in entities)
			{
				var neighbors = GetNeighbours(entity);

				Vector2 center = Vector2.Zero;
				Vector2 direction = Vector2.Zero;
				int count = 0;

				foreach (Collider neighbor in neighbors)
				{
					center += neighbor.GetComponent<DrawInfo>().Position;
					direction += neighbor.GetComponent<Velocity>().Value;
					count++;
				}

				entity.RemoveComponent<Behavior>();
				entity.AddComponent(new Behavior(center, direction, count));
			}
		}

		private IEnumerable<Collider> GetNeighbours(Entity entity)
		{
			const int _width = (int)(TestConfiguration.ResolutionWidth / TestConfiguration.NeighborRange * 3);
			const int distance = _width;
			const int doubleDistance = 2 * distance;

			var collider = entity.GetComponent<BoxCollider>();
			var neighbouringRectangle = new RectangleF(collider.Bounds.Left - distance, collider.Bounds.Top + distance, collider.Bounds.Width + doubleDistance, collider.Bounds.Height + doubleDistance);

			return Physics.BoxcastBroadphase(neighbouringRectangle);
		}
	}
}