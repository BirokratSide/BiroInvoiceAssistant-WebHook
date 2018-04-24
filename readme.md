## BiroInvoiceAssistantWebhook

BiroInvoiceAssistant web hook is the endpoint to which processed invoices from the InvoiceAssistant get posted via HTTPS.

The endpoint needs to ensure a HTTPS connection.


### TODOS

- Need to establish a secure TLS tunnel by getting a verified certificate.
	- https://ngrok.com/docs#bind-tls

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

## Theory

### Setting up a Secure HTTPS endpoint

### Setting up a Domain name and how DNS works

[Source article](https://www.digitalocean.com/community/tutorials/an-introduction-to-dns-terminology-components-and-concepts).

###### Setting up some terminology

- a domain name is structured in the following way

```
<host>.<name>.<tld>, example: www.mypage.com
```

- **TLD**: a server responsible for keeping domain under that top level path.
- **Root server**: Servers responsible for requests about TLDs. There are 13 in the world. Actually mirrored, but they share the same IP address. Root server doesn't know exact IP of the request, but know the name server which will hold the IP! 
	- if you query ```www.wikipedia.org```, then Root server will return the path to the name server responsible for ```.org```.
- **Host**: when you are given a domain name, you can expose many hosts on the domain within your private network, so for example ```api.mypage.com```, ```ftp.mypage.com``` can lead to different computers.
- **Subdomain: .com is the top level domain, and everything before it is a subdomain (except the first word like www. or api., ...). ubuntu.com - ubuntu is a subdomain of .com.**
	- Each domain can control subdomains under it - it's a hierarchy!
	- Example: You could have a subdomain for the history department of your school:
		- www.history.uni-lj.si
- **THIS IS HOW DNS WORKS: It goes from left to right from most specific to least specific!!!**
- **FQDN**: Fully qualified domain name is like an absolute address in DNS. Domains just like on a filesystem can be given relative to one another, and as such, can be somewhat ambiguous. A FQDN is an absolute name that specifies its location in relation to the absolute root of the domain name system.
	- e.g. ```mail.google.com.``` - the trailing dot is required!
- **Name Server**: Give answers to name queries. Point to other servers and stuff.
- **Zone File**: A zone file is a simple text file that contains the mappings between domain names and IP addresses. This is how the DNS system finally find out which IP address should be contacted when a user requests a certain domain name.
- **Records**: Records are kept within zone files. Can be a mapping from domain name to IP address, define name servers for the domain, define mail servers for the domain, etc.

- **Domain-level name server**: Example that Root server directs to it after a request of ```www.wikipedia.org```. DLNS will find a zone file for ```wikipedia.org```, and within it, a ```www``` host will be registered - it will return the IP.

- **Resolving Name Server**: Basically the client application of DNS. It knows the Root server addresses, and it caches responses to improve speed.
	- ISP provides them or Google, ...


###### What happens when you put an address in a browser

- Computer first looks to see if it can find out locally where the resource is located:
	- checks the ```hosts``` file on the computer
	- if it doesn't find it, then sends the request to the resolving name server and waits back to receive the IP address of the resource.


###### Zone file

