using Infinite.MVCCore.Taxibooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infinite.MVCCore.Taxibooking.Controllers
{
    public class CustomerController : Controller
    {
        
        public readonly IConfiguration _configuration;
        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async  Task<IActionResult> Index()
        {
            List<CustomerViewModel> customers = new();
            using (var client=new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Customers/GetAllCustomers");
                if(result.IsSuccessStatusCode)
                {
                    customers = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                }
            }
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [Route("Customer/Create")]

        public async Task<IActionResult> Create(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Customers/CreateCustomer", customer);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                CustomerViewModel customer = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Customers/GetCustomerById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        customer = await result.Content.ReadAsAsync<CustomerViewModel>();
                        return View();
                    }
                    //else
                    //{
                    //    //ModelState.AddModelError("", "Customer doesn't exist");
                    //}

                }
            }
            return View();
        }

        [HttpPost]
        [Route("Customer/Edit")]
        public async Task<IActionResult> Edit(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Customers/UpdateCustomer/{customer.CustomerId}", customer);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error, Please try later");
                    }

                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            CustomerViewModel customer = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Customers/GetCustomerById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    customer = await result.Content.ReadAsAsync<CustomerViewModel>();
                    return View(customer);
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");
                }
            }
            return View(customer);
        }



        [HttpPost("Customer/Delete/{id}")]
        

        public async Task<IActionResult> Delete(CustomerViewModel customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Customers/DeleteCustomer/{customer.CustomerId}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");
                }
            }
            return View();

        }

    }
}
