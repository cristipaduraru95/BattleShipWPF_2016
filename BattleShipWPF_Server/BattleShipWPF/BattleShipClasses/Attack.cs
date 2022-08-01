using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace BattleShipWPF.BattleShipClasses
{
    public class Attack
    {
        private readonly Position _position;

        public Attack(int x, int y)
        {
            _position = new Position
            {
                X = x,
                Y = y
            };
        }

        public Attack(Position position)
        {
            _position = position;
        }

        public string Shoot(BattleshipPlayer player, Ship ship)
        {
            StringBuilder stringBuilder = new StringBuilder();
            switch (player.Board.Field[_position.X, _position.Y])
            {
                case (int) BattleshipBoard.BoardValues.Empty:
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action) (() => player.Board.Buttons[_position.X, _position.Y].Background = Brushes.Cyan));

                    break;
                case (int) BattleshipBoard.BoardValues.DamagedBody:

                    break;
                case (int) BattleshipBoard.BoardValues.Body:
                    if (player.GetShip(_position).Weakness[0] == ship.Ammunition
                        || player.GetShip(_position).Weakness[1] == ship.Ammunition)
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            (Action)
                                (() => player.Board.Buttons[_position.X, _position.Y].Background = Brushes.DarkRed));

                        player.Board.Field[_position.X, _position.Y] = (int) BattleshipBoard.BoardValues.DamagedBody;
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            (Action)
                                (() => player.Board.Buttons[_position.X, _position.Y].Background = Brushes.Orange));
                    }
                    break;
                case (int) BattleshipBoard.BoardValues.Head:
                    if (player.GetShip(_position).Weakness[0] == ship.Ammunition
                        || player.GetShip(_position).Weakness[1] == ship.Ammunition)
                    {
                        foreach (var playerShip in player.Ships.ToList())
                        {
                            if (playerShip.Head.X == _position.X && playerShip.Head.Y == _position.Y)
                            {
                                for (var i = 0; i < playerShip.GetBodySize(); i++)
                                {
                                    var x = playerShip.BodyPositions[i].X;
                                    var y = playerShip.BodyPositions[i].Y;
                                    player.Board.Field[x, y] = (int) BattleshipBoard.BoardValues.Empty;
                                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                        (Action) (() => player.Board.Buttons[x, y].Background = Brushes.White));
                                }
                                player.Board.Field[playerShip.Head.X, playerShip.Head.Y] =
                                    (int) BattleshipBoard.BoardValues.Empty;
                                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                    (Action)
                                        (() =>
                                            player.Board.Buttons[playerShip.Head.X, playerShip.Head.Y]
                                                .Background =
                                                Brushes.White));

                                //server.SendMessage("-" + playerShip.GetType());
                                player.Ships.Remove(playerShip);
                                player.ShipComboboxItems.Remove(playerShip.GetType().Name);
                                stringBuilder.Append("-");
                                stringBuilder.Append(playerShip.GetType().Name);
                                return stringBuilder.ToString();
                            }
                        }
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            (Action)
                                (() => player.Board.Buttons[_position.X, _position.Y].Background = Brushes.Orange));
                    }
                    break;
            }
            return stringBuilder.ToString();
        }
    }
}