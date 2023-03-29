using AutoMapper;
using HRM.DAL.Models;
using HRM.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly HrmContext _context;

        public EmployeeController(IMapper mapper, HrmContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<EmployeeVM>(employee);

            return View(viewModel);
        }
        public IActionResult Index()
        {
            var employee = _context.Employees.ToList();
            var viewModel = _mapper.Map<IEnumerable<EmployeeVM>>(employee);
            return View(viewModel);
        }
    }
}
