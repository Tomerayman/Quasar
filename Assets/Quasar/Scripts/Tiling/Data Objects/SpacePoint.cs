namespace Quasar.Tiling
{
    public class SpacePoint
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public float Gravity { get; set; }

        public SpacePoint(int row, int col, float gravity)
        {
            Row = row;
            Col = col;
            Gravity = gravity;
        }

        public void TakeGravity(float gravity)
        {
            Gravity += gravity;
        }


    }
}