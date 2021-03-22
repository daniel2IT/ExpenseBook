using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace ExpenseBook
{
    public class HelperClass
    {
        public static CrmServiceClient getCRMService()
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
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };
        }

        public static int GetMaxNo(CrmServiceClient service)
        {
            EntityCollection numberCollection = GetNoCollection(service);

            int maxNo = 0;
            foreach (Entity app in numberCollection.Entities)
            {

                if(maxNo < Convert.ToInt32(app.Attributes["new_no"]))
                {
                    maxNo = Convert.ToInt32(app.Attributes["new_no"]);
                }
            }
                return maxNo;
        }

        public static EntityCollection GetNoCollection(CrmServiceClient service)
        {
            QueryExpression queryEmployer = new QueryExpression("new_expense");
            queryEmployer.ColumnSet.AddColumns("new_no");
            queryEmployer.Criteria.AddCondition("new_no", ConditionOperator.NotNull);

            return service.RetrieveMultiple(queryEmployer);
        }

        public static EntityCollection GetEntityCollection(CrmServiceClient service, string entityName)
        {
            List<string> queryCollumns = new List<string>();

            queryCollumns.Add("new_name");
            queryCollumns.Add("statuscode");

            switch (entityName)
            {
                case "new_expense":
                    queryCollumns.Add("new_no");
                    queryCollumns.Add("new_date");
                    queryCollumns.Add("new_spent");
                    queryCollumns.Add("new_vat");
                    queryCollumns.Add("new_total");
                    queryCollumns.Add("new_comment");
                    queryCollumns.Add("new_employee");
                    queryCollumns.Add("new_expenseid");
                    break;
                case "new_employee":
                    queryCollumns.Add("new_employer");
                    queryCollumns.Add("new_employeeid");
                    break;
                case "new_employer":
                    queryCollumns.Add("new_employerid");
                    break;
            }

            QueryExpression query = new QueryExpression(entityName);
            
            // Fill Data To Query
            query.ColumnSet.AddColumns(queryCollumns.ToArray());

            query.Criteria.AddCondition("new_name", ConditionOperator.NotNull);

            if (entityName.Equals("new_expense"))
            {
                query.Criteria.AddCondition("new_no", ConditionOperator.NotNull);
                query.Criteria.AddCondition("new_date", ConditionOperator.NotNull);
                query.Criteria.AddCondition("new_spent", ConditionOperator.NotNull);
                query.Criteria.AddCondition("new_vat", ConditionOperator.NotNull);
                query.Criteria.AddCondition("new_total", ConditionOperator.NotNull);
                query.Criteria.AddCondition("new_comment", ConditionOperator.NotNull);
                query.Criteria.AddCondition("new_employee", ConditionOperator.NotNull);
            }

            return service.RetrieveMultiple(query);
        }
    }
}