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

