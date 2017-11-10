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
        private string portalQuoteUrl = "https://theportalwiki.com/wiki/";
        private string[] quoteUrls = { "Turret_voice_lines", "GLaDOS_voice_lines", "Announcer_voice_lines", "Caroline_voice_lines", "Cave_Johnson_voice_lines", "Core_voice_lines", "Wheatley_voice_lines", "Defective_Turret_voice_lines" };
        private string[] quoteIdentities = { "Turret", "GlaDOS", "Announcer", "Caroline", "Cave Johnson", "Core", "Wheatley", "Defective_Turret_voice_lines" };

        [Command("quote")]
        [Summary("Prints a random Portal quote")]
        [Alias("Quote", "portal", "Portal", "portal2", "Portal2")]
        public async Task Quote()
        {
            var rnd = new Random();

            var identityIndex = rnd.Next(quoteUrls.Length);
            Console.WriteLine("identityIndex: " + identityIndex);
            Console.WriteLine("quoteUrls length: " + quoteUrls.Length + " Random Identity number: " + identityIndex);
            var url = portalQuoteUrl + quoteUrls[identityIndex];
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // This XPath isn't working properly for all of the wiki pages, I'll need to test this out more
            var quotes = doc.DocumentNode.SelectNodes("//*[@id=\"mw - content - text\"]/ul/li/i | //a[@class=\"internal\" and text() != \"Download\" and text() != \"Play\"]");
            var quoteIndex = rnd.Next(quotes.Count);
            Console.WriteLine("quoteIndex: " + quoteIndex);
            Console.WriteLine("Identity:" + quoteIdentities[identityIndex] + " quotes Count: " + quotes.Count + " Random Quote number: " + quoteIndex);
            var randomQuote = quotes[quoteIndex].InnerText;

            await ReplyAsync(quoteIdentities[identityIndex] + ": " + randomQuote);
        }
    }
}
