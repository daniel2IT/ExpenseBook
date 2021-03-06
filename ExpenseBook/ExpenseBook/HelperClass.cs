using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace ExpenseBook
{
    public class HelperClass
    {
        public static CrmServiceClient getCRMServie()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string connectionString = ConfigurationManager.ConnectionStrings["CRMConnectionString"].ConnectionString;

            CrmServiceClient serviceClient = new CrmServiceClient(connectionString);

            if (serviceClient == null)
            {
                throw new InvalidOperationException(serviceClient.LastCrmError);
            }

            return serviceClient;
        }
        public static ExecuteMultipleRequest MultipleRequestSetUp()
        {
            return new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };
        }

    }
}