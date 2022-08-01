using System.Collections.Generic;
using System.Linq;
using BattleShipWPF.BattleShipClasses.AbstractClasses;

namespace BattleShipWPF.BattleShipClasses
{
    public class BattleshipReferee : Referee
    {
        private readonly List<BattleshipPlayer> _players = new List<BattleshipPlayer>();

        public List<BattleshipPlayer> Players => _players;

        public BattleshipReferee()
        {
            Rules = new List<IRule> {new AreAllShipsDestroyed(), new CanMoreShipsBeDestroyed(this)};
        }

        public void AddPlayer(BattleshipPlayer player)
        {
            Players.Add(player);
        }

        public override void PrintWinner(AbstractClasses.Player player)
        {
            //Console.WriteLine(player.GetName() + " has won the game!");
        }

        public bool RunAllRules(BattleshipPlayer player) => Rules.Any(rule => rule.Run(player));

        public bool IsDraw() => Players[0].Ships.Count == 1 && Players[1].Ships.Count == 1 &&
                                   Players[0].Ships[0].GetType() == Players[1].Ships[0].GetType();
    }
}