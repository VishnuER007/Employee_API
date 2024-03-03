using EmployeeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public EmployeeController(IConfiguration configuration, ILogger<EmployeeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]

        public IActionResult GetAll()
        {
            string connectionstring = _configuration.GetConnectionString("DefaultConnection");
            List<Employee> EmployeeList = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAllEmployees", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Designation = reader.GetString(reader.GetOrdinal("Designation")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        MobileNo = reader.GetString(reader.GetOrdinal("MobileNo")),
                        Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                    };
                    EmployeeList.Add(employee);
                }
                connection.Close();
            }
            if (EmployeeList.Count > 0)
            {
                return Ok(EmployeeList);
            }
            else
            {
                return NotFound("Employee Info Not Found");
            }
        }

        [HttpGet("{id}")]

        public IActionResult Get(string id)
        {
            string connectionstring = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetEmployeeById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Employee employee = new Employee
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Designation = reader.GetString(reader.GetOrdinal("Designation")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        MobileNo = reader.GetString(reader.GetOrdinal("MobileNo")),
                        Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),

                    };
                    connection.Close();
                    return Ok(employee);
                }
                else
                {
                    connection.Close();
                    return NotFound("Employee Not Found");
                }
            }
        }

        [HttpPost]

        public IActionResult Create(Employee employee)
        {
            string connectionstring = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("InsertEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@Designation", employee.Designation);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@MobileNo", employee.MobileNo);
                command.Parameters.AddWithValue("@Salary", employee.Salary);

                command.ExecuteNonQuery();
                connection.Close();

                return Ok("Employee Information added successfully");
            }
        }

        [HttpPut("{id}")]

        public IActionResult Update(string id, Employee employee)
        {
            string connectionstring = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UpdateEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("Id", id);
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@Designation", employee.Designation);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@MobileNo", employee.MobileNo);
                command.Parameters.AddWithValue("@Salary", employee.Salary);

                command.ExecuteNonQuery();
                connection.Close();

                return Ok("Employee Information Updated successfully");

            }

        }

        [HttpDelete("{id}")]

        public IActionResult Delete(string id)
        {
            string connectionstring = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok("Employee Information deleted successfully.");
            }

        }

    }
}
