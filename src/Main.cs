using System;
using Sharpie;

class MainClass {
    static int Main(string[] args) {
        if(args.Length != 2) {
            Console.WriteLine("Usage: sharpie.exe SERVER CHANNEL");
            return 1;
        }

        var irc = new Irc(new IrcConfig() {
            Server = args[0],
            Nickname = "sharpie",
            Channels = new string[] { args[1] }
        });

        irc.OnChannelMessage((sender, channel, message) => {
            if(message.StartsWith(irc.Nickname + ": ")) {
                irc.SendMessage(channel, "{0}: {1}", sender.Nickname,
                        message.Substring(irc.Nickname.Length + 2));
            }
        });

        irc.Run();

        return 0;
    }
}
