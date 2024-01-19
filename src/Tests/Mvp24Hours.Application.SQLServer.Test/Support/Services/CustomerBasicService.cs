//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Logic;
using Mvp24Hours.Application.SQLServer.Test.Support.Entities.Basics;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.ValueObjects.Logic;
using System.Collections.Generic;
using System.Linq;

namespace Mvp24Hours.Application.SQLServer.Test.Support.Services
{
    public class CustomerBasicService : RepositoryService<CustomerBasic, IUnitOfWork>
    {
        public CustomerBasicService(IUnitOfWork unitOfWork)
            : base(unitOfWork) { }

        // custom methods here

        public IList<CustomerBasic> GetWithContacts()
        {
            var paging = new PagingCriteria(3, 0);

            var customers = Repository.GetBy(x => x.Contacts.Any(), paging);

            foreach (var customer in customers)
            {
                Repository.LoadRelation(customer, x => x.Contacts);
            }
            return customers;
        }

        public IList<CustomerBasic> GetWithPagedContacts()
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
