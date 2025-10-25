using Leads.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Leads.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
    public DbSet<Lead> Leads => Set<Lead>();
}
