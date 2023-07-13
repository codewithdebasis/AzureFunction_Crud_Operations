using System.Data.SqlClient;
using System.Net;
using Function_Crud_Operations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Function_Crud_Operations
{
    public class FnGetEmployees
    {
        private readonly ILogger _logger;

        public FnGetEmployees(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FnGetEmployees>();
        }

        [Function("GetEmployees")]
        public async Task<List<Employee>> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetEmployees")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var _employee = new List<Employee>();
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("DBConnections")))
                {
                    connection.Open();
                    var query = @"Select * from Employees";
                    SqlCommand command = new SqlCommand(query, connection);
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Employee employee = new Employee()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Code = Convert.ToString(reader["Code"]),
                            FullName = Convert.ToString(reader["FullName"]),
                            DateOfBirth = Convert.ToDateTime(reader["DOB"]),
                            Address = Convert.ToString(reader["Address"]),
                            City = Convert.ToString(reader["City"]),
                            State = Convert.ToString(reader["State"]),
                            Country = Convert.ToString(reader["Country"]),
                            PostalCode = Convert.ToString(reader["PostalCode"]),
                            EmailId = Convert.ToString(reader["EmailId"]),
                            PhoneNo = Convert.ToString(reader["PhoneNo"]),
                            JoiningDate = Convert.ToDateTime(reader["JoiningDate"])
                        };
                        _employee.Add(employee);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return _employee;
        }

    }
}
