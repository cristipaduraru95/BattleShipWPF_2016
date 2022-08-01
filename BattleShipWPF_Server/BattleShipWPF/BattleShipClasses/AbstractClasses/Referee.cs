using System.Collections.Generic;

namespace BattleShipWPF.BattleShipClasses.AbstractClasses
{
    public abstract class Referee
    {
        protected List<IRule> Rules;

        public abstract void PrintWinner(Player player);

    }
}