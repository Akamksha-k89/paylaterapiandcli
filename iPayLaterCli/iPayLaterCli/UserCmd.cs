
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iPayLaterCli
{
    [Command(Name = "user", Description = "to register user details")]
    class UserCmd : iPayLaterCmdBase
    {
        [Option(CommandOptionType.NoValue, ShortName = "r", LongName = "register", Description = "register user", ValueName = "register", ShowInHelpText = true)]
        public bool register { get; set; } = false;

        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "user", Description = "PayLater username", ValueName = "user name", ShowInHelpText = true)]
        public string userName { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "e", LongName = "email", Description = "PayLater user email", ValueName = "user email", ShowInHelpText = true)]
        public string email { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "credit", Description = "PayLater user credit limit", ValueName = "user credit limit", ShowInHelpText = true)]
        public string credit { get; set; }

        public UserCmd(ILogger<MerchantCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }
        private iPayLaterCmd Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email))
            {
                userName = Prompt.GetString("User name:", userName);
                email = Prompt.GetString("User email:", email);
                credit = Prompt.GetString("user credit limit:", credit);
            }

            try
            {
                var User = new UserProfile()
                {
                    userName = userName,
                    email = email,
                    creditLimit = credit
                };
                string payload = JsonConvert.SerializeObject(User);
                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                if (register)
                {
                    Uri url = new Uri($"{Client.BaseUrl}/user/");
                    var response = await iPayLaterClient.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("new user registered");
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        Console.WriteLine($"user registration failed - {resultJson.message}");
                    }

                }
                return 0;

            }
            catch (Exception ex)
            {
                OnException(ex);
                return 1;
            }
        }
    }
}
