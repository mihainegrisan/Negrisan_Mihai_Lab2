using System;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using GrpcCustomersService;


namespace Negrisan_Mihai_Lab2.Controllers
{
    public class CustomersGrpcController : Controller
    {
        private readonly GrpcChannel channel;

        public CustomersGrpcController()
        {
            channel = GrpcChannel.ForAddress("https://localhost:5001");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            CustomerList cust = client.GetAll(new Empty());
            return View(cust);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var client = new CustomerService.CustomerServiceClient(channel);
                client.Insert(customer);
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var client = new CustomerService.CustomerServiceClient(channel);
            var grpcCustomer = client.Get(new CustomerId() {Id = id.Value});
            client.Update(grpcCustomer);

            var customer = new LibraryModel.Models.Customer()
            {
                CustomerID = grpcCustomer.CustomerId,
                Name = grpcCustomer.Name,
                Address = grpcCustomer.Adress,
                BirthDate = DateTime.Parse(grpcCustomer.Birthdate)
            };

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var client = new CustomerService.CustomerServiceClient(channel);
                customer = client.Get(new CustomerId() {Id = customer.CustomerId});
                client.Update(customer);

                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            if (id == null)
            {
                return BadRequest();
            }

            var grpcCustomer = client.Get(new CustomerId() { Id = id.Value });

            var customer = new LibraryModel.Models.Customer()
            {
                CustomerID = grpcCustomer.CustomerId,
                Name = grpcCustomer.Name,
                Address = grpcCustomer.Adress,
                BirthDate = DateTime.Parse(grpcCustomer.Birthdate)
            };

            return View(customer);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var client = new CustomerService.CustomerServiceClient(channel);

            //Customer customer = client.Get(new CustomerId() {Id = (int) id});
            var customerId = new CustomerId() {Id = (int) id};

            client.Delete(customerId);

            return RedirectToAction(nameof(Index));
        }

    }
}
