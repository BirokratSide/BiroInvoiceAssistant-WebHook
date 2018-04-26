## Planning

### Architecture

- [First idea](https://www.dropbox.com/s/ceyof25al2lrv1i/IMG_0100.JPG?dl=0) (will not use)

- [Second idea](https://www.dropbox.com/s/k0dlpxoar7u7kex/IMG_0101.JPG?dl=0) (will use)

### TODOS

###### Plan

- [pseudocode](https://www.dropbox.com/s/6xndazthl3p86pk/IMG_0102.JPG?dl=0)

These todos are in chronological order:

- Bricelj dogovor:
	- We will put the dll just as a class inside the project
	- **OnInsertSlika**
		- Do I write the condition or will you?
		- SendInvoice is prepared, but we should have an unique identifier, to track it back to the appropriate database record (KnjigaPoste or Android)
	- **WEBHOOK**
		- This needs to be a https service.
		- Where will it run? Can it be in the same application?
		- We need an SSL certificate from an authority for ```andersen.si``` domain. In the webhook it's already prepared that you just install the certificate in your CertificateStore and it will find it.
		- Need to create a new table:
			- Need the following fields to be able to render the record and save it back to real tables in bazure when you are done:
				- Type: is it KnjigaPoste or Android?
				- ID of slika in BiroDatabase
				- ID of record in KnjigaPoste or Android
				- InvoiceAssistantReturnValue

- Perhaps we can implement everything above here for HTTP, then move on to HTTPS when we move to production for real.
	- This depends on whether there is anything else to do other than just replace the HTTP procedure with HTTPS. Algoritmik may have some other differences as well.
	- Already asked them about these differences - just have to wait for a response!

- StudentApp
	- Tell Pajnic how to make it.
	- Need to tell him where original records are stored.
	- Need to tell him where Slika is stored.
	- Need to tell him where the tmpTabela is.
			

###### Subtasks

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