using RestSharp;
using System.IO;
using System;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace ToDoListWeb.EmailServices
{
    public class EmailService
    {

        private readonly string mailgunApiKey = "24982a91a0f33a65ccd9c828804e6165-262b213e-99127dbf";
        private readonly string mailgunDomain = "sandbox9540eb10bbac451fa6ff98598559289d.mailgun.org";

       

        public async Task<RestResponse> SendSimpleMessageAsync(string from, string to, string subject, string body)
        {
            var client = new RestClient($"https://api.mailgun.net/v3/{mailgunDomain}/messages");
            client.AddDefaultParameter("apiKey", mailgunApiKey, ParameterType.QueryString);

            var request = new RestRequest();
            request.AddParameter("from", from);
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("text", body);

            request.AddParameter("method", "POST");

            var response = await client.ExecuteAsync(request);
            return response;
        }
    }
}

