using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using DataAccess = LibraryModel.Data;
using ModelAccess = LibraryModel.Models;

namespace GrpcCustomersService.Services
{
    public class GrpcCrudService : CustomerService.CustomerServiceBase
    {
        private DataAccess.LibraryContext db = null;

        public GrpcCrudService(DataAccess.LibraryContext db)
        {
            this.db = db;
        }

        public override Task<CustomerList> GetAll(Empty empty, ServerCallContext context)
        {
            CustomerList pl = new CustomerList();
            var query = from cust in db.Customers
                select new Customer()
                {
                    CustomerId = cust.CustomerID,
                    Name = cust.Name,
                    Adress = cust.Address
                };

            pl.Item.AddRange(query.ToArray());

            return Task.FromResult(pl);
        }

        public override Task<Customer> Get(CustomerId customerId, ServerCallContext context)
        {
            var customer = db.Customers.Find(customerId.Id);
            var grpcCustomer = new Customer
            {
                CustomerId = customer.CustomerID,
                Name = customer.Name,
                Adress = customer.Address,
                Birthdate = customer.BirthDate.ToString(CultureInfo.CurrentCulture),
            };

            return Task.FromResult(grpcCustomer);
        }

        public override Task<Empty> Insert(Customer requestData, ServerCallContext context)
        {
            db.Customers.Add(new ModelAccess.Customer
            {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Address = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });

            db.SaveChanges();

            return Task.FromResult(new Empty());
        }

        public override Task<Customer> Update(Customer requestData, ServerCallContext context)
        {
            db.Customers.Update(new ModelAccess.Customer
            {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Address = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });

            db.SaveChanges();

            return Task.FromResult(requestData);
        }

        public override Task<Empty> Delete(CustomerId customerId, ServerCallContext context)
        {
            //var customer = db.Customers.FirstOrDefault(c => c.CustomerID == customerId.Id);
            var customer = db.Customers.Find(customerId.Id);

            if (customer != null)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }

            return Task.FromResult(new Empty());
        }
    }
}
