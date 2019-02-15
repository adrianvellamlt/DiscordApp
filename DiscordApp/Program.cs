using Discord;
using Discord.Audio;
using Discord.Commands;
using DiscordApp.Enums;
using DiscordApp.Utils;
using NAudio.Wave;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args) => new Program().Start();

    public static DiscordClient _client;
    public static IAudioClient _vClient;
    private string token = Definitions.ServerDefinitions.Token.GetStringValue();
    public static bool isConnected = false;

    public void Start()
    {
        _client = new DiscordClient(x =>
        {
            x.LogLevel = LogSeverity.Info;
            x.LogHandler = Common.Log;
        });

        _client.UsingCommands(x =>
        {
            x.PrefixChar = Convert.ToChar(Definitions.ServerDefinitions.PrefixChar.GetStringValue());
            x.AllowMentionPrefix = false;
            x.HelpMode = HelpMode.Public;
        });

        _client.UsingAudio(x =>
        {
            x.Mode = AudioMode.Both;
        });

        CreateCommand();

        try
        {
            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(token, TokenType.Bot);

                _client.UserJoined += async (s, e) => {
                    string channelMention = Common.GetTextChannel(Definitions.TextChannels.Rules.GetStringValue()).Mention;
                    await Common.GetTextChannel(Definitions.TextChannels.ChatNChill
                        .GetStringValue()).SendMessage($"Welcome {e.User.Mention}\nThese are our server {channelMention}\nAbide by them u **cuck**!");
                };

                isConnected = true;
                Thread.Sleep(1000);
                TextAndSpeech.InstantiateVoiceConnection();
            });
        }
        catch (ArgumentException) {
            Console.WriteLine("Press any key to stop bot.");
            Console.ReadKey();
        }

        
    }

    private void CreateCommand()
    {
        var commandService = _client.GetService<CommandService>();

        commandService.CreateCommand("whattodo")
            .Description("Gives you an option on what you should do.")
            .Do(async (e) =>
            {
                await e.Channel.SendTTSMessage("Kill yourself you fucking pleb");
            });

        commandService.CreateCommand("say")
                    .Alias(new string[] { "speak", "ghidli" })
                    .Description("Searches the audio repo for a file with the specified parameter.")
                    .Parameter("Phrase", ParameterType.Required)
                    .Do(e =>
                    {
                        TextAndSpeech.Speak(e.GetArg("Phrase"), e);
                    });

        commandService.CreateCommand("emote")
            .Parameter("Emote", ParameterType.Required)
            .Do(async (e) =>
            {
                string result = "";
                switch (e.GetArg("Emote").ToLower())
                {
                    case "all":
                        result = TextAndSpeech.StringAllEmotes();
                        break;
                    case "random":
                        result = TextAndSpeech.GetRandomEmote();
                        break;
                    default:
                        result = TextAndSpeech.GetEmote(e.GetArg("Emote"));
                        break;
                }
                await e.Channel.SendMessage(result);
            });
    }
}
