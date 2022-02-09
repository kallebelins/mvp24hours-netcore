//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Infrastructure.Data.EFCore;
using Mvp24Hours.Patterns.Test.Support.Entities;

namespace Mvp24Hours.Patterns.Test.Support.Data
{
    public class DataContext : Mvp24HoursContext
    {
        #region [ Ctor ]

        public DataContext()
            : base()
        {
        }

        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        #endregion

        #region [ Sets ]

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }

        #endregion
    }
}
