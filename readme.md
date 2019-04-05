## BiroInvoiceAssistantWebhook

THIS PROJECT WAS MOVED. NOW ALL PROJECTS THAT ARE DEPENDENCIES OF BIROINVOICEASSISTANT ARE IN ONE REPOSITORY. THIS IS KEPT TO PRESERVE THE GIT HISTORY OF THE TESTS PROJECT.

BiroInvoiceAssistant web hook is the endpoint to which processed invoices from the InvoiceAssistant get posted via HTTPS.

### Configuration

Open up the ```appsettings.example.json``` file to see how the configuration should look like. Create an ```appsettings.json``` file in the same folder, add the parameters in the same manner as the example file.

##### Check out some resources for this project

- [big picture plans](./resources/plans.md) - the big picture and the TODOS for it.
- [todos](./resources/planshook.md) - The todos and plans concerning only the webhook project.
- [environments](./resources/environments.md) - How the tools is set up in different environments.

##### Useful theory regarding this project

- [configuring for HTTPS](./resources/httpsconfig.md) - Currently the service is running over http, but eventually, it will have to run over https.
- [On the workings of DNS](./resources/httpsdns.md)
