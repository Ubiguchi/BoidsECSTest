namespace BoidsECSTest.NezEcs.Components
{
    using Nez;

    public class CellId : Component
    {
        public int X;
        public int Y;

        public CellId(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}