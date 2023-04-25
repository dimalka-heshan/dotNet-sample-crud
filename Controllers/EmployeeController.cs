using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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


        [HttpGet("{Id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int Id)
        {
            if(_dbContext.Employees == null) 
            {
                return NotFound();
            }

            var employee = await _dbContext.Employees.FindAsync(Id);
            
            if(employee == null) 
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeById), new { Id = employee.Id}, employee );
        }

        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(int Id, Employee employee)
        {
            if(Id != employee.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(employee).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!EmployeeAvailable(Id))
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
        
        private bool EmployeeAvailable(int Id)
        {
            return (_dbContext.Employees?.Any(x => x.Id == Id)).GetValueOrDefault();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteEmployee(int Id)
        {
            if(_dbContext.Employees == null)
            {
                return NotFound();
            }

            var employee = await _dbContext.Employees.FindAsync(Id);
            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Employees.Remove(employee);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

    }
}