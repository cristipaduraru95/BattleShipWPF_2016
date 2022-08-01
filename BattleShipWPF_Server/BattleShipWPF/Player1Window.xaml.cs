using BattleShipWPF.BattleShipClasses;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace BattleShipWPF
{
    public partial class Player1Window
    {
        private readonly Server _server;
        private readonly BattleshipGame _battleshipGame;

        public Player1Window()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            _battleshipGame = new BattleshipGame("Server", ShipCombobox, StatusLabel);
            ShipCombobox.ItemsSource = _battleshipGame.Player.ShipComboboxItems;
            _battleshipGame.Player.ShipComboboxItems.CollectionChanged += OnShipComboboxItemsChange;
            StatusLabel.Visibility = Visibility.Hidden;
            StatusLabel.Content = "Choose a ship to attack !";
            StartBattleButton.Visibility = Visibility.Hidden;
            _server = new Server();
            _server.MessageReceived += OnMessageReceived;
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
                _server.SendMessage(":" + (int)Ship.ShipType.Jet + (int)Ship.ShipType.Submarine +
                                   (int)Ship.ShipType.Tank);
            }
        }

        public void OnMessageReceived(object sender, EventArgs e)
        {
            if (_server.GetResponse().Equals(""))
            {
                return;
            }
            if (_server.GetResponse()[0].Equals(':'))
                _battleshipGame.FillEnemyShipList(_server.GetResponse());
            if (_server.GetResponse()[_server.GetResponse().Length - 1].Equals('!'))
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() => StatusLabel.Content = "Server has won!"));
                return;
            }
            if (_server.GetResponse()[1].Equals(','))
            {
                string response = _battleshipGame.VerifyEnemyAttack(_server.GetResponse());
                _server.SendMessage(response);
                if (_battleshipGame.Referee.RunAllRules(_battleshipGame.Player))
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (Action)(() => StatusLabel.Content = "Client has won!"));
                    _server.SendMessage("Client has won!");
                    return;
                }
                Brush color;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        int x = (int)char.GetNumericValue(_server.GetResponse()[0]);
                        int y = (int)char.GetNumericValue(_server.GetResponse()[2]);
                        color = _battleshipGame.Player.Board.Buttons[x, y].Background;
                        _server.SendMessage(color.ToString());
                    }));

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() => ShipCombobox.Items.Refresh()));
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (Action)(() => StatusLabel.Content = "Server turn"));
            }

            if (_server.GetResponse()[0].Equals('-'))
                _battleshipGame.RemoveEnemyShip(_server.GetResponse());
            if (_server.GetResponse()[0].Equals('#'))
            {
                _battleshipGame.VerifyColor(_server.GetResponse());
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)(() => StatusLabel.Content = "Client turn"));
            }

        }

        public void AttackEnemy(object sender, RoutedEventArgs e)
        {
            if (StatusLabel.Content.Equals("Server turn"))
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
                            _server.SendMessage(attack);
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                  (Action)(() => StatusLabel.Content = "Client turn"));
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