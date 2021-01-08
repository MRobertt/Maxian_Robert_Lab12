using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcCustomersService;
using Microsoft.Extensions.Logging;
using DataAccess = LibraryModel.Data;
using ModelAccess = LibraryModel.Models;


namespace GrpcCustomersService
{
    public class GrpcCrudService : CustomerService.CustomerServiceBase
    {

        private DataAccess.LibraryContext db = null;
        public GrpcCrudService(DataAccess.LibraryContext db)
        {
            this.db = db;
        }
        public override Task<Customer> Get(CustomerId requestData, ServerCallContext context)
        {
            var data = db.Customers.Find(requestData.Id);

            Customer emp = new Customer()
            {
                CustomerId = data.CustomerID,
                Name = data.Name,
                Adress = data.Adress

            };
            return Task.FromResult(emp);
        }

        public override Task<Empty> Delete(CustomerId requestData, ServerCallContext
       context)
        {
            var data = db.Customers.Find(requestData.Id);
            db.Customers.Remove(data);

            db.SaveChanges();
            return Task.FromResult(new Empty());
        }
        public override Task<Customer> Update(Customer requestData, ServerCallContext context)
        {
            db.Customers.Update(new ModelAccess.Customer()
            {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Adress = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });
            db.SaveChanges();
            return Task.FromResult(new Customer());
        }
    }
}
