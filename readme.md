## BiroInvoiceAssistantWebhook

BiroInvoiceAssistant web hook is the endpoint to which processed invoices from the InvoiceAssistant get posted via HTTPS.

The endpoint needs to ensure a HTTPS connection.


### TODOS

- Need to enable HTTPS in the .NET application

https://blogs.msdn.microsoft.com/webdev/2017/11/29/configuring-https-in-asp-net-core-across-different-platforms/

- Need to expose a POST method, to which the InvoiceAssistant API can connect.

### Using

the default path for the API is set up at 
```
https://localhost:44340/api/[controller]
```

- Run the server in Visual Studio
- ```ngrok tls 44340```
- The webhook is now exposed on ngrok's addres (e.g. ```https://iej834j28.ngrok.io/api/invoiceassistant```)


### Configuring

- The source article was:
	- Follow the instructions in [here](https://blogs.msdn.microsoft.com/webdev/2017/11/29/configuring-https-in-asp-net-core-across-different-platforms/).

	- a [sample project](https://github.com/aspnet/samples/blob/master/samples/aspnetcore/security/KestrelHttps/KestrelServerOptionsExtensions.cs) on github.

##### Create and register a ```.pfx``` certificate using pkcs12.
- Windows

```
New-SelfSignedCertificate -NotBefore (Get-Date) -NotAfter (Get-Date).AddYears(1) -Subject "localhost" -KeyAlgorithm "RSA" -KeyLength 2048 -HashAlgorithm "SHA256" -CertStoreLocation "Cert:\CurrentUser\My" -KeyUsage KeyEncipherment -FriendlyName "HTTPS development certificate" -TextExtension @("2.5.29.19={critical}{text}","2.5.29.37={critical}{text}1.3.6.1.5.5.7.3.1","2.5.29.17={critical}{text}DNS=localhost")
```

After this, add it to your certificate store in windows.

- OS X

	- Create a file called ```https.config``` and add this to it:

```
[ req ]
default_bits = 2048
default_md = sha256
default_keyfile = key.pem
prompt = no
encrypt_key = no

distinguished_name = req_distinguished_name
req_extensions = v3_req
x509_extensions = v3_req

[ req_distinguished_name ]
commonName = "localhost"

[ v3_req ]
subjectAltName = DNS:localhost
basicConstraints = critical, CA:false
keyUsage = critical, keyEncipherment
extendedKeyUsage = critical, 1.3.6.1.5.5.7.3.1
```

After, use

```
openssl req -config https.config -new -out csr.pem

openssl x509 -req -days 365 -extfile https.config -extensions v3_req -in csr.pem -signkey key.pem -out https.crt

openssl pkcs12 -export -out https.pfx -inkey key.pem -in https.crt -password pass:<password>
```

After this, add it to the system keychain using

```
sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain https.crt
```

#### Configure the server with appsettings.json

You need to configure the appsettings to be able to find your certificate when using HTTPS.

- OS X
```
"HttpServer":  {
    "Endpoints": {
      "Http":  {
        "Host": "localhost",
        "Port": 8080,
        "Scheme": "http"
      },
      "Https": {
        "Host": "localhost",
        "Port": 44340,
        "Scheme": "https",
        "FilePath": "/Users/km/Desktop/playground/birokrat/InvoiceAssistant/InvoiceAssistantWebhook/certificate_config/https.pfx",
        "Password": "spremeni1"
      }
    }
  }
```

- Windows

```
"HttpServer":{
    "Endpoints":{
        "Http":{
            "Host": "localhost",
            "Port": 8080,
            "Scheme": "http"
        },
        "Https":{
            "Host": "localhost",
            "Port": 44340,
            "Scheme": "https",
            "StoreName": "My",
            "StoreLocation": "CurrentUser"
        }
    }
}
```