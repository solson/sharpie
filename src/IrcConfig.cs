namespace Sharpie {
    public class IrcConfig {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Nickname { get; set; }
        public string Username { get; set; }
        public string Realname { get; set; }
        public string[] Channels { get; set; }

        public IrcConfig() {
            // Set the default values.
            Port = 6667;
            Username = "sharpie";
            Realname = "Sharpie IRC bot";
            Channels = new string[]{};
        }
    }
}
