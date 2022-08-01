using BattleShipWPF.BattleShipClasses.AbstractClasses;

namespace BattleShipWPF.BattleShipClasses
{
    public class AreAllShipsDestroyed : IRule
    {
        public bool Run(AbstractClasses.Player player) => ArePlayerShipsDestroyed((BattleshipPlayer)player);

        private bool ArePlayerShipsDestroyed(BattleshipPlayer player) => player.Ships.Count == 0;
    }
}