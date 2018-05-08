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

using BiroInvoiceAssistant;
using BiroInvoiceAssistant.structs;
using BiroInvoiceAssistant.helpers;

using SharedUtilities.utils;

namespace InvHookTest.Controllers
{
    [Produces("application/json")]
    [Route("api/InvoiceAssistant")]
    public class InvoiceAssistantController : Controller
    {

        private IConfiguration Configuration { get; set; }
        private InvoiceAssistantTestAPI InvoiceAssistantAPI;
        private BazureInvoiceBufferAPI BazureBufferAPI;

        #region [Constructor]
        public InvoiceAssistantController(IConfiguration configuration)
        {
            Configuration = configuration;

            string ApiEndpoint = Configuration.GetValue<string>("InvoiceAssistantAPI:ENDPOINT");
            string ApiKey = Configuration.GetValue<string>("InvoiceAssistantAPI:APIKEY");
            InvoiceAssistantAPI = new InvoiceAssistantTestAPI(ApiEndpoint, ApiKey);
            BazureBufferAPI = GetBazureInvoiceAPI(configuration);
        }
        #endregion

        #region [API]
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

        #region [Auxiliary]
        private static string VerifySubscription(string content)
        {
            dynamic RequestObject = JsonConvert.DeserializeObject(content);
            string SubscribeURL = RequestObject.SubscribeURL;
            HttpClient client = new HttpClient();
            HttpResponseMessage msg = client.GetAsync(SubscribeURL).GetAwaiter().GetResult();
            return msg.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        private void StoreInvoiceInBazureBuffer(string content)
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

            if (innerObject == null) {
                throw new Exception("Unable to parse the inner object from the content object" + content);
            }

            if (innerObject.status != "DONE")
            {
                // log that something has gone wrong and log the
                // innerObject state as well.
                return;
            }
            string inv_key = "";
            Exception tmpException = null;
            try
            {
                inv_key = innerObject.inv_key;
            }
            catch (Exception ex) {
                tmpException = ex;
            }
            try
            {
                inv_key = innerObject.inv_key.Value;
            }
            catch (Exception ex) {
                tmpException = ex;
            }

            if (inv_key == "")
                throw tmpException;



            SParsedInvoice parsedInvoice = InvoiceAssistantAPI.PollForInvoice(inv_key, false, true).GetAwaiter().GetResult();

            // store the record to the database
            SInvoiceRecord rec = CInvoiceRecordToIdentifierMapper.ReverseMap(parsedInvoice.file_name);
            rec.InvoiceAssistantContent = JsonConvert.SerializeObject(rec);

            BazureBufferAPI.SaveBufferRecord(rec);
        }


        private static BazureInvoiceBufferAPI GetBazureInvoiceAPI(IConfiguration configuration)
        {
            SBAzureSettings config = new SBAzureSettings(
                configuration.GetValue<string>("DatabaseConnection:Username"),
                configuration.GetValue<string>("DatabaseConnection:Password"),
                configuration.GetValue<string>("DatabaseConnection:ServerAddress"),
                configuration.GetValue<string>("DatabaseConnection:InitialCatalog"),
                configuration.GetValue<bool>("DatabaseConnection:IntegratedSecurity"),
                configuration.GetValue<string>("DatabaseConnection:TargetDatabase")
            );
            return new BazureInvoiceBufferAPI(config);
        }
        #endregion
    }
}