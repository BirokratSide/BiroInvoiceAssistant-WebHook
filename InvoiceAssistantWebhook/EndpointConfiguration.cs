using System;

namespace InvoiceAssistantWebhook
{
    public class EndpointConfiguration
    {
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Scheme { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public string CertificateFilePath { get; set; }
        public string CertificatePassword { get; set; }
    }
}
