using Content.Domain;
using Microsoft.EntityFrameworkCore;

namespace Content.Infrastructure;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {}
    
    public DbSet<TestElement> TestElements => Set<TestElement>();
}