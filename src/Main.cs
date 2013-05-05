using System;
using Sharpie;

class MainClass {
    static int Main(string[] args) {
        if(args.Length != 2) {
            Console.WriteLine("Usage: sharpie.exe SERVER CHANNEL");
            return 1;
        }

        string server = args[0];
        string channel = args[1];
        const string nick = "sharpie";

        var irc = new Irc(server, 6667, nick, nick, nick,
                new string[] { channel });

        irc.Run();

        return 0;
    }
}
