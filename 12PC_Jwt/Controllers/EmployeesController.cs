using _12PC_Jwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _12PC_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private static readonly List<Employee> _employees = new List<Employee>
        {
            new Employee { FirstName = "Ahmet", LastName = "Yılmaz", Salary = 15000, Department = "IT" },
            new Employee { FirstName = "Ayşe", LastName = "Kara", Salary = 12000, Department = "HR" },
            new Employee { FirstName = "Mehmet", LastName = "Demir", Salary = 18000, Department = "Finance" },
            new Employee { FirstName = "Elif", LastName = "Çelik", Salary = 20000, Department = "Sales" },
            new Employee { FirstName = "Murat", LastName = "Yücedağ", Salary = 25000, Department = "R&D" }
        };

        // GET: api/employees
        [HttpGet]
        [Authorize]
        public IActionResult GetEmployees()
        {
            return Ok(_employees);
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Çalışan bilgisi boş olamaz.");

            _employees.Add(employee);
            return Ok(employee);
        }
    }
}
