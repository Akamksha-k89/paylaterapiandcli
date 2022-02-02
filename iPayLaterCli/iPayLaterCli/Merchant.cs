using System;
using System.Collections.Generic;
using System.Text;

namespace iPayLaterCli
{
    class Merchant
    {
        public string userName { get; set; }

        public string email { get; set; }

        public string discount { get; set; }

    }

    public static class Client
    {
        public static string BaseUrl = @"http://localhost:3000/api";
    }
}
