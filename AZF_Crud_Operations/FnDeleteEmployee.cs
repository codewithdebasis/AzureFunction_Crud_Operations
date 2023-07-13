using System.Data.SqlClient;
using System.Net;
using Function_Crud_Operations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Function_Crud_Operations
{
    public class FnDeleteEmployee
    {
        private readonly ILogger _logger;

        public FnDeleteEmployee(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FnDeleteEmployee>();
        }

        [Function("DeleteEmployeeById")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteEmployee/{id}")] HttpRequestData req, long id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("DBConnections")))
                {
                    connection.Open();
                    var query = @"delete from Employees where Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    var reader = await command.ExecuteNonQueryAsync();                    
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new ObjectResult(e.ToString());
            }

            return new OkObjectResult(HttpStatusCode.OK);
        }
    }
}
