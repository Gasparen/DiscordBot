using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Info : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        [Alias("echo")]
        public async Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }
    }
}
