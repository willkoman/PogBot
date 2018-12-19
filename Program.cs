using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace KelschBot
{
    public class Program
    {
        public string[] responses =
        {
            "Yo this guy smells really bad",
            "Do you hear something?",
            "*yikes*",
            "*yikes*",
            "Dude why can't we just ban this kid",
            "Holy shit just fucking die jfc you're so retarded like why are you even here?",
            "Are you always so stupid or is today a special occasion?",
            "<:downvote:522279950515175424>",
            "<:downvote:522279950515175424>"
        };
        public string[] shitlist =
        {
            "208322021417943040","","","","","",""
        };
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            
            var client = new DiscordSocketClient();
            client.Log += Log;
            string token = "No Thanks Dad";
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await client.SetGameAsync("Just Shitting Around | !pog");
            client.MessageReceived += MessageReceived;

            //Block this task until program is closed
            await Task.Delay(-1);
        }
        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!pog")
            {//<:pog:524455455506956308> Willko Official
                IEmote emote = Emote.Parse("<:Pog:478342281183756288>");
                //IEmote emote = Emote.Parse("<:pog:524455455506956308>");
                var m = (RestUserMessage)await message.Channel.GetMessageAsync(message.Id);
                await m.AddReactionAsync(emote);

                 await message.Channel.SendMessageAsync("<:Pog:478342281183756288>");
                

            }
            if (message.Content.Contains("!pog purge "))
            {
                try
                {
                    int messageCount = int.Parse(message.Content.Substring(10));
                    IEnumerable<IMessage> messages = await message.Channel.GetMessagesAsync(messageCount + 1).Flatten();
                    await message.Channel.DeleteMessagesAsync(messages);
                    if(messageCount>1)
                    await message.Channel.SendMessageAsync("Successfully deleted "+messageCount+" messages");
                    else
                    await message.Channel.SendMessageAsync("Successfully deleted " + messageCount + " message");
                }
                catch(Exception e)
                {
                    await message.Channel.SendMessageAsync("***Invalid Input***");
                }

            }
            if (message.Content.ToLower().Contains("<@524377330857148432> Thank".ToLower()) ||message.Content.ToLower().Contains("<@524377330857148432> Ty".ToLower()))
            {//<:pog:524455455506956308> Willko Official
                Console.WriteLine("Thanks Recieved");
                IEmote emote = new Emoji("😉");

                await message.Channel.SendMessageAsync("You're very welcome "+emote);


            }
            if(message.Content.ToLower().Contains("nigg".ToLower())||message.Content.ToLower().Contains("n word".ToLower()) || message.Content.ToLower().Contains("n-word".ToLower()))
                await message.Channel.SendMessageAsync("***OH GOD MRS. OBAMA GET DOWN***");
            // angelo
            foreach (string d in shitlist)
            if (message.Author.Id.ToString() == d)
                await message.Channel.SendMessageAsync(responses[new Random().Next(0,responses.Length)]);

            if (message.Content == "!pog")
                await message.Channel.SendFileAsync("C:\\Pictures\\something.png", "Caption goes here");

        }
            

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
