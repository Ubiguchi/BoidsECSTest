using Microsoft.Xna.Framework;
using Nez;

namespace BoidsECSTest.NezEcs.Components
{
	public class Behavior : Component
	{
        public readonly Vector2 Center;
        public readonly Vector2 Direction;
        public readonly int Count;

		public Behavior()
			: this(Vector2.Zero, Vector2.Zero, 0)
		{
		}

		public Behavior(Vector2 center, Vector2 direction, int count)
        {
            Center = center;
            Direction = direction;
            Count = count;
        }
    }
}
