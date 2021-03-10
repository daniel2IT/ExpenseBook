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

        public static int GetMaxNo(CrmServiceClient service)
        {
            EntityCollection numberCollection = HelperClass.Query(service, "new_expense", "maxNumber");
            
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

        public static EntityCollection Query(CrmServiceClient service, string entityCollection, string exception)
        {
            if (exception.Equals("maxNumber"))
            {
                QueryExpression query = new QueryExpression(entityCollection); // new_employer
                query.ColumnSet.AddColumns("new_no");
                query.Criteria.AddCondition("new_no", ConditionOperator.NotNull);


                return service.RetrieveMultiple(query);
            }
            else
            {
                List<string> QueryCollumns = new List<string>();
                QueryCollumns.Add("new_name");
                QueryCollumns.Add("statuscode");
                if (entityCollection.Equals("new_expense"))
                {
                    QueryCollumns.Add("new_no");
                    QueryCollumns.Add("new_date");
                    QueryCollumns.Add("new_spent");
                    QueryCollumns.Add("new_vat");
                    QueryCollumns.Add("new_total");
                    QueryCollumns.Add("new_comment");
                    QueryCollumns.Add("new_employee");
                }
                if (entityCollection.Equals("new_employee"))
                {
                    QueryCollumns.Add("new_employer");
                    if (!exception.Equals("null"))
                    {
                        QueryCollumns.Add("new_employeeid");
                    }
                }
                if (entityCollection.Equals("new_employer"))
                {
                    QueryCollumns.Add("new_employerid");
                }

                QueryExpression query = new QueryExpression(entityCollection); // new_employer
                query.ColumnSet.AddColumns(QueryCollumns.ToArray());

                if (!exception.Equals("null"))
                {
                    // Using For Post Method
                    query.Criteria.AddCondition("new_name", ConditionOperator.Equal, exception);
                }
                else
                {
                    query.Criteria.AddCondition("new_name", ConditionOperator.NotNull);
                }

                query.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));

                if (entityCollection.Equals("new_expense"))
                {
                    query.Criteria.AddCondition("new_no", ConditionOperator.NotNull);
                    query.Criteria.AddCondition("new_date", ConditionOperator.NotNull);
                    query.Criteria.AddCondition("new_spent", ConditionOperator.NotNull);
                    query.Criteria.AddCondition("new_vat", ConditionOperator.NotNull);
                    query.Criteria.AddCondition("new_total", ConditionOperator.NotNull);
                    query.Criteria.AddCondition("new_comment", ConditionOperator.NotNull);

                    query.Criteria.AddCondition("new_employee", ConditionOperator.NotNull);
                }

                /*   if (entityCollection.Contains("employeeCollection"))
                   {
                       query.Criteria.AddCondition("new_employer", ConditionOperator.NotNull);
                   }*/

                return service.RetrieveMultiple(query);
            }
        }
    }
}