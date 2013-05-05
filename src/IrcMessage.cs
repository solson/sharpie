using System;
using System.Collections.Generic;

namespace Sharpie {
    public class IrcMessage {
        public string Prefix { get; private set; }
        public string Command { get; private set; }
        public List<string> Arguments { get; private set; }

        public IrcMessage(string prefix, string command, List<string> args) {
            Prefix = prefix;
            Command = command;
            Arguments = args;
        }

        public static IrcMessage Parse(string line) {
            string prefix = null;

            // Check for a prefix.
            if(line.StartsWith(":")) {
                int endPrefix = line.IndexOf(" ");

                if(endPrefix == -1)
                    throw new ArgumentException("Invalid IRC message");

                prefix = line.Substring(1, endPrefix - 1);
                line = line.Substring(endPrefix + 1);
            }

            // Parse the command name or numeric.
            string command;

            int endCommand = line.IndexOf(" ");

            if(endCommand == -1) {
                command = line;
                line = null;
            } else {
                command = line.Substring(0, endCommand);
                line = line.Substring(endCommand + 1);
            }

            if(string.IsNullOrWhiteSpace(command))
                throw new ArgumentException("Invalid IRC message");

            // Parse the command arguments.
            List<string> args = new List<string>();

            while(!string.IsNullOrWhiteSpace(line)) {
                if(line.StartsWith(":")) {
                    args.Add(line.Substring(1));
                    line = null;
                } else {
                    int endArg = line.IndexOf(" ");

                    if(endArg == -1) {
                        args.Add(line);
                        line = null;
                    } else {
                        args.Add(line.Substring(0, endArg));
                        line = line.Substring(endArg + 1);
                    }
                }
            }

            return new IrcMessage(prefix, command, args);
        }
    }
}
