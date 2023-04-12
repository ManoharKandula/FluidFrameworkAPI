using FluidFrameworkDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FluidFrameworkDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _dbContext;

        public EmployeeController(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if(_dbContext.Employees == null)
            {
                return NotFound();
            }
            return await _dbContext.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }
            var employee = await _dbContext.Employees.FindAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id}, employee);
        }

        [HttpPut]
        public async Task<ActionResult<Employee>> PutEmployee (int id, Employee employee)
        {
            if(id != employee.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(employee).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        private bool EmployeeAvailable(int id)
        {
            return (_dbContext.Employees?.Any(x => x.Id == id)).GetValueOrDefault();
        }
    }
}
