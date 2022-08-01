using System;
using System.IO;
using System.Net.Sockets;

namespace BattleShipWPF
{
    public abstract class NetworkUser
    {
        public delegate void MessageReceiveEventHandler(object sender, EventArgs e);
        public event MessageReceiveEventHandler MessageReceived;
        protected NetworkStream _outputStrem;
        protected BinaryReader _reader;
        protected BinaryWriter _writer;
        protected string _reply;

        protected virtual void OnMessageReceived(EventArgs e) => MessageReceived?.Invoke(this, EventArgs.Empty);

        protected void ReceiveMessage()
        {
            _reply = _reader.ReadString();
            OnMessageReceived(EventArgs.Empty);
        }

        public virtual void SendMessage(string value)
        {
                _writer.Write(value);
        }

        public string GetResponse() => _reply;
    }
}
