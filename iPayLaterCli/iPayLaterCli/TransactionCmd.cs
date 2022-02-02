
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iPayLaterCli
{
    [Command(Name = "transaction", Description = "to add the transaction details of user")]
    class TransactionCmd : iPayLaterCmdBase
    {
        [Option(CommandOptionType.NoValue, ShortName = "n", LongName = "new", Description = "new transaction", ValueName = "new", ShowInHelpText = true)]
        public bool newtransaction { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "user", Description = "PayLater username", ValueName = "user name", ShowInHelpText = true)]
        public string userName { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "m", LongName = "merchant", Description = "PayLater merchant name", ValueName = "merchant name", ShowInHelpText = true)]
        public string merchantName { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "a", LongName = "amount", Description = "PayLater transaction amount", ValueName = "amount", ShowInHelpText = true)]
        public string amount { get; set; }

        public TransactionCmd(ILogger<MerchantCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }
        private iPayLaterCmd Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if (newtransaction)
            {

                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(merchantName) || string.IsNullOrEmpty(amount))
                {
                    userName = Prompt.GetString("User name:", userName);
                    merchantName = Prompt.GetString("merchant name:", merchantName);
                    amount = Prompt.GetString("transaction amount:", amount);
                }

                try
                {
                    var User = new Transaction()
                    {
                        userName = userName,
                        merchant = merchantName,
                        amount = amount
                    };
                    string payload = JsonConvert.SerializeObject(User);
                    HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                    Uri url = new Uri($"{Client.BaseUrl}/transaction/");
                    var response = await iPayLaterClient.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("new transaction added");
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        Console.WriteLine($"transaction failed - {resultJson.message}");
                        return 1;
                    }


                }
                catch (Exception ex)
                {
                    OnException(ex);
                    return 1;
                }
            }
            return 0;
        }
    }
}
