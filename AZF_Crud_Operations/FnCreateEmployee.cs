using System.Data.SqlClient;
using System.Net;
using System.Web.Http.Results;
using Function_Crud_Operations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Function_Crud_Operations
{
    public class FnCreateEmployee
    {
        private readonly ILogger _logger;

        public FnCreateEmployee(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FnCreateEmployee>();
        }

        [Function("CreateEmployee")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var _input_data = JsonConvert.DeserializeObject<Employee>(requestBody);

            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("DBConnections")))
                {
                    connection.Open();
                    if (!string.IsNullOrEmpty(_input_data.Code))
                    {
                        var query = $"INSERT INTO [dbo].[Employees] ([Code],[FullName],[DOB],[Address],[City],[State],[Country],[PostalCode],[EmailId],[PhoneNo],[JoiningDate]) VALUES (" +
                                                                            $"'{_input_data.Code}'," +
                                                                            $"'{_input_data.FullName}'," +
                                                                            $"'{_input_data.DateOfBirth}'," +
                                                                            $"'{_input_data.Address}'," +
                                                                            $"'{_input_data.City}'," +
                                                                            $"'{_input_data.State}'," +
                                                                            $"'{_input_data.Country}'," +
                                                                            $"'{_input_data.PostalCode}'," +
                                                                            $"'{_input_data.EmailId}'," +
                                                                            $"'{_input_data.PhoneNo}'," +
                                                                            $"'{_input_data.JoiningDate}')";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                    }
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
