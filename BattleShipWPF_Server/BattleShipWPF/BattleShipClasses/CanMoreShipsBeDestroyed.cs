using BattleShipWPF.BattleShipClasses.AbstractClasses;

namespace BattleShipWPF.BattleShipClasses
{
    public class CanMoreShipsBeDestroyed : IRule
    {
        private readonly BattleshipReferee _referee;

        public CanMoreShipsBeDestroyed(BattleshipReferee referee)
        {
            _referee = referee;
        }

        public bool Run(AbstractClasses.Player player)
        {
            var player1 = _referee.Players[0];
            var player2 = _referee.Players[1];

            if (CheckIfOnlyOnePlayerHasThreeShips(player1, player2))
                return true;
            if (CheckIfOnlyOnePlayerHasThreeShips(player2, player1))
                return true;
            return CheckIfOneShipRemains(player1, player2) || CheckIfOneShipRemains(player2, player1);
        }

        private static bool CheckIfOneShipRemains(BattleshipPlayer player1, BattleshipPlayer player2)
            => player1.Ships.Count == 1 && player2.Ships.Count == 2 &&
               (player1.Ships[0].Ammunition != player2.Ships[0].Weakness[0] &&
                player1.Ships[0].Ammunition != player2.Ships[0].Weakness[1] ||
                player1.Ships[0].Ammunition != player2.Ships[1].Weakness[0] &&
                player1.Ships[0].Ammunition != player2.Ships[1].Weakness[1]);

        private static bool CheckIfOnlyOnePlayerHasThreeShips(BattleshipPlayer player1, BattleshipPlayer player2)
            => player1.Ships.Count == 1 && player2.Ships.Count > 2;
    }
}