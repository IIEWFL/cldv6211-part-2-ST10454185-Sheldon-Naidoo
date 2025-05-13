using EventEaseManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEaseManagementSystem.Models;
using System.Threading.Tasks;

namespace EventEaseManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly EventEaseDBContext DBContext;

        public CustomerController(EventEaseDBContext dbContext)
        {
            this.DBContext = dbContext;
        }

        // Read all of the customers in a list form

        // GET: Customers List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Customers = await DBContext.Customers.ToListAsync();
            return View(Customers);
        }

        // Get the details of a specific customer
        // GET: Customer details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var details = await DBContext.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);

            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

        // Create a new customer
        // GET: Create customer form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create a new customer
        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel viewModel)
        {
            var customer = new Customer
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                ProfilePicture = viewModel.ProfilePicture,
                Email = viewModel.Email,
                PhoneNumber = viewModel.PhoneNumber
            };
            await DBContext.Customers.AddAsync(customer);
            await DBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Update an existing customer
        // GET: Update customer form
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await DBContext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        
        // POST: Update an existing customers details
        [HttpPost]
        public async Task<IActionResult> Edit(Customer viewModel)
        {
            var customer = await DBContext.Customers.FindAsync(viewModel.CustomerId);
            if (customer is not null)
            {
                customer.FirstName = viewModel.FirstName;
                customer.LastName = viewModel.LastName;
                customer.ProfilePicture = viewModel.ProfilePicture;
                customer.Email = viewModel.Email;
                customer.PhoneNumber = viewModel.PhoneNumber;

                await DBContext.SaveChangesAsync();

            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        

        // Delete an existing customer
        // GET: Delete customer form
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await DBContext.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        

        // POST: Delete an existing customer
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await DBContext.Customers.FindAsync(id);
            DBContext.Customers.Remove(customer);
            await DBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        

    }
}
