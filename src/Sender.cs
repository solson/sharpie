using System;

namespace Sharpie {
    public class Sender {
        public string Nickname { get; private set; }
        public string Username { get; private set; }
        public string Hostname { get; private set; }
        public bool IsServer { get; private set; }

        public Sender(string nick, string user, string host) {
            Nickname = nick;
            Username = user;
            Hostname = host;
            IsServer = false;
        }

        public Sender(string server) {
            Hostname = server;
            IsServer = true;
        }

        public static Sender Parse(string sender) {
            // Find the end of the nick, if there is a nickname.
            int endNick = sender.IndexOf('!');

            // If there is no !, then it's a server rather than a user.
            if(endNick == -1) {
                return new Sender(sender);
            }

            string nick = sender.Substring(0, endNick);
            sender = sender.Substring(endNick + 1);

            // Find the end of the username;
            int endUser = sender.IndexOf('@');

            if(endUser == -1) {
                throw new ArgumentException("Invalid IRC message sender.");
            }

            string user = sender.Substring(0, endUser);

            // The rest of the string is the hostname.
            string host = sender.Substring(endUser + 1);

            return new Sender(nick, user, host);
        }
    }
}
