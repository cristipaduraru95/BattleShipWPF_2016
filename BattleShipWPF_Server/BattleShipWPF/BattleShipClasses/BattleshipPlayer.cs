using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleShipWPF.BattleShipClasses
{
    public class BattleshipPlayer : AbstractClasses.Player
    {
        private readonly BattleshipBoard _board;
        private List<Ship> _ships;
        public ObservableCollection<string> ShipComboboxItems;

        public BattleshipPlayer(string name)
        {
            _board = new BattleshipBoard(10);
            Name = name;
            _ships = new List<Ship>();
            ShipComboboxItems = GetShipTypes();
        }

        public List<Ship> Ships { get => _ships; set => _ships = value; }

        public BattleshipBoard Board => _board;

        public bool AreAllShipsOnBoard() => Ships.Count == 3;

        public Ship GetShip(Position position)
        {
            foreach (var ship in Ships)
            {
                for (var i = 0; i < ship.GetBodySize(); i++)
                {
                    if ((position.X == ship.Head.X && position.Y == ship.Head.Y) ||
                        (position.X == ship.BodyPositions[i].X) && (position.Y == ship.BodyPositions[i].Y))
                        return ship;
                }
            }

            return null;
        }

        public ObservableCollection<string> GetShipTypes()
        {
            ObservableCollection<string> shipTypes = new ObservableCollection<string>();
            foreach (var shipType in System.Enum.GetValues(typeof(Ship.ShipType))) { shipTypes.Add(shipType.ToString()); }
            return shipTypes;
        }

    }
}