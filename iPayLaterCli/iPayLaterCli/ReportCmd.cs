using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iPayLaterCli
{
    [Command(Name = "report", Description = "reports related to merchant,users")]
    class ReportCmd : iPayLaterCmdBase
    {
        [Option(CommandOptionType.NoValue, ShortName = "m", LongName = "merchant", Description = "List a merchant discount", ValueName = "merchant discount", ShowInHelpText = true)]
        public bool discountMerchant { get; set; } = false;

        [Option(CommandOptionType.NoValue, ShortName = "d", LongName = "dues", Description = "Lists a user dues", ValueName = "user dues", ShowInHelpText = true)]
        public bool duesOfUser { get; set; } = false;

        [Option(CommandOptionType.NoValue, ShortName = "t", LongName = "totaldues", Description = "Lists all the users with dues", ValueName = "user total dues", ShowInHelpText = true)]
        public bool totalDues { get; set; } = false;

        [Option(CommandOptionType.NoValue, ShortName = "c", LongName = "creditlimit", Description = "Lists all the users at credit limit", ValueName = "users at credit limit", ShowInHelpText = true)]
        public bool creditLimit { get; set; } = false;

        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "userName", Description = "name", ValueName = "username", ShowInHelpText = true)]
        public string userName { get; set; }

        public ReportCmd(ILogger<ReportCmd> logger, IConsole console, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _console = console;
            _httpClientFactory = clientFactory;
        }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            try
            {
                if (discountMerchant)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = Prompt.GetString("Merchant Username:", userName);

                    }
                    Uri url = new Uri($"{Client.BaseUrl}/merchant/GetActiveDiscount/{userName}");
                    var response = await iPayLaterClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        Console.Write($"Discount provided by merchant {userName} - {resultJson}");
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        Console.WriteLine($"report failed - {resultJson.message}");
                    }
                }
                else if(duesOfUser)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = Prompt.GetString("User name:", userName);
                        Uri url = new Uri($"{Client.BaseUrl}/user/dues/{userName}");
                        var response = await iPayLaterClient.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            dynamic resultJson = JsonConvert.DeserializeObject(result);
                            Console.Write($"Dues - {resultJson.dues}");
                        }
                        else
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            dynamic resultJson = JsonConvert.DeserializeObject(result);
                            Console.WriteLine($"report failed - {resultJson.message}");
                        }
                    }

                }
                else if(totalDues)
                {
                    Uri url = new Uri($"{Client.BaseUrl}/user/totalDues");
                    var response = await iPayLaterClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        if(resultJson.Count > 0)
                        {
                            Console.WriteLine("user       dues");
                            foreach (dynamic item in resultJson)
                            {
                                Console.WriteLine("--------------------------------------");
                                Console.WriteLine($"{item.userName}          {item.dues}");
                            }
                                Console.WriteLine("--------------------------------------");

                        }

                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        Console.WriteLine($"report failed - {resultJson.message}");
                    }

                }
                else if (creditLimit)
                {
                    Uri url = new Uri($"{Client.BaseUrl}/user/nearlyUsedCreditLimit");
                    var response = await iPayLaterClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        if (resultJson.Count > 0)
                        {
                            Console.WriteLine("users");
                            Console.WriteLine("-------------------");
                            foreach (dynamic item in resultJson)
                            {
                                Console.WriteLine($"{item.userName}");
                            }

                        }

                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic resultJson = JsonConvert.DeserializeObject(result);
                        Console.WriteLine($"report failed - {resultJson.message}");
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