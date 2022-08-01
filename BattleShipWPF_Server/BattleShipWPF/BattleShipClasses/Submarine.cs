
namespace BattleShipWPF.BattleShipClasses
{
    public class Submarine : Ship
    {
        public Submarine()
        {
            Type = (int) ShipType.Submarine;
            Weakness[0] = (int) ShipAmmunition.Rocket;
            Weakness[1] = (int) ShipAmmunition.CannonBall;
            Ammunition = (int) ShipAmmunition.Torpile;
            ShipBody = new int[4, 3];
            Head = new Position();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    ShipBody[i, j] = (int) BattleshipBoard.BoardValues.Body;
                }
            }
            BodyPositions = new Position[GetBodySize()];
            for (var k = 0; k < GetBodySize(); k++)
            {
                BodyPositions[k] = new Position();
            }
        }

        public override string GetStringType() => nameof(ShipType.Submarine);
    }
}