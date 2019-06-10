using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace KelschBot
{
    public class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public Int32 dwTimeout;
        }

        public const UInt32 FLASHW_STOP = 0;
        public const UInt32 FLASHW_CAPTION = 1;
        public const UInt32 FLASHW_TRAY = 2;
        public const UInt32 FLASHW_ALL = 3;
        public const UInt32 FLASHW_TIMER = 4;
        public const UInt32 FLASHW_TIMERNOFG = 12;

        Random r = new Random();
        public string[] responses =
        {
            "Yo this guy smells really bad",
            "Do you hear something?",
            "*shut up man*",
            "*yikes*",
            "Dude why can't we just ban this kid",
            "Holy shit just fucking die jfc you're so retarded like why are you even here?",
            "Are you always so stupid or is today a special occasion?",
            "<:downvote:522279950515175424>",
            "<:downvote:522279950515175424>"
        };
        public string[] hydecaption =
       {
            "I'm that gorilla dick nigga, I make dike pussy wet.",
            "If you like sports you're a fucking faggot",
            "When we win, do not forget that these people want you broke, dead, your kids raped and brainwashed, and they think it's funny",
            "Stand up like men! and reclaim our soil.\nKinsmen arise! Look toward the stars and proclaim our destiny.\nIn Metaline Falls we have a saying, “Defeat never. Victory forever!",
            "My mom says im part jewish",
            "opioids are the religion of the masses",
            "I slept with a tranny. That's right kids! I sucked another man's dick, and convinced myself that it doesn't make me gay!",
            "I like to shill my inane, gay bullshit on /pol/.",
            "everyone who gave me money is a fucking idiot. You fucking retards actually thought I wouldn't waste it on cars and bitcoin. I know Charles went back to working a manual labor job but fuck him, I'm way funnier and smarter.",
            "My mom is cool and my mom will treat you right",
            "Imagine you just got accepted into an Ivy League school and you rejected them. That's Purple gangsta. Imagine you got the dopest piece of pussy you ever had and she's all into that. And she's 420 friendly. That's Shemale Kush."
        };
        public string[] funfacts =
        {
            "McDonald’s once made bubblegum-flavored broccoli",
            "Some fungi create zombies, then control their minds",
            "The first oranges weren’t orange",
            "There’s only one letter that doesn’t appear in any U.S. state name",
            "A cow-bison hybrid is called a “beefalo”",
            "Johnny Appleseed’s fruits weren’t for eating",
            "Scotland has 421 words for “snow”",
            "Samsung tests phone durability with a butt-shaped robot",
            "The “Windy City” name has nothing to do with Chicago weather",
            "Peanuts aren’t technically nuts",
            "Armadillo shells are bulletproof",
            "Firefighters use wetting agents to make water wetter",
            "The longest English word is 189,819 letters long",
            "“Running amok” is a medically recognized mental condition",
            "Octopuses lay 56,000 eggs at a time",
            "Cats have fewer toes on their back paws",
            "Kleenex tissues were originally intended for gas masks",
            "Blue whales eat half a million calories in one mouthful",
            "That tiny pocket in jeans was designed to store pocket watches",
            "Turkeys can blush",
            "Iceland’s last McDonald’s burger was sold eight years ago …",
            "The man with the world’s deepest voice can make sounds humans can’t hear",
            "The American flag was designed by a high school student",
            "Thanks to 3D printing, NASA can basically “email” tools to astronauts"
        };
        public string[] funfactsd =
        {
            "This interesting fact will have your taste buds crawling. Unsurprisingly, the attempt to get kids to eat healthier didn’t go over well with the child testers, who were 'confused by the taste.'",
            "The tropical fungus Ophiocordyceps infects ants’ central nervous systems. By the time the fungi been in the insect bodies for nine days, they have complete control over the host’s movements. They force the ants to climb trees, then convulse and fall into the cool, moist soil below, where fungi thrive. Once there, the fungus waits until exactly solar noon to force the ant to bite a leaf and kill it",
            "The original oranges from Southeast Asia were a tangerine-pomelo hybrid, and they were actually green. In fact, oranges in warmer regions like Vietnam and Thailand still stay green through maturity.",
            "Can you guess the answer to this random fact? You’ll find a Z (Arizona), a J (New Jersey), and even two X’s (New Mexico and Texas)—but not a single Q.",
            "You can even buy its meat in at least 21 states",
            "Yes, there was a real John Chapman who planted thousands of apple trees on U.S. soil. But the apples on those trees were much more bitter than the ones you’d find in the supermarket today. “Johnny Appleseed” didn’t expect his fruits to be eaten whole, but rather made into hard apple cider.",
            "421?! Some examples: *sneesl* (to start raining or snowing); *feefle* (to swirl); *flinkdrinkin* (a light snow).",
            "Do these interesting facts have you rethinking everything? People stash their phones in their back pockets all the time, which is why Samsung created a robot that is shaped like a butt—and yes, even wears jeans—to “sit” on their phones to make sure they can take the pressure.",
            "Was this one of the random facts you already knew? Chicago’s nickname was coined by 19th-century journalists who were referring to the fact that its residents were “windbags” and “full of hot air.”",
            "They’re legumes. According to Merriam-Webster, a nut is only a nut if it’s “a hard-shelled dry fruit or seed with a separable rind or shell and interior kernel.” That means walnuts, almonds, cashews, and pistachios aren’t nuts either. They’re seeds.",
            "In fact, one Texas man was hospitalized when a bullet he shot at an armadillo ricocheted off the animal and hit him in the jaw.",
            "The chemicals reduce the surface tension of plain water so it’s easier to spread and soak into objects, which is why it’s known as “wet water.”",
            "We won’t spell it out here (though you can read it here), but the full name for the protein nicknamed titin would take three and a half hours to say out loud",
            "Considered a culturally bound syndrome, a person “running amok” in Malaysia commits a sudden, frenzied mass attack, then begins to brood.",
            "The mother spends six months so devoted to protecting the eggs that she doesn’t eat. The babies are the size of a grain of rice when they’re born.",
            "Like most four-legged mammals, they have five toes on the front, but their back paws only have four toes. Scientists think the four-toe back paws might help them run faster",
            "When there was a cotton shortage during World War I, Kimberly-Clark developed a thin, flat cotton substitute that the army tried to use as a filter in gas masks. The war ended before scientists perfected the material for gas masks, so the company redeveloped it to be smoother and softer, then marketed Kleenex as facial tissue instead.",
            "Those 457,000 calories are more than 240 times the energy the whale uses to scoop those krill into its mouth.",
            "The original jeans only had four pockets: that tiny one, plus two more on the front and just one in the back.",
            "When turkeys are scared or excited—like when the males see a female they’re interested in—the pale skin on their head and neck turns bright red, blue, or white. The flap of skin over their beaks, called a “snood,” also reddens.",
            "… and you can still see it today. Its home is in a hostel, but you can catch a glimpse on the 24/7 live webcam stream dedicated to it. These interesting facts keep getting weirder and weirder.",
            "The man, Tim Storms, can’t even hear the note, which is eight octaves below the lowest G on a piano—but elephants can.",
            "It started as a school project for Bob Heft’s junior-year history class, and it only earned a B- in 1958. His design had 50 stars even though Alaska and Hawaii weren’t states yet. Heft figured the two would earn statehood soon and showed the government his design. After President Dwight D. Eisenhower called to say his design was approved, Heft’s teacher changed his grade to an A.",
            "Getting new equipment to the Space Station used to take months or years, but the new technology means the tools are ready within hours."
        };
        public string[] shitlist =
        {
            "" 
               // +"208322021417943040" 
                ,
                "","","","","","",""
        };
        private static void FlashWindow(IntPtr hWnd)
        {
            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = Process.GetCurrentProcess().MainWindowHandle;
            fInfo.dwFlags = FLASHW_TRAY | FLASHW_TIMERNOFG;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;
            FlashWindowEx(ref fInfo);
        }
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            
            var client = new DiscordSocketClient();
            client.Log += Log;
            string token = "NTI0Mzc3MzMwODU3MTQ4NDMy.Dx447g.ARwLPJez52VpGuYQvoJKRGPxfsw";
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await client.SetGameAsync("Just Shitting Around | !pog");
            client.MessageReceived += MessageReceived;

            //Block this task until program is closed
            await Task.Delay(-1);
        }
        private async Task MessageReceived(SocketMessage message)
        {
            FlashWindow(Process.GetCurrentProcess().MainWindowHandle);
            SocketGuild server = ((SocketGuildChannel)message.Channel).Guild;
            Console.WriteLine(message.Author+" said:");
            Console.WriteLine(message.Content);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter($"{server.Name} - Log.txt", true))
            {
                await file.WriteLineAsync(System.DateTime.Now+" | "+ message.Channel +" | "+ message.Author + " said:");
                await file.WriteLineAsync(message.Content);
            } 
        

            if (message.Content.ToLower() == "!pog")
            {//<:pog:524455455506956308> Willko Official
                IEmote emote = Emote.Parse("<:Pog:478342281183756288>");
                //IEmote emote = Emote.Parse("<:pog:524455455506956308>");
                var m = (RestUserMessage)await message.Channel.GetMessageAsync(message.Id);
                await m.AddReactionAsync(emote);

                 await message.Channel.SendMessageAsync("<:Pog:478342281183756288>");
                

            }
            if (message.Content.ToLower().Contains("!pog purge "))
            {
                try
                {
                    int messageCount = int.Parse(message.Content.Substring(10));
                    IEnumerable<IMessage> messages = await message.Channel.GetMessagesAsync(messageCount + 1).Flatten();
                    await message.Channel.DeleteMessagesAsync(messages);
                    if (messageCount > 1)
                    {
                        await message.Channel.SendMessageAsync("Successfully deleted " + messageCount + " messages");
                        Console.WriteLine("Successfully deleted " + messageCount + " messages");
                    }
                    else { 
                        await message.Channel.SendMessageAsync("Successfully deleted " + messageCount + " message");
                        Console.WriteLine("Successfully deleted " + messageCount + " message");
                }
                }
                catch(Exception e)
                {
                    await message.Channel.SendMessageAsync("***Invalid Input*** \n"+e.Message);
                    Console.WriteLine("   Invalid Input   " + e.Message);
                }

            }
            if (message.Content.ToLower().Contains("<@524377330857148432> Thank".ToLower()) ||message.Content.ToLower().Contains("<@524377330857148432> Ty".ToLower()))
            {//<:pog:524455455506956308> Willko Official
                Console.WriteLine("Thanks Recieved");
                IEmote emote = new Emoji("😉");

                await message.Channel.SendMessageAsync("You're very welcome "+emote);


            }
            
            // angelo
            foreach (string d in shitlist)
            if (message.Author.Id.ToString() == d)
                await message.Channel.SendMessageAsync(responses[new Random().Next(0,responses.Length)]);
            // Sam Hyde
            if (message.Content.ToLower() == "!hydeme")
            {
                Console.WriteLine("Sending hydeman");
                await message.Channel.SendFileAsync("hyde\\hyde (" + r.Next(1, 9) + ").jpg", hydecaption[r.Next(hydecaption.Length)]);
            }
            if (message.Content.ToLower().Contains("!fun"))
            {
                Console.WriteLine("Sending fun fact");
                int hh = r.Next(funfacts.Length);
                await message.Channel.SendMessageAsync(funfacts[hh]+"```"+funfactsd[hh]+"```");
            }
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
