using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;

using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Net;

namespace CrmConnectFuncApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["Query"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

         try
            {
                string clientId = "############";
                string clientSecret = "###########";
                string crmUrl = "https://############.crm.dynamics.com/";
                string tenantId = "###########";
                string authority = "https://login.microsoftonline.com/"+ tenantId; // AAD authority

                // Initialize the connection string
                string connectionString = $@"AuthType=ClientSecret;
                                            Url={crmUrl};
                                            ClientId={clientId};
                                            ClientSecret={clientSecret};
                                            Authority={authority};";

                // Create a new ServiceClient instance
                var serviceClient = new ServiceClient(connectionString);
               
                if (serviceClient.IsReady)
                {
                    Entity newOrder = new Entity("order");
                  
                    // for lookup entity
                    newOrder["deliveryId"] = new EntityReference("deliveryId", Guid.Parse("f1e6bd888ac0e61180ca00155d340529"));
                    // for optionset value
                    newOrder["order_status"] = new OptionSetValue(125080002);
                
                    Guid guid = serviceClient.Create(newOrder);
                    //serviceClient.Update(newIncident);


                    if (guid != Guid.Empty)
                    {
                        log.LogInformation("New Order record created with ID:" + guid);
                    }
                    else
                    {
                        log.LogInformation("Failed to create new Order record.");
                    }
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return new OkObjectResult(responseMessage);
        }
    }
}
