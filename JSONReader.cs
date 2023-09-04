/* NameSpaces */
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
/* NameSpaces */
namespace FlamesBotV2
{
    public class JSONReader
    {
        public string DiscordToken { get; set; }
        public string DiscordPrefix { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader("config.json", new UTF8Encoding(false)))
            {
                string json = await sr.ReadToEndAsync();
                JSONStruct obj = JsonConvert.DeserializeObject<JSONStruct>(json);

                this.DiscordToken = obj.Token;
                this.DiscordPrefix = obj.Prefix;
            }
        }
    }
    internal sealed class JSONStruct
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
    }
}
