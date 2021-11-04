//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using MongoDB.Bson;
using Mvp24Hours.Infrastructure.Data.MongoDb.Test.Data;
using Mvp24Hours.Infrastructure.Data.MongoDb.Test.Entities;
using Mvp24Hours.Infrastructure.Data.MongoDb.Test.Helpers;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Infrastructure.Data.MongoDb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ServiceQueryTest
    {
        public ServiceQueryTest()
        {
            StartupHelper.ConfigureServices();
        }

        [Fact, Priority(1)]
        public void Create_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

            for (int i = 0; i < 10; i++)
            {
                service.Add(new Customer
                {
                    Oid = ObjectId.GenerateNewId(),
                    Created = DateTime.Now,
                    Name = $"Test {i}",
                    Active = true
                });

            }

            int count = service.GetByCount(x => x.Active);

            Assert.True(count > 0);
        }

        [Fact, Priority(2)]
        public void Get_Filter_Customer_By_Name()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customer = service.GetBy(x => x.Name == "Test 2");
            Assert.True(customer != null);
        }
    }
}
