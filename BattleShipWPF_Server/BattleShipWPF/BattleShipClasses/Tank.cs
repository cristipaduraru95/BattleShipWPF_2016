
namespace BattleShipWPF.BattleShipClasses
{
    public class Tank : Ship
    {
        public Tank()
        {
            Type = (int) ShipType.Tank;
            Weakness[0] = (int) ShipAmmunition.Rocket;
            Weakness[1] = (int) ShipAmmunition.Torpile;
            Ammunition = (int) ShipAmmunition.CannonBall;
            ShipBody = new int[4, 3];
            Head = new Position();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    ShipBody[i, j] = (int) BattleshipBoard.BoardValues.Body;
                }
            }
            ShipBody[0, 0] = ShipBody[0, 2] = (int) BattleshipBoard.BoardValues.Empty;
            BodyPositions = new Position[GetBodySize()];
            for (var k = 0; k < GetBodySize(); k++)
            {
                BodyPositions[k] = new Position();
            }
        }

        public override string GetStringType() => nameof(ShipType.Tank);
    }
}