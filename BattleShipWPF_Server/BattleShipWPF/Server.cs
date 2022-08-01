using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

#pragma warning disable 618

namespace BattleShipWPF
{
    public class Server : NetworkUser
    {

        private Socket _connection;
        private NetworkStream _socketStream;

        public Server()
        {
           new Thread(RunServer).Start();
        }

        private void RunServer()
        {
            try
            {
                var tcpListener = new TcpListener(5000);
                tcpListener.Start();
                while (true)
                {
                    _connection = tcpListener.AcceptSocket();
                    _socketStream = new NetworkStream(_connection);
                    _writer = new BinaryWriter(_socketStream);
                    _reader = new BinaryReader(_socketStream);
                    _writer.Write("Server>> Connection Successful");
                    _reply = "";
                    do
                    {
                        ReceiveMessage();
                    } while (_connection.Connected);
                    _writer.Close();
                    _reader.Close();
                    _socketStream.Close();
                    _connection.Close();
                }
            }

            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                Application.Current.Shutdown();
            }
        }

    }
}