namespace BattleShipWPF.BattleShipClasses
{
    public class Position
    {
        private int _x;
        private int _y;

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }

        public Position()
        {
            X = 0;
            Y = 0;
        }
    }
}