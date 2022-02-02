
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace iPayLaterCli
{
    [HelpOption("--help")]
    abstract class iPayLaterCmdBase
    {
        private iPayLaterClient _iPayLaterClient;

        protected ILogger _logger;
        protected IHttpClientFactory _httpClientFactory;
        protected IConsole _console;

        protected virtual Task<int> OnExecute(CommandLineApplication app)
        {
            return Task.FromResult(0);
        }

        protected string ProfileFolder
        {
            get
            {
                return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile, Environment.SpecialFolderOption.Create)}\\.istrada\\";
            }
        }


        protected iPayLaterClient iPayLaterClient
        {
            get
            {
                if (_iPayLaterClient == null)
                {
                    _iPayLaterClient = new iPayLaterClient(_httpClientFactory.CreateClient(), _logger);
                }

                return _iPayLaterClient;
            }
        }    

      

   

        protected void OnException(Exception ex)
        {
            OutputError(ex.Message);
            _logger.LogError(ex.Message);
            _logger.LogDebug(ex, ex.Message);
        }


        protected void Output(string data)
        {
            OutputToConsole(data);
        }

   
        protected void OutputToConsole(string data)
        {
            _console.BackgroundColor = ConsoleColor.Black;
            _console.ForegroundColor = ConsoleColor.White;
            _console.Out.Write(data);
            _console.ResetColor();
        }

        protected void OutputError(string message)
        {
            _console.BackgroundColor = ConsoleColor.Red;
            _console.ForegroundColor = ConsoleColor.White;
            _console.Error.WriteLine(message);
            _console.ResetColor();
        }
    }
}