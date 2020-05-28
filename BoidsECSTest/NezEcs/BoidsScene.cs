namespace BoidsECSTest.NezEcs
{
    using System;
    using System.IO;
	using BoidsECSTest.NezEcs.Components;
	using BoidsECSTest.NezEcs.Systems;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Nez;

	public class BoidsScene : Scene
	{
		public BoidsScene(NezGame game)
		{
			ClearColor = Color.Black;
		}

		public override void Initialize()
		{
			base.Initialize();

			AddRenderer(new DefaultRenderer());
			CreateEntities();
			CreateSystems();
		}

		private void CreateEntities()
		{
			//var spriteFont = Content.Load<SpriteFont>("font");
			//var nezFont = new NezSpriteFont(spriteFont);
			//var fpsCounter = CreateEntity($"fpsCounter");
			//fpsCounter.AddComponent(new FramesPerSecondCounter(nezFont, Color.White));

			Core.DebugRenderEnabled = false;

			Texture2D square;
			using (Stream stream = File.OpenRead(@"Content\square.png"))
			{
				square = Texture2D.FromStream(Core.GraphicsDevice, stream);
			}

			var random = new System.Random();
			for (int i = 0; i < TestConfiguration.BoidsCount; ++i)
			{
				var position = new Vector2((float)random.NextDouble() * TestConfiguration.ResolutionWidth, (float)random.NextDouble() * TestConfiguration.ResolutionHeight);
				Entity entity = CreateEntity($"boid_{i}", position);

				entity.AddComponent(new DrawSystemRenderer(square));

				var drawInfo = new DrawInfo
				{
					Color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f),
					Position = position,
					Size = new Vector2(random.Next(10, 15), random.Next(20, 30)),
				};

				entity.AddComponent(drawInfo);

				Vector2 velocity = new Vector2((float)random.NextDouble() - .5f, (float)random.NextDouble() - .5f);
				if (velocity != Vector2.Zero)
				{
					velocity.Normalize();
				}

				entity.AddComponent(new Velocity { Value = velocity * (TestConfiguration.MinVelocity + ((float)random.NextDouble() * (TestConfiguration.MaxVelocity - TestConfiguration.MinVelocity))) });
				entity.AddComponent<Acceleration>();
				entity.AddComponent(new BoxCollider());
				entity.AddComponent(new CellId(0, 0));
			}
		}

		private void CreateSystems()
		{
			var behaviorCollationSystem = new BehaviorSystem(TestConfiguration.CellColumnCount, TestConfiguration.CellRowCount, Physics.SpatialHashCellSize);
			AddEntityProcessor(behaviorCollationSystem);
			//AddEntityProcessor(new BehaviorSystem());
			AddEntityProcessor(new BoidsSystem(behaviorCollationSystem));
			AddEntityProcessor(new MoveSystem());
		}
	}
}