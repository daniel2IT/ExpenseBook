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
            QueryExpression queryEmployer = new QueryExpression("new_employer");
            queryEmployer.ColumnSet.AddColumns("new_no");
            queryEmployer.Criteria.AddCondition("new_no", ConditionOperator.NotNull);

            return service.RetrieveMultiple(queryEmployer);
        }

        public static EntityCollection GetEntityCollection(CrmServiceClient service, string entityName)
        {
            List<string> QueryCollumns = new List<string>();

            QueryCollumns.Add("new_name");
            QueryCollumns.Add("statuscode");

            switch (entityName)
            {
                case "new_expense":
                    QueryCollumns.Add("new_no");
                    QueryCollumns.Add("new_date");
                    QueryCollumns.Add("new_spent");
                    QueryCollumns.Add("new_vat");
                    QueryCollumns.Add("new_total");
                    QueryCollumns.Add("new_comment");
                    QueryCollumns.Add("new_employee");
                    break;
                case "new_employee":
                    QueryCollumns.Add("new_employer");
                    QueryCollumns.Add("new_employeeid");
                    break;
                case "new_employer":
                    QueryCollumns.Add("new_employerid");
                    break;
            }

            QueryExpression query = new QueryExpression(entityName);
            // fill query
            query.ColumnSet.AddColumns(QueryCollumns.ToArray());

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

    

            public static EntityCollection Query(CrmServiceClient service, string entityCollection, string exception, string valueForUpdate)
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
                    // Using For Put Method
                    if (exception.Equals("new_expenseid"))
                    {
                        QueryCollumns.Add("new_expenseid");
                    }
                }
                if (entityCollection.Equals("new_employee"))
                {
                    QueryCollumns.Add("new_employer");
                    if (!exception.Equals(""))
                    {
                        QueryCollumns.Add("new_employeeid");
                    }
                }
                if (entityCollection.Equals("new_employer"))
                {
                    QueryCollumns.Add("new_employerid");
                }

                QueryExpression query = new QueryExpression(entityCollection);
                query.ColumnSet.AddColumns(QueryCollumns.ToArray());

                // Using For Post/Put Methods -> Conditions
                if (!exception.Equals("") && !exception.Equals("new_expenseid") &&
                    !exception.Equals("new_employeeid") && !exception.Equals("new_employerid")) 
                {
                    query.Criteria.AddCondition("new_name", ConditionOperator.Equal, exception);
                }
                else if(exception.Equals("new_employeeid") && valueForUpdate != "")
                {
                    query.Criteria.AddCondition("new_name", ConditionOperator.Equal, valueForUpdate);
                }
                else if (exception.Equals("new_employerid") && valueForUpdate != "")
                {
                    query.Criteria.AddCondition("new_name", ConditionOperator.Equal, valueForUpdate);
                }
                else if (!exception.Equals("new_expenseid") && valueForUpdate == "")
                {
                    query.Criteria.AddCondition("new_name", ConditionOperator.NotNull);
                }

                query.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));

                if (entityCollection.Equals("new_expense"))
                {
                    if (exception.Equals("new_expenseid")) // Put Method
                    {
                        query.Criteria.AddCondition("new_no", ConditionOperator.Equal, valueForUpdate);
                    }
                    else
                    {
                        query.Criteria.AddCondition("new_no", ConditionOperator.NotNull);
                        query.Criteria.AddCondition("new_date", ConditionOperator.NotNull);
                        query.Criteria.AddCondition("new_spent", ConditionOperator.NotNull);
                        query.Criteria.AddCondition("new_vat", ConditionOperator.NotNull);
                        query.Criteria.AddCondition("new_total", ConditionOperator.NotNull);
                        query.Criteria.AddCondition("new_comment", ConditionOperator.NotNull);

                        query.Criteria.AddCondition("new_employee", ConditionOperator.NotNull);
                    }
                }

                return service.RetrieveMultiple(query);
        }
    }
}