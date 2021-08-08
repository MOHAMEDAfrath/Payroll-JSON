using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
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
            Assert.AreEqual(6, list.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            foreach (var mem in list)
                System.Console.WriteLine(mem.id + " " + mem.first_name + " " + mem.last_name + " " + mem.salary);

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
        //add data to json server
        public void add(JsonObject jsonObject)
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);

        }
        [TestMethod]
        public void TestMethod_ToTest_MultipleInsertion()
        {
            List<JsonObject> list = new List<JsonObject>();
            JsonObject jsonObject = new JsonObject();
            jsonObject.Add("first_name", "Priya");
            jsonObject.Add("last_name", "Devi");
            jsonObject.Add("salary", 23000);
            list.Add(jsonObject);
            JsonObject jsonObject1 = new JsonObject();
            jsonObject1.Add("first_name", "Dhanush");
            jsonObject1.Add("last_name", "Raj");
            jsonObject1.Add("salary", 32000);
            list.Add(jsonObject1);
            foreach (var mem in list)
                add(mem);
            //retrieve to check the count after insertion
            IRestResponse response = GetAllEmployee();
            List<Employee> result = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(9, result.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        //method to update values for given id
        [TestMethod]
        public void Test_Method_To_Update()
        {
            Employee employee = new Employee();
            RestRequest request = new RestRequest("/employees/9", Method.PUT);
            JsonObject json = new JsonObject();
            json.Add("first_name", "Danny");
            json.Add("last_name", "Raj");
            json.Add("salary", 30000);
            request.AddJsonBody(json);
            IRestResponse response = restClient.Execute(request);
            var result = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual(result.salary, 30000);
        }
        //deletes the given employee id
        [TestMethod]
        public void Delete_TestMethod()
        {
            RestRequest request = new RestRequest("/employees/9", Method.DELETE);
            IRestResponse response = restClient.Execute(request);
            //check count after deletion
            IRestResponse response1 = GetAllEmployee();
            List<Employee> result = JsonConvert.DeserializeObject<List<Employee>>(response1.Content);
            Assert.AreEqual(8, result.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
