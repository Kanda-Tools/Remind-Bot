using dotenv.net;
using System.Threading.Tasks;

namespace ReminderBot
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            DotEnv.Load();
            var bot = new Bot();
            await bot.RunAsync();
        }
    }
}
