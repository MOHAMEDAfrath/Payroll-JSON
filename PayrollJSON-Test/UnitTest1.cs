using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace PayrollJSON_Test
{
    [TestClass]
    public class UnitTest1
    {
        //initializing the client
        RestClient restClient;
       [TestInitialize]
       public void SetUp()
        {
            restClient = new RestClient("http://localhost:4000");
        }
        //gets the employee detail from server
        public IRestResponse GetAllEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);
            IRestResponse response = restClient.Execute(request);
            return response;
        }
        //retrieve from json server
        [TestMethod]
        public void TestMethod_ToTest_RetrieveAllData_JSONServer()
        {
            IRestResponse response = GetAllEmployee();
            List<Employee> list = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(6,list.Count);
            Assert.AreEqual(HttpStatusCode.OK,response.StatusCode);
            foreach(var mem in list)
                System.Console.WriteLine(mem.id+" "+mem.first_name+" "+mem.last_name+" "+mem.salary);

        }
        //test the insertion
        [TestMethod]
        public void TestMethod_ToTest_InsertData_JSONServer()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JsonObject jsonObject = new JsonObject();
            jsonObject.Add("first_name", "Ram");
            jsonObject.Add("last_name", "Kumar");
            jsonObject.Add("salary", 25000);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            var result = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Ram", result.first_name);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        }
    }
}
