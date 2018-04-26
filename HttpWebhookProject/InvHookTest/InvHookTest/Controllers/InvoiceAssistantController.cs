using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.IO;
using System.Text;
using Newtonsoft.Json;

using BiroInvoiceAssistant;

namespace InvHookTest.Controllers
{
    [Produces("application/json")]
    [Route("api/InvoiceAssistant")]
    public class InvoiceAssistantController : Controller
    {

        #region [Methods]

        [HttpPost]
        public async void Post() // needs to be this way because algoritmik sends text/plain encoded by UTF8
        {
            string content = "";
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                content = await reader.ReadToEndAsync();
            }

            string msgType = Request.Headers["x-amz-sns-message-type"];
            if (msgType != null && msgType == "SubscriptionConfirmation")
            {
                VerifySubscription(content);
            }
            else
            {
                RetrieveInvoice(content);
            }
        }
        #endregion

        #region [Auxiliary]
        private static void VerifySubscription(string content)
        {
            dynamic RequestObject = JsonConvert.DeserializeObject(content);
            string SubscribeURL = RequestObject.SubscribeURL;
            HttpClient client = new HttpClient();
            HttpResponseMessage msg = client.GetAsync(SubscribeURL).GetAwaiter().GetResult();
        }

        private static void RetrieveInvoice(string content)
        {
            dynamic RequestObject = JsonConvert.DeserializeObject(content);
            InvoiceAssistantTestAPI api = new BiroInvoiceAssistant.InvoiceAssistantTestAPI("endpoint", "apikey");
            api.PollForInvoice("pollkey", false, true);

            // store the record to the database
        }
        #endregion
    }
}