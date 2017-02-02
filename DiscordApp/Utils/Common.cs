using Discord;
using DiscordApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordApp.Utils
{
    public static class Common
    {
        public static void Log(object sender, LogMessageEventArgs e)
        {
            string consoleMsg = $"[{ e.Severity }] [{e.Source}] {e.Message}";

            if (e.Message == "Disconnected")
                Program.isConnected = false;

            if (e.Message == "Connected" && Program.isConnected)
                Program._client.FindServers(Definitions.ServerDefinitions.ServerName.GetStringValue())
                    .FirstOrDefault().TextChannels
                    .SingleOrDefault(x => x.Name.Contains(Definitions.TextChannels.ChatNChill.GetStringValue()))
                    .SendMessage(consoleMsg);

            Console.WriteLine(consoleMsg);
        }

        public static Channel GetTextChannel(string textChannel)
        {
            return Program._client.FindServers(Definitions.ServerDefinitions.ServerName.GetStringValue())
                .FirstOrDefault().TextChannels
                .FirstOrDefault(x => x.Name == textChannel);
        }
    }
}
