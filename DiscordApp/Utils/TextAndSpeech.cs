using Discord;
using Discord.Audio;
using Discord.Commands;
using DiscordApp.Enums;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiscordApp.Utils
{
    public static class TextAndSpeech
    {
        public static void InstantiateVoiceConnection()
        {
            JoinMostPopulatedVoiceChannel();
            try { Program._vClient.Send(null, 0, 0); } catch (NullReferenceException) { }
        }

        public static void Speak(string file, CommandEventArgs e)
        {
            try
            {
                if (!Program.isConnected)
                    InstantiateVoiceConnection();
                if (Program._vClient != null)
                {
                    var channelCount = Program._client.GetService<AudioService>().Config.Channels;
                    var OutFormat = new WaveFormat(48000, 16, channelCount);
                    using (var MP3Reader = new Mp3FileReader(Definitions.ServerDefinitions.AudioPath.GetStringValue() + file + ".mp3"))
                    using (var resampler = new MediaFoundationResampler(MP3Reader, OutFormat))
                    {
                        resampler.ResamplerQuality = 60;
                        int blockSize = OutFormat.AverageBytesPerSecond / 50;
                        byte[] buffer = new byte[blockSize];
                        int byteCount;

                        while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0)
                        {
                            if (byteCount < blockSize)
                            {
                                for (int i = byteCount; i < blockSize; i++)
                                    buffer[i] = 0;
                            }
                            Program._vClient.Send(buffer, 0, blockSize);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Common.GetTextChannel(Definitions.TextChannels.ChatNChill.GetStringValue()).SendTTSMessage("That file wasn't found.");
            }
        }

        public static void JoinMostPopulatedVoiceChannel()
        {
            try
            {
                var voiceChannel = Program._client.FindServers(Definitions.ServerDefinitions.ServerName.GetStringValue())
                .FirstOrDefault().VoiceChannels
                .Where(x => x.Users.Count() > 0)
                .OrderByDescending(x => x.Users.Count())
                .FirstOrDefault();
                Program._vClient = Program._client.GetService<AudioService>().Join(voiceChannel).Result;
            }
            catch (AggregateException)
            {
                Console.WriteLine("AggregateException occurred");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("NullReferenceException occurred");
            }
        }
        
        private static List<Server.Emoji> GetAllEmojis()
        {
            return Program._client.FindServers(Definitions.ServerDefinitions.ServerName.GetStringValue())
                .FirstOrDefault().CustomEmojis.ToList();
        }

        public static string EmojiToString(Server.Emoji emote)
        {
            return $"<:{emote.Name}:{emote.Id}>";
        }

        public static string GetRandomEmote()
        {
            List<Server.Emoji> emojiList = GetAllEmojis();
            Server.Emoji emote = emojiList.ElementAt(new Random().Next(0, emojiList.Count()));
            return EmojiToString(emote);
        }

        public static Dictionary<string, string> GetEmoteList()
        {
            List<Server.Emoji> emojiList = GetAllEmojis();
            Dictionary<string, string> emojiStringList = new Dictionary<string, string>();
            foreach (Server.Emoji emote in emojiList)
            {
                emojiStringList.Add(emote.Name, EmojiToString(emote));
            }
            return emojiStringList;
        }

        public static string StringAllEmotes()
        {
            return string.Join(" ", GetEmoteList().Select(x => x.Value));
        }

        public static string GetEmote(string emote)
        {
            return EmojiToString(GetAllEmojis().FirstOrDefault(x => x.Name.Equals(emote, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
