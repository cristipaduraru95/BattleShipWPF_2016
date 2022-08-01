
using System;
using System.Collections.Generic;

namespace BattleShipWPF.BattleShipClasses
{
    public abstract class Ship
    {
        public enum ShipType
        {
            Submarine,
            Tank,
            Jet
        }

        public enum ShipAmmunition
        {
            Rocket,
            Torpile,
            CannonBall
        }
        private readonly int[] _weakness = new int[2];
        private int _ammunition;
        private Position[] _bodyPositions;
        private Position _head;
        private int[,] _shipBody;
        private int _type;

        public int[] Weakness => _weakness;
        public int Ammunition { get => _ammunition; set => _ammunition = value; }
        public Position[] BodyPositions { get => _bodyPositions; set => _bodyPositions = value; }
        public Position Head { get => _head; set => _head = value; }
        public int[,] ShipBody { get => _shipBody; set => _shipBody = value; }
        public int Type { get => _type; set => _type = value; }
        public abstract string GetStringType();

        public int GetBodySize()
        {
            var count = 0;

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (ShipBody[i, j] == (int) BattleshipBoard.BoardValues.Body)
                    {
                        count++;
                    }
                }
            }
            return count;
        }


    }
}