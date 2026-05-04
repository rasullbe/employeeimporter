using EmployeeImporter.Data;
using EmployeeImporter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EmployeeImporter.Controllers
{
    // <summary>
    // Handles the main pagee that r displaying employees and importing from CSV
    // </summary>
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // <summary>
        // Injects the database context via dependency injection
        // </summary>
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // <summary>
        // GET: Displays all employees sorted by surname
        // </summary>
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .OrderBy(e => e.Surname)
                .ToListAsync();
            return View(employees);
        }

        // <summary>
        // POST: Parses the uploaded CSV file and inserts valid rows into the database
        // Reports how many rows were successfully processed
        //  </summary>
        // <param name="file">The CSV file uploaded by the user</param>
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file.";
                return View("Index", await _context.Employees.OrderBy(e => e.Surname).ToListAsync());
            }

            int successCount = 0;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                // Skip the header line
                await reader.ReadLineAsync();

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var columns = line.Split(',');

                    // Skip rows with insufficient columns
                    if (columns.Length < 11) continue;

                    try
                    {
                        var employee = new Employee
                        {
                            PayrollNumber = columns[0].Trim(),
                            Forenames = columns[1].Trim(),
                            Surname = columns[2].Trim(),
                            DateOfBirth = DateTime.Parse(columns[3].Trim(), new CultureInfo("en-GB")),
                            Telephone = columns[4].Trim(),
                            Mobile = columns[5].Trim(),
                            Address = columns[6].Trim(),
                            Address2 = columns[7].Trim(),
                            Postcode = columns[8].Trim(),
                            EmailHome = columns[9].Trim(),
                            StartDate = DateTime.Parse(columns[10].Trim(), new CultureInfo("en-GB"))
                        };

                        _context.Employees.Add(employee);
                        successCount++;
                    }
                    catch
                    {
                        // Skip rows that fail to parse
                        continue;
                    }
                }
            }

            await _context.SaveChangesAsync();

            ViewBag.Message = $"Successfully imported {successCount} rows.";
            return View("Index", await _context.Employees.OrderBy(e => e.Surname).ToListAsync());
        }
    }
}