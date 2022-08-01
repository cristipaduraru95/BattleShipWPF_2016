
namespace BattleShipWPF.BattleShipClasses
{
    public sealed class Jet : Ship
    {
        public Jet()
        {
            Type = (int) ShipType.Jet;
            Weakness[0] = (int) ShipAmmunition.CannonBall;
            Weakness[1] = (int) ShipAmmunition.Torpile;
            Ammunition = (int) ShipAmmunition.Rocket;
            ShipBody = new int[4, 3];
            Head = new Position();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    ShipBody[i, j] = (int) BattleshipBoard.BoardValues.Body;
                }
            }
            ShipBody[0, 0] = ShipBody[0, 2] = ShipBody[2, 0] = ShipBody[2, 2] = (int) BattleshipBoard.BoardValues.Empty;
            BodyPositions = new Position[GetBodySize()];
            for (var k = 0; k < GetBodySize(); k++)
            {
                BodyPositions[k] = new Position();
            }
        }

        public override string GetStringType() => nameof(ShipType.Jet);
    }
}