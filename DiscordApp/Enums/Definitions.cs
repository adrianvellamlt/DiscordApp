using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordApp.Enums
{
    public static class Definitions
    {
        public class StringValueAttribute : Attribute
        {
            public string StringValue { get; protected set; }
            public StringValueAttribute(string value)
            {
                StringValue = value;
            }
        }

        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public enum ServerDefinitions: int
        {
            [StringValue("Area 47")]
            ServerName = 1,
            [StringValue("Mjc2NDUyNzkwNzQ0OTA3Nzc2.C3PdEg.mE5MJ-ZJgnLxjrraTwEhj1fjdic")]
            Token = 2,
            [StringValue("-")]
            PrefixChar = 3,
            [StringValue(@"D:\Desktop\DiscordApp\DiscordApp\Audio\")]
            AudioPath = 4
        }

        public enum TextChannels: int
        {
            [StringValue("chatnchill")]
            ChatNChill = 1,
            [StringValue("developnchill")]
            DevelopAndChill = 2,
            [StringValue("bottestinggrounds")]
            BotTesting = 3,
            [StringValue("rules")]
            Rules = 4
        }

        public enum MarkDownText
        {
            [StringValue("*")]
            Italics = 1,
            [StringValue("**")]
            Bold = 2,
            [StringValue("***")]
            BoldItalics = 3,
            [StringValue("~~")]
            Strikeout = 4,
            [StringValue("__")]
            Underline = 5,
            [StringValue("__*")]
            UnderlineItalics = 6,
            [StringValue("__**")]
            UnderlineBold = 7,
            [StringValue("__***")]
            UnderlineBoldItalics = 8,
        }
    }
}
