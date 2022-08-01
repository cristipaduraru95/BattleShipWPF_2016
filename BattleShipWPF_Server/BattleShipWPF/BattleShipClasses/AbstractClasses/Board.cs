namespace BattleShipWPF.BattleShipClasses.AbstractClasses
{
    public abstract class Board
    {
        private int[,] _field;
        private int _length;

        public int[,] Field { get => _field; set => _field = value; }
        public int Length { get => _length; set => _length = value; }
    }
}