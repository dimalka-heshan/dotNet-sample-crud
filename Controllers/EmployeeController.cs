using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.core.IConfigurations;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        // private readonly EmployeeContext _dbContext;

        // public EmployeeController(EmployeeContext dbContext)
        // {
        //     _dbContext = dbContext;
        // }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        // {
        //     if(_dbContext.Employees == null) 
        //     {
        //         return NotFound();
        //     }
        //     return await _dbContext.Employees.ToListAsync();
        // }


        // [HttpGet("{Id}")]
        // public async Task<ActionResult<Employee>> GetEmployeeById(int Id)
        // {
        //     if(_dbContext.Employees == null) 
        //     {
        //         return NotFound();
        //     }

        //     var employee = await _dbContext.Employees.FindAsync(Id);
            
        //     if(employee == null) 
        //     {
        //         return NotFound();
        //     }

        //     return employee;
        // }

        // [HttpPost]
        // public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        // {
        //     _dbContext.Employees.Add(employee);
        //     await _dbContext.SaveChangesAsync();

        //     return CreatedAtAction(nameof(GetEmployeeById), new { Id = employee.Id}, employee );
        // }

        // [HttpPut]
        // public async Task<ActionResult> UpdateEmployee(int Id, Employee employee)
        // {
        //     if(Id != employee.Id)
        //     {
        //         return BadRequest();
        //     }
        //     _dbContext.Entry(employee).State = EntityState.Modified;

        //     try
        //     {
        //         await _dbContext.SaveChangesAsync();
        //     }
        //     catch(DbUpdateConcurrencyException)
        //     {
        //         if(!EmployeeAvailable(Id))
        //         {
        //             return NotFound();
        //         } 
        //         else
        //         {
        //             throw;
        //         }
        //     }
        //     return Ok();
        // }
        
        // private bool EmployeeAvailable(int Id)
        // {
        //     return (_dbContext.Employees?.Any(x => x.Id == Id)).GetValueOrDefault();
        // }

        // [HttpDelete("{Id}")]
        // public async Task<ActionResult> DeleteEmployee(int Id)
        // {
        //     if(_dbContext.Employees == null)
        //     {
        //         return NotFound();
        //     }

        //     var employee = await _dbContext.Employees.FindAsync(Id);
        //     if (employee == null)
        //     {
        //         return NotFound();
        //     }

        //     _dbContext.Employees.Remove(employee);

        //     await _dbContext.SaveChangesAsync();

        //     return Ok();
        // }

        private readonly ILogger<EmployeeController> _logger;

        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(
            ILogger<EmployeeController> logger,
            IUnitOfWork unitOfWork
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee (Employee employee)
        {
            if(ModelState.IsValid)
            {
                employee.Id = Guid.NewGuid();

                await _unitOfWork.Employees.Add(employee);
                await _unitOfWork.CompleteAsync();

                //return CreatedAtAction("GetItem", new Object{employee.Id}, employee);
                return CreatedAtAction("GetItem", new { Id = employee.Id}, employee );
            }
            return new JsonResult("Something went wrong") {StatusCode = 500};
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem (Guid id)
        {
            var employee = await _unitOfWork.Employees.GetById(id);

            if(employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employees = await _unitOfWork.Employees.All();

            return Ok(employees);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, Employee employee)
        {
            if(id != employee.Id)
                return BadRequest();

            await _unitOfWork.Employees.Upsert(employee);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _unitOfWork.Employees.GetById(id);

            if(employee == null)
                return BadRequest();

            await _unitOfWork.Employees.Delete(id);
            await _unitOfWork.CompleteAsync();

            return Ok(employee);
        }
    }
}