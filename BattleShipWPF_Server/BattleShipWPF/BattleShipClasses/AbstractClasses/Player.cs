using System.Collections.Generic;

namespace BattleShipWPF.BattleShipClasses.AbstractClasses
{
    public abstract class Player
    {
        private string _name;

        protected string Name { get => _name; set => _name = value; }

    }
}