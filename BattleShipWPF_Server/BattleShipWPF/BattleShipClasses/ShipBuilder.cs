using System.Windows;
using System.Windows.Media;

namespace BattleShipWPF.BattleShipClasses
{
    public class ShipBuilder
    {
        public static bool IsSpaceFree(Ship ship, BattleshipPlayer player, RoutedEventArgs e)
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (player.Board.Buttons[i, j].Equals(e.Source))
                    {
                        ship.Head.X = i;
                        ship.Head.Y = j;
                    }
                }
            }
            var count = 0;
            var xHead = ship.Head.X;
            var yHead = ship.Head.Y;
            if (yHead == 0)
            {
                yHead = 1;
            }
            var length = player.Board.Length - 1;

            if (xHead == length - 2)
            {
                xHead--;
            }

            if (xHead == length - 1)
            {
                xHead -= 2;
            }

            if (xHead == length)
            {
                xHead -= 3;
            }


            if (yHead == player.Board.Length - 1)
            {
                yHead = player.Board.Length - 2;
            }

            for (var i = xHead; i < 4 + xHead; i++)
            {
                for (var j = yHead; j < 3 + yHead; j++)
                {
                    if (player.Board.Field[i, j - 1] != ship.ShipBody[i - xHead, j - yHead]
                        && player.Board.Field[i, j - 1] == (int) BattleshipBoard.BoardValues.Empty)

                        count++;
                }
            }
            return ship.GetBodySize() == count;
        }

        public static void BuildShip(RoutedEventArgs e, Ship ship, BattleshipPlayer player)
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (player.Board.Buttons[i, j].Equals(e.Source))
                    {
                        ship.Head.X = i;
                        ship.Head.Y = j;
                    }
                }
            }
            if (IsSpaceFree(ship, player, e))
            {
                FillBodyPosition(ship, player);
                player.Board.Field[ship.Head.X, ship.Head.Y] = (int) BattleshipBoard.BoardValues.Head;
                for (var i = 0; i < 10; i++)
                    for (var j = 0; j < 10; j++)
                    {
                        if (player.Board.Buttons[i, j].Equals(e.Source))
                        {
                            for (var j2 = 0; j2 < ship.GetBodySize(); j2++)
                                player.Board.Buttons[ship.BodyPositions[j2].X, ship.BodyPositions[j2].Y].Background
                                    = Brushes.Yellow;
                            player.Board.Buttons[ship.Head.X, ship.Head.Y].Background =
                                Brushes.Red;
                        }
                    }
                player.Ships.Add(ship);
            }
        }

        private static void FillBodyPosition(Ship ship, BattleshipPlayer player)
        {
            var xHead = ship.Head.X;
            var yHead = ship.Head.Y;
            var k = 0;

            if (yHead == 0)
            {
                yHead = 1;
            }
            var length = player.Board.Length - 1;

            if (xHead == length - 2)
            {
                xHead--;
            }

            if (xHead == length - 1)
            {
                xHead -= 2;
            }

            if (xHead == length)
            {
                xHead -= 3;
            }


            if (yHead == player.Board.Length - 1)
            {
                yHead = player.Board.Length - 2;
            }

            ship.Head.X = xHead;
            ship.Head.Y = yHead;

            for (var i = xHead; i < 4 + xHead; i++)
            {
                for (var j = yHead; j < 3 + yHead; j++)
                {
                    if (ship.ShipBody[i - xHead, j - yHead] != (int) BattleshipBoard.BoardValues.Empty)
                    {
                        player.Board.Field[i, j - 1] = ship.ShipBody[i - xHead, j - yHead];
                        ship.BodyPositions[k].X = i;
                        ship.BodyPositions[k].Y = j - 1;
                        k++;
                    }
                }
            }
            player.Board.Field[xHead, yHead] = (int) BattleshipBoard.BoardValues.Head;
        }
    }
}