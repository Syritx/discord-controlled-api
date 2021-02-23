using Discord.Commands;
using System.Threading.Tasks;
using System.Text.Json;

using api.src.disc.http;

namespace api.src.disc {

    public class Cmds : ModuleBase<SocketCommandContext> {
        
        [Command("toggle")]
        public async Task Toggle() {
            try {
                string message = await Https.PreformHttpRequest(Https.HttpType.PostRequest, "toggle-status");
                await ReplyAsync(message);
            }
            catch (System.Exception e) {
                await ReplyAsync(e.StackTrace);
            }
        }

        [Command("status")]
        public async Task GetStatus() {
            try {
                string message = await Https.PreformHttpRequest(Https.HttpType.GetRequest, "get-status");

                JsonDocument doc = JsonDocument.Parse(message);
                string status = doc.RootElement.GetProperty("status").ToString();

                await ReplyAsync("Current status: "+status);
            }
            catch (System.Exception e) {
                await ReplyAsync(e.StackTrace);
            }
        }
    }
}