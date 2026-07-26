using Application.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EcommerceDbContext _context;

        public UnitOfWork(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
            => await _context.SaveChangesAsync();
    }
}
