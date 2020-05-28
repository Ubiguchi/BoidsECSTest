namespace BoidsECSTest.NezEcs
{
	using Nez;
	using Microsoft.Xna.Framework.Graphics;
    using System;

    public class NezGame : Core
    {
        public NezGame()
			: base(TestConfiguration.ResolutionWidth, TestConfiguration.ResolutionHeight, TestConfiguration.IsFullScreen )
        {
            IsFixedTimeStep = false;
			Screen.SynchronizeWithVerticalRetrace = false;
		}

		protected override void Initialize()
		{
			base.Initialize();
			SetupScreen();

			Content.RootDirectory = "Content";
			Physics.SpatialHashCellSize = Math.Min(TestConfiguration.CellWidth, TestConfiguration.CellHeight);

			Scene = new BoidsScene(this);
		}

		private void SetupScreen()
		{
			Scene.SetDefaultDesignResolution(TestConfiguration.ResolutionWidth, TestConfiguration.ResolutionHeight, Scene.SceneResolutionPolicy.BestFit);
			Screen.SetSize(TestConfiguration.ResolutionWidth, TestConfiguration.ResolutionHeight);
			Window.AllowUserResizing = true;
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			GraphicsDevice.BlendState = BlendState.AlphaBlend;

			Insist.IsTrue(Core.GraphicsDevice.GraphicsProfile == TestConfiguration.Profile);
		}
    }
}
