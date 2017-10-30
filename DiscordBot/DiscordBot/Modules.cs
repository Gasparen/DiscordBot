using Discord.Commands;
using HtmlAgilityPack;
using System;
using System.Threading.Tasks;


namespace DiscordBot
{
    public class Info : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        [Alias("echo")]
        
        // How the F do I add a log call from here?
        // Do I need to subscribe to the logger somehow?
        // Or add it earlier?
        public async Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }
    }

    public class Portal : ModuleBase<SocketCommandContext>
    {
        [Command("quote")]
        [Summary("Prints a random Portal quote")]
        [Alias("Quote", "portal", "Portal", "portal2", "Portal2")]
        public async Task Quote()
        {
            Console.WriteLine("Modules - Portal - Quote - Enter quote");
            var url = "https://theportalwiki.com/wiki/Turret_voice_lines";
            var web = new HtmlWeb();
            Console.WriteLine("Modules - Portal - Quote - Load Url");
            var doc = web.Load(url);

            Console.WriteLine("Modules - Portal - Quote - Select Nodes");
            var nodes = doc.DocumentNode.SelectNodes("//a[@class=\"internal\"]");


            Console.WriteLine("Modules - Portal - Quote - Number of nodes:" + nodes.Count);
                
            foreach (var node in nodes)
            {
                Console.WriteLine(node.InnerText);
            }
                

            await ReplyAsync("Here a quote will be added");
        }
    }
}
