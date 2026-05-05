using Acly.Requests;
using Acly.Tasks;

namespace Acly.Tests
{
    public class ServerTests
    {
        [Test]
        public async Task PostTest()
        {
            Api.Server server = new("https://api.acly.ru/oauth", "php");
            Dictionary<string, string> parameters = new()
            {
                { "email", "gdanovvk@gmail.com" }
            };

            var result = await server.PostString("signin", parameters);
            Console.WriteLine(result);
        }
    }
}
