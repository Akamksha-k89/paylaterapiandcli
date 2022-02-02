
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
    [Command(Name = "merchant", Description = "to register and update merchant details")]
    class MerchantCmd : iPayLaterCmdBase
    {
        [Option(CommandOptionType.NoValue, ShortName = "n", LongName = "register", Description = "register merchant", ValueName = "register", ShowInHelpText = true)]
        public bool register { get; set; } = false;

        [Option(CommandOptionType.NoValue, ShortName = "u", LongName = "update", Description = "update merchant discount", ValueName = "update", ShowInHelpText = true)]
        public bool update { get; set; } = false;

        [Option(CommandOptionType.SingleValue, ShortName = "s", LongName = "merchant", Description = "PayLater merchant username", ValueName = "merchant username", ShowInHelpText = true)]
        public string merchantName { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "d", LongName = "discount", Description = "PayLater merchant discount", ValueName = "merchant discount", ShowInHelpText = true)]
        public string discount { get; set; }

        public MerchantCmd(ILogger<MerchantCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }
        private iPayLaterCmd Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(merchantName) || string.IsNullOrEmpty(discount))
            {
                merchantName = Prompt.GetString("Merchant Username:", merchantName);
                discount = Prompt.GetString("Merchant Discount:", discount);

            }

            try
            {
                var Merchant = new Merchant()
                {
                    userName = merchantName,
                    discount = discount
                };
                string payload = JsonConvert.SerializeObject(Merchant);
                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                if (register)
                {
                    Uri url = new Uri($"{Client.BaseUrl}/merchant/");
                    var response = await iPayLaterClient.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("new merchant registered");
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        Console.WriteLine($"merchant registration failed - {resultJson.message}");
                    }
                   
                }
                else if(update)
                {
                    Uri url = new Uri($"{Client.BaseUrl}/merchant/");
                    var response = await iPayLaterClient.PutAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("merchant updated");
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        Console.WriteLine($"merchant update failed - {resultJson.message}");
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
