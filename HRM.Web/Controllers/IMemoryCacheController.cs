using HRM.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HRM.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IMemoryCacheController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly HrmContext _context;


        public IMemoryCacheController(IMemoryCache memoryCache, HrmContext context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }


      
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            // Check if the data is present in the cache
            if (!_memoryCache.TryGetValue("AllEmployee", out List<Employee> employee))
            {
                // If not, fetch data from the database and add it to the cache
                employee = await _context.Employees.ToListAsync();
                _memoryCache.Set("AllEmployee", employee, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
            }



            return Ok(employee);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            // Check if the data is present in the cache
            if (!_memoryCache.TryGetValue($"Employee-{id}", out Employee employee))
            {
                // If not, fetch data from the database and add it to the cache
                employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                _memoryCache.Set($"Employee-{id}", employee, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
            }



            return Ok(employee);
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            // Add the new student to the database
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();



            // Invalidate the cache
            _memoryCache.Remove("AllEmployee");



            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }



            // Update the student in the database
            _context.Entry(employee).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(s => s.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }



            // Invalidate the cache
            _memoryCache.Remove($"Student-{id}");
            _memoryCache.Remove("AllStudents");



            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            // Remove the student from the database
            var student = await _context.Employees.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(student);
            await _context.SaveChangesAsync();



            // Invalidate the cache
            _memoryCache.Remove($"Student-{id}");
            _memoryCache.Remove("AllStudents");



            return NoContent();
        }

    }
}
