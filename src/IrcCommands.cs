namespace Sharpie {
    public partial class Irc {
        public delegate void ChannelMessageHandler(Sender sender, string channel, string message);
        public void OnChannelMessage(ChannelMessageHandler handler) {
            AddHandler("PRIVMSG", (msg) => {
                if(msg.Arguments[0][0] == '#')
                    handler(msg.Sender, msg.Arguments[0], msg.Arguments[1]);
            });
        }

        public delegate void PrivateMessageHandler(Sender sender, string message);
        public void OnPrivateMessage(PrivateMessageHandler handler) {
            AddHandler("PRIVMSG", (msg) => {
                if(msg.Arguments[0][0] != '#')
                    handler(msg.Sender, msg.Arguments[1]);
            });
        }

        public void SendMessage(string receiver, string format, params object[] xs) {
            Send("PRIVMSG {0} :{1}", receiver, string.Format(format, xs));
        }
    }
}
