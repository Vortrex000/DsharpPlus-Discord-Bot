/* NameSpaces */
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
/* NameSpaces */

namespace FlamesBotV2
{
    public class Commands : BaseCommandModule
    {
        [Command("ping")]
        public async Task PingPrefixCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong!");
        }
    }
}
