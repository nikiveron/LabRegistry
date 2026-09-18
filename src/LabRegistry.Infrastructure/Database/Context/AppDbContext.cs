using LabRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabRegistry.Infrastructure.Database.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<InspectionObject> InspectionObjects { get; set; }
}