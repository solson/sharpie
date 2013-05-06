using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Sharpie {
    public partial class Irc {
        private TcpClient tcp;
        private StreamReader reader;
        private StreamWriter writer;

        public delegate void CommandHandler(IrcMessage msg);
        private Dictionary<string, CommandHandler> handlers;

        public string Server { get; private set; }
        public int Port { get; private set; }
        public bool Connected { get; private set; }

        public string Nickname { get; private set; }
        public string Username { get; private set; }
        public string Realname { get; private set; }
        public string[] Channels { get; private set; }

        public Irc(string server, int port, string nickname, string username,
                string realname, string[] channels) {
            Server = server;
            Port = port;

            Nickname = nickname;
            Username = username;
            Realname = realname;

            Channels = channels;

            handlers = new Dictionary<string, CommandHandler>();

            handlers["PING"] = (msg) => {
                Send("PONG :{0}", msg.Arguments[0]);
            };

            handlers["001"] = (msg) => {
                foreach(string channel in Channels) {
                    Send("JOIN {0}", channel);
                }
            };
        }

        public void AddHandler(string command, CommandHandler handler) {
            if(handlers.ContainsKey(command)) {
                handlers[command] += handler;
            } else {
                handlers[command] = handler;
            }
        }

        public void RemoveHandler(string command, CommandHandler handler) {
            if(handlers.ContainsKey(command)) {
                handlers[command] -= handler;
            } else {
                throw new ArgumentException("Attempted to remove handler that is not registered.");
            }
        }

        public void Send(string format, params object[] xs) {
            string str = string.Format(format, xs);
            Console.WriteLine("<< " + str);
            writer.Write(str + "\r\n");
            writer.Flush();
        }

        public void Run() {
            Connect();
            Register();

            while(Connected) {
                string line = reader.ReadLine();

                if(line == null) {
                    Connected = false;
                    break;
                }

                System.Console.WriteLine(">> " + line);
                var msg = IrcMessage.Parse(line);

                if(handlers.ContainsKey(msg.Command))
                    handlers[msg.Command](msg);
            }
        }

        private void Connect() {
            try {
                tcp = new TcpClient(Server, Port);
                var ns = tcp.GetStream();
                reader = new StreamReader(ns);
                writer = new StreamWriter(ns);
                Connected = true;
            } catch(Exception) {
                Connected = false;
                throw;
            }
        }

        private void Disconnect() {
            Connected = false;

            if(tcp != null)
                ((IDisposable)tcp).Dispose();
        }

        private void Register() {
            Send("NICK {0}", Nickname);
            Send("USER {0} * * :{1}", Username, Realname);
        }
    }
}
