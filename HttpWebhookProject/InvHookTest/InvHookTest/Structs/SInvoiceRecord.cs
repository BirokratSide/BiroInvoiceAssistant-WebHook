using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHookTest.Structs
{
    /* This same struct should be used on the bazure application side to create the InvoiceAssistant filename from it.
    */
    public class SInvoiceRecord
    {
        public string CompanyId;
        public string CompanyYearId;
        public string Oznaka;
        public string InvoiceAssistantFilename;
        public string InvoiceAssistantContent;
        public Dictionary<string, string> AdditionalParams;

        public bool IsValid() {
            return !(CompanyId == null || CompanyId == "" || CompanyYearId == null || CompanyYearId == "" || Oznaka == null || Oznaka == "" || InvoiceAssistantFilename == null || InvoiceAssistantFilename == "" || InvoiceAssistantContent == null || InvoiceAssistantContent == "");    
        }
    }
}
