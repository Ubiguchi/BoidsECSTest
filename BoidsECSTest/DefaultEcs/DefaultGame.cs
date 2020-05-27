using System;
using System.Diagnostics;
using System.IO;
using BoidsECSTest.DefaultEcs.Components;
using BoidsECSTest.DefaultEcs.Systems;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BoidsECSTest.DefaultEcs
{
    public class DefaultGame : Game
    {
        #region Fields

        public const int BoidsCount = TestConfiguration.BoidsCount;

        public const int ResolutionWidth = TestConfiguration.ResolutionWidth;
        public const int ResolutionHeight = TestConfiguration.ResolutionHeight;

        public const float BehaviorSeparationWeight = TestConfiguration.BehaviorSeparationWeight;
        public const float BehaviorAlignmentWeight = TestConfiguration.BehaviorAlignmentWeight;
        public const float BehaviorCohesionWeight = TestConfiguration.BehaviorCohesionWeight;

        public const float NeighborRange = TestConfiguration.NeighborRange;

        public const float MinVelocity = TestConfiguration.MinVelocity;
        public const float MaxVelocity = TestConfiguration.MaxVelocity;

        private readonly GraphicsDeviceManager _deviceManager;
        private readonly SpriteBatch _batch;
        private readonly Texture2D _square;
        private readonly SpriteFont _font;
        private readonly World _world;
        private readonly DefaultParallelRunner _runner;
        private readonly ISystem<float> _system;
        private readonly ISystem<float> _drawSystem;
        private readonly Stopwatch _watch;

        private int _frameCount;
        private string _fps = string.Empty;

        #endregion

        #region Initialisation

        public DefaultGame()
        {
            _deviceManager = new GraphicsDeviceManager(this);
            IsFixedTimeStep = false;
            _deviceManager.GraphicsProfile = TestConfiguration.Profile;
            _deviceManager.IsFullScreen = TestConfiguration.IsFullScreen;
            _deviceManager.PreferredBackBufferWidth = ResolutionWidth;
            _deviceManager.PreferredBackBufferHeight = ResolutionHeight;
            _deviceManager.SynchronizeWithVerticalRetrace = false;
            _deviceManager.ApplyChanges();
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Content.RootDirectory = "Content";

            _batch = new SpriteBatch(GraphicsDevice);
            using (Stream stream = File.OpenRead(@"Content\square.png"))
            {
                _square = Texture2D.FromStream(GraphicsDevice, stream);
            }
            _font = Content.Load<SpriteFont>("font");

            _world = new World();

            _runner = new DefaultParallelRunner(Environment.ProcessorCount);
            _system = new SequentialSystem<float>(
                new BehaviorSystem(_world, _runner),
                new BoidsSystem(_world, _runner),
                new MoveSystem(_world, _runner));

            _drawSystem = new DrawSystem(_batch, _square, _world, _runner);

            _world.CreateBehaviors();

            Random random = new Random();

            for (int i = 0; i < BoidsCount; ++i)
            {
                Entity entity = _world.CreateEntity();
                entity.Set(new DrawInfo
                {
                    Color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1f),
                    Position = new Vector2((float)random.NextDouble() * _deviceManager.PreferredBackBufferWidth, (float)random.NextDouble() * _deviceManager.PreferredBackBufferHeight),
                    Size = new Vector2(random.Next(10, 15), random.Next(20, 30)),
                });

                Vector2 velocity = new Vector2((float)random.NextDouble() - .5f, (float)random.NextDouble() - .5f);
                if (velocity != Vector2.Zero)
                {
                    velocity.Normalize();
                }

                entity.Set(new Velocity { Value = velocity * (MinVelocity + ((float)random.NextDouble() * (MaxVelocity - MinVelocity))) });
                entity.Set<Acceleration>();
                entity.Set(entity.Get<DrawInfo>().Position.ToGridId());
            }

            _world.Optimize();

            _watch = Stopwatch.StartNew();
        }

        #endregion

        #region Game

        protected override void Update(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _system.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            _drawSystem.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            ++_frameCount;
            if (_watch.Elapsed.TotalSeconds > .5)
            {
                _fps = (_frameCount / _watch.Elapsed.TotalSeconds).ToString();
                _frameCount = 0;
                _watch.Restart();
            }

            _batch.Begin();
            _batch.DrawString(_font, _fps, new Vector2(10, 10), Color.White);
            _batch.End();
        }

        protected override void Dispose(bool disposing)
        {
            _runner.Dispose();
            _world.Dispose();
            _system.Dispose();
            _square.Dispose();
            _batch.Dispose();
            _deviceManager.Dispose();

            base.Dispose(disposing);
        }

        #endregion
    }
}
