using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvHookTest.Controllers
{
    [Produces("application/json")]
    [Route("api/InvoiceAssistant")]
    public class InvoiceAssistantController : Controller
    {

        private IConfiguration Configuration { get; set; }
        private HttpClient rihardClient = null;
        private HttpClient biroInvAstClient = null;
        string biroInvAstPath = "";

        #region [Properties]
        public HttpClient RihardClient {
            get {
                if (rihardClient == null)
                    rihardClient = new HttpClient();
                return rihardClient;
            }
        }

        public HttpClient BiroInvAstClient {
            get {
                if (biroInvAstClient == null) {
                    biroInvAstClient = new HttpClient();
                    biroInvAstClient.BaseAddress = new Uri(Configuration.GetValue<string>("BiroInvoiceAssistantHost:Endpoint"));
                }
                return biroInvAstClient;
            }
        }
        #endregion

        #region [Constructor]
        public InvoiceAssistantController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region [API]
        [HttpGet]
        [HttpGet("ping", Name = "Ping")]
        public async Task<string> Ping() {
            return "Pong!";
        }

        [HttpPost]
        public async void Post() // needs to be this way because algoritmik sends text/plain encoded by UTF8
        {
            Console.WriteLine("Incoming request");

            string content = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }

            string msgType = Request.Headers["x-amz-sns-message-type"];
            if (msgType != null && msgType == "SubscriptionConfirmation")
            {
                string returnMessage = VerifySubscription(content);
                Console.WriteLine("Registration finished: {0}", returnMessage);
            }
            else
            {
                StoreInvoiceInBazureBuffer(content);
            }
        }
        #endregion

        #region [auxiliary]
        private string VerifySubscription(string content)
        {
            dynamic RequestObject = JsonConvert.DeserializeObject(content);
            string SubscribeURL = RequestObject.SubscribeURL;

            if (rihardClient == null)
            {
                rihardClient = new HttpClient();
            }
            HttpResponseMessage msg = rihardClient.GetAsync(SubscribeURL).GetAwaiter().GetResult();
            return msg.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        private async void StoreInvoiceInBazureBuffer(string content)
        {
            string key = GetPollingKey(content);

            HttpResponseMessage msg = await BiroInvAstClient.GetAsync(string.Format("/api/invoice/process?inv_key={0}", key));
            Console.WriteLine("Call completed: " + msg.Content.ReadAsStringAsync());
        }
        #endregion

        #region [auxiliary to auxiliary]
        private string GetPollingKey(string content)
        {
            dynamic requestObject = JsonConvert.DeserializeObject(content);
            dynamic innerObject = null;

            try
            {
                innerObject = JsonConvert.DeserializeObject(requestObject.Message);
            }
            catch (Exception ex) { }
            try
            {
                innerObject = JsonConvert.DeserializeObject(requestObject.Message.Value);
            }
            catch (Exception ex) { }
            try
            {
                innerObject = JsonConvert.DeserializeObject(requestObject.Message.Value.Value);
            }
            catch (Exception ex) { }

            if (innerObject == null)
            {
                throw new Exception("Unable to parse the inner object from the content object" + content);
            }

            if (innerObject.status != "DONE")
            {
                // log that something has gone wrong and log the
                // innerObject state as well.
                throw new Exception("InvoiceAssistant has not finished processing the polled invoice.");
            }
            string inv_key = "";
            Exception tmpException = null;
            try
            {
                inv_key = innerObject.inv_key;
            }
            catch (Exception ex)
            {
                tmpException = ex;
            }
            try
            {
                inv_key = innerObject.inv_key.Value;
            }
            catch (Exception ex)
            {
                tmpException = ex;
            }

            if (inv_key == "")
                throw tmpException;
            return inv_key;
        }
        #endregion
    }
}