using BattleShipWPF.BattleShipClasses.AbstractClasses;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BattleShipWPF.BattleShipClasses
{
    public class BattleshipGame : Game
    {
        private readonly BattleshipPlayer _enemy;
        private readonly BattleshipReferee _referee;
        private readonly BattleshipPlayer _player;
        protected string _enemyAttack;
        protected int _Xattack;
        protected int _Yattack;
        private readonly ComboBox _shipCombobox;
        private readonly Label _statusLabel;

        public BattleshipGame(string PlayerName, ComboBox ShipCombobox, Label StatusLabel)
        {
            _enemy = new BattleshipPlayer("Enemy");
            _referee = new BattleshipReferee();
            _player = new BattleshipPlayer(PlayerName);
            _referee.AddPlayer(_player);
            _enemyAttack = "";
            _shipCombobox = ShipCombobox;
            _statusLabel = StatusLabel;
        }

        public BattleshipPlayer Player => _player;

        public BattleshipPlayer Enemy => _enemy;

        public BattleshipReferee Referee => _referee;

        public void SetXAttack(int xAttack)
        {
            _Xattack = xAttack;
        }

        public void SetYAttack(int yAttack)
        {
            _Yattack = yAttack;
        }

        public void CreatePlayersBoards(Grid PlayerGrid, Grid EnemyMockupGrid)
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    Player.Board.Buttons[i, j].Click += OnPlayerBoardClick;
                    CreatePlayerGrid(PlayerGrid, Player.Board.Buttons[i, j], i, j);
                    CreateEnemyGrid(EnemyMockupGrid, Enemy.Board.Buttons[i, j], i, j);
                }
            }
        }

        private void CreatePlayerGrid(Grid PlayerGrid, UIElement element, int row, int column)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            PlayerGrid.Children.Add(element);
        }

        private void CreateEnemyGrid(Grid EnemyMockupGrid, UIElement element, int row, int column)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            EnemyMockupGrid.Children.Add(element);
        }

        private void OnPlayerBoardClick(object sender, RoutedEventArgs e)
        {
            var jet = new Jet();
            var submarine = new Submarine();
            var tank = new Tank();
            var choosenShip = _shipCombobox.Text;
            if (choosenShip == submarine.GetStringType())
            {
                if (ShipBuilder.IsSpaceFree(submarine, _player, e))
                {
                    ShipBuilder.BuildShip(e, submarine, _player);
                    _player.ShipComboboxItems.Remove(choosenShip);
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => _shipCombobox.Items.Refresh()));
                }
            }

            if (choosenShip == jet.GetStringType())
            {
                if (ShipBuilder.IsSpaceFree(jet, _player, e))
                {
                    ShipBuilder.BuildShip(e, jet, _player);
                    _player.ShipComboboxItems.Remove(choosenShip);
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => _shipCombobox.Items.Refresh()));
                }
            }

            if (choosenShip == tank.GetStringType())
            {
                if (ShipBuilder.IsSpaceFree(tank, _player, e))
                {
                    ShipBuilder.BuildShip(e, tank, _player);
                    _player.ShipComboboxItems.Remove(choosenShip);
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => _shipCombobox.Items.Refresh()));
                }
            }
        }

        public string VerifyEnemyAttack(string ClientResponse)
        {
            var jet = new Jet();
            var submarine = new Submarine();
            var tank = new Tank();
            var x = (int)char.GetNumericValue(ClientResponse[0]);
            var y = (int)char.GetNumericValue(ClientResponse[2]);
            var attack = new Attack(x, y);
            var startindex = ClientResponse.IndexOf(".", StringComparison.Ordinal) + 1;
            var endindex = ClientResponse.Length - 1 - startindex + 1;
            var ship = ClientResponse.Substring(startindex, endindex);
            string response = "";

            if (ship == submarine.GetStringType())
            {
               response = attack.Shoot(_player, submarine);
            }

            else if (ship == tank.GetStringType())
            {
                response = attack.Shoot(_player, tank);
            }
            else if (ship == jet.GetStringType())
            {
                response = attack.Shoot(_player, jet);
            }

            return response;
        }

        public void FillEnemyShipList(string ClientResponse)
        {
            var jet = new Jet();
            var tank = new Tank();
            var submarine = new Submarine();
            var ship1 = (int)char.GetNumericValue(ClientResponse[1]);
            var ship2 = (int)char.GetNumericValue(ClientResponse[2]);
            var ship3 = (int)char.GetNumericValue(ClientResponse[3]);
            if (ship1 == (int)Ship.ShipType.Jet || ship2 == (int)Ship.ShipType.Jet ||
                ship3 == (int)Ship.ShipType.Jet)
                _enemy.Ships.Add(jet);
            if (ship1 == (int)Ship.ShipType.Tank || ship2 == (int)Ship.ShipType.Tank ||
                ship3 == (int)Ship.ShipType.Tank)
                _enemy.Ships.Add(tank);
            if (ship1 == (int)Ship.ShipType.Submarine || ship2 == (int)Ship.ShipType.Submarine ||
                ship3 == (int)Ship.ShipType.Submarine)
                _enemy.Ships.Add(submarine);
            Referee.AddPlayer(_enemy);
        }

        public void VerifyColor(string ClientResponse)
        {
            var response = ClientResponse[1];
            if (response == ',')
                return;
            {
                if (ClientResponse.Equals(Brushes.Cyan.ToString()))
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => _enemy.Board.Buttons[_Xattack, _Yattack].Background = Brushes.Cyan));

                else if (ClientResponse.Equals(Brushes.DarkRed.ToString()))
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => _enemy.Board.Buttons[_Xattack, _Yattack].Background = Brushes.Yellow));
                else if (ClientResponse.Equals(Brushes.White.ToString()))
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => _enemy.Board.Buttons[_Xattack, _Yattack].Background = Brushes.Red));
                else if (ClientResponse.Equals(Brushes.Orange.ToString()))
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => _enemy.Board.Buttons[_Xattack, _Yattack].Background = Brushes.Orange));
            }
        }

        public void RemoveEnemyShip(string ClientResponse)
        {
            var ship = (int)char.GetNumericValue(ClientResponse[1]);
            foreach (var ship1 in _enemy.Ships.ToList())
            {
                if (ship1.Type == ship)
                    _enemy.Ships.Remove(ship1);
            }
        }

        public override bool IsGameOver()
        {
            throw new NotImplementedException();
        }
    }
}
