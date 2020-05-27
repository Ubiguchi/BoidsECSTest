namespace BoidsECSTest
{
    internal static class Program
    {
        private static void Main()
        {
			if (TestConfiguration.IsDefaultEcs) 
			{
				new BoidsECSTest.DefaultEcs.DefaultGame().Run();
			}
			else
			{
				/// TODO: Fix FPS
				/// TODO: Improve use of spatial hashing
				/// TODO: Add multi-threading
				/// TODO: Check Boids algorithmica equality
				new BoidsECSTest.NezEcs.NezGame().Run();
			}
		}
	}
}