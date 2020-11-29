using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Basket.Repository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        void SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
