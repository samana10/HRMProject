using AutoMapper;
using HRM.DAL.Models;
using HRM.Web.Models;
using HRM.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text.Json;

namespace HRM.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly HrmContext _context;
        private readonly IConnectionMultiplexer _redis;

        public EmployeeController(IMapper mapper, HrmContext context, IConnectionMultiplexer redis)
        {
            _mapper = mapper;
            _context = context;
            _redis = redis;
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
        public async Task<IActionResult> Index()
        {
            var db = _redis.GetDatabase();
            var timer = Stopwatch.StartNew();

            //check if we've already seen that username recently
            var cache = await db.StringGetAsync($"repos:employee");

            if (string.IsNullOrEmpty(cache))
            {
                var employee = _context.Employees.ToList();
                var viewModel = _mapper.Map<IEnumerable<EmployeeVM>>(employee);
                var data = new ResponseModel { Repos = "employee", Employees = viewModel, Cached = true };
                await db.StringSetAsync($"repos:employee", JsonSerializer.Serialize(data), expiry: TimeSpan.FromSeconds(60));
                data.Cached = false;
                cache = JsonSerializer.Serialize(data);
            }
            var obj = JsonSerializer.Deserialize(cache, typeof(ResponseModel));
            ResponseModel _data = obj as ResponseModel;
            var viewEmployeeModel = _mapper.Map<IEnumerable<EmployeeVM>>(_data?.Employees);
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            return View(viewEmployeeModel);
        }
    }
}
