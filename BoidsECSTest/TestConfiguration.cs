namespace BoidsECSTest
{
	using System;
	using Microsoft.Xna.Framework.Graphics;

	internal static class TestConfiguration
    {
		public const float BehaviorSeparationWeight = 20;
		public const float BehaviorAlignmentWeight = 4;
		public const float BehaviorCohesionWeight = 1;

		public const int BoidsCount = 300; // 30000;

		public const int CellColumnCount = (int) (ResolutionWidth / (NeighborRange / 2));
		public const int CellRowCount = (int) (ResolutionHeight / (NeighborRange / 2));
		public const int CellWidth = ResolutionWidth / CellColumnCount;
		public const int CellHeight = ResolutionHeight / CellRowCount;

		public static int DegreeOfParallelism = 1;// Environment.ProcessorCount;

		// This should really be HiDef
		public static GraphicsProfile Profile = GraphicsProfile.Reach;

		public const bool IsDefaultEcs = true;
		public const bool IsFullScreen = false;

		public const float MinVelocity = 290;
		public const float MaxVelocity = 300;

		public const float NeighborRange = 150;

		public const int ResolutionWidth = 1920;
		public const int ResolutionHeight = 1080;
	}
}