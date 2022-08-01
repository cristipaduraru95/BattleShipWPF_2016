using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using BattleShipWPF.BattleShipClasses;

namespace BattleShipWPF
{
    public partial class Player2Window
    {
        private readonly Client _client;
        private readonly BattleshipGame _battleshipGame;

        public Player2Window()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            _battleshipGame = new BattleshipGame("Client", ShipCombobox, StatusLabel);
            ShipCombobox.ItemsSource = _battleshipGame.Player.ShipComboboxItems;
            _battleshipGame.Player.ShipComboboxItems.CollectionChanged += OnShipComboboxItemsChange;
            StatusLabel.Visibility = Visibility.Hidden;
            StatusLabel.Content = "Choose a ship to attack !";
            StartBattleButton.Visibility = Visibility.Hidden;
            _client = new Client();
            _client.MessageReceived += OnMessageReceived;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _battleshipGame.CreatePlayersBoards(PlayerGrid, EnemyMockupGrid);
        }

        private void OnShipComboboxItemsChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_battleshipGame.Player.AreAllShipsOnBoard())
            {
                StartBattleButton.Visibility = Visibility.Visible;
                _client.SendMessage(":" + (int)Ship.ShipType.Jet + (int)Ship.ShipType.Submarine +
                                   (int)Ship.ShipType.Tank);
            }
        }

        public void OnMessageReceived(object sender, EventArgs e)
        {
            if (_client.GetResponse().Equals(""))
            {
                return;
            }
            if (_client.GetResponse()[0].Equals(':'))
                _battleshipGame.FillEnemyShipList(_client.GetResponse());
            if (_client.GetResponse()[_client.GetResponse().Length - 1].Equals('!'))
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() => StatusLabel.Content = "Client has won!"));
                return;
            }
            if (_client.GetResponse()[1].Equals(','))
            {
                string response = _battleshipGame.VerifyEnemyAttack(_client.GetResponse());
                _client.SendMessage(response);
                if (_battleshipGame.Referee.RunAllRules(_battleshipGame.Player))
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => StatusLabel.Content = "Server has won!"));
                    _client.SendMessage("Server has won!");
                    return;
                }
                Brush color;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        int x = (int)char.GetNumericValue(_client.GetResponse()[0]);
                        int y = (int)char.GetNumericValue(_client.GetResponse()[2]);
                        color = _battleshipGame.Player.Board.Buttons[x, y].Background;
                        _client.SendMessage(color.ToString());
                    }));
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() => ShipCombobox.Items.Refresh()));
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() => StatusLabel.Content = "Client turn"));
            }

            if (_client.GetResponse()[0].Equals('-'))
                _battleshipGame.RemoveEnemyShip(_client.GetResponse());
            if (_client.GetResponse()[0].Equals('#'))
            {
                _battleshipGame.VerifyColor(_client.GetResponse());
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() => StatusLabel.Content = "Server turn"));
            }

        }

        public void AttackEnemy(object sender, RoutedEventArgs e)
        {
            if (StatusLabel.Content.Equals("Client turn"))
            {
                for (var i = 0; i < 10; i++)
                {
                    for (var j = 0; j < 10; j++)
                    {
                        if (_battleshipGame.Enemy.Board.Buttons[i, j].Equals(e.Source))
                        {
                            _battleshipGame.SetXAttack(i);
                            _battleshipGame.SetYAttack(j);
                            string attack = i + "," + j + "." + ShipCombobox.Text;
                            _client.SendMessage(attack);
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                 (Action)(() => StatusLabel.Content = "Server turn"));
                            return;
                        }
                    }
                }
            }
        }

        private void OnStartBattleClick(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    _battleshipGame.Enemy.Board.Buttons[i, j].Click += AttackEnemy;
                }
            }
            StatusLabel.Visibility = Visibility.Visible;
            ShipCombobox.ItemsSource = _battleshipGame.Player.GetShipTypes();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)(() => ShipCombobox.Items.Refresh()));
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)(() => StatusLabel.Content = "Server turn"));
            StartBattleButton.Visibility = Visibility.Hidden;
        }


    }
}