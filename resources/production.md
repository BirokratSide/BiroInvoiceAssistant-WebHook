# Production service

### Plan

The plan is for the webhook to run over HTTPS on the andersen production server and connect to BAzure's tmpTabela table, where it passes on 
the data.

### Current situation

The webhook is currently running on 

birowebhooks.andersen.si/api/invoiceassistant

The procudure bellow is implemented.

##### How to set it up

**Configuration**

- $domain = Ask Bricelj
- $production = hidden
- $endpoint = vmKristijan (will later be a production server)

**Procedure**

- Make sure $domain is set up correctly so that the above url is linking to $production
- $production IIS should have a URL Rewrite -> Reverse Proxy rule to forward ```birowebhook.andersen.si``` to $endpoint.
- $endpoint IIS should have a URL Rewrite -> Reverse Proxy rule to forward port 80 to localhost:5000
- Run the birowebhook application in either debug mode or release mode on $endpoint. By default it will start on port 5000.