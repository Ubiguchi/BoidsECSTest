using Microsoft.Xna.Framework.Graphics;

namespace BoidsECSTest
{
    internal static class TestConfiguration
    {
		public const bool IsDefaultEcs = false;
		public const int BoidsCount = 300; // 30000;

		public const int ResolutionWidth = 1920;
		public const int ResolutionHeight = 1080;

		public const float BehaviorSeparationWeight = 20;
		public const float BehaviorAlignmentWeight = 4;
		public const float BehaviorCohesionWeight = 1;
		public const bool IsFullScreen = false;

		// This should really be HiDef
		public static GraphicsProfile Profile = GraphicsProfile.Reach;
		public const float NeighborRange = 150;

		public const float MinVelocity = 290;
		public const float MaxVelocity = 300;
	}
}