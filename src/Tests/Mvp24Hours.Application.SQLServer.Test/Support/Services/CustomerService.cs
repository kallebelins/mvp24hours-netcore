//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Logic;
using Mvp24Hours.Application.SQLServer.Test.Support.Entities;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Application.SQLServer.Test.Support.Services
{
    public class CustomerService : RepositoryService<Customer, IUnitOfWork>
    {
        // custom methods here

        public IList<Customer> GetWithContacts()
        {
            var paging = new PagingCriteria(3, 0);

            var customers = Repository.GetBy(x => x.Contacts.Any(), paging);

            foreach (var customer in customers)
            {
                Repository.LoadRelation(customer, x => x.Contacts);
            }
            return customers;
        }

        public IList<Customer> GetWithPagedContacts()
        {
            var paging = new PagingCriteria(3, 0);

            var customers = Repository.GetBy(x => x.Contacts.Any(), paging);

            foreach (var customer in customers)
            {
                Repository.LoadRelation(customer, x => x.Contacts, clause: c => c.Active, limit: 1);
            }
            return customers;
        }
    }
}
