using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace BattleShipWPF
{
    public class Client : NetworkUser
    {
        private TcpClient _client;

        public Client()
        {
           new Thread(RunClient).Start();
        }

        public override void SendMessage(string value)
        {
            _writer.Write(value);
        }

        private void RunClient()
        {
            try
            {
                _client = new TcpClient();
                _client.Connect("localhost", 5000);
                _outputStrem = _client.GetStream();
                _writer = new BinaryWriter(_outputStrem);
                _reader = new BinaryReader(_outputStrem);

                do
                {
                    ReceiveMessage();
                } while (_reply != "Server>>terminate");

                _writer.Close();
                _reader.Close();
                _outputStrem.Close();
                _client.Close();

                Application.Current.Shutdown();
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                Application.Current.Shutdown();
            }
        }
    }
}