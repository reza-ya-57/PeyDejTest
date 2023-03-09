using Microsoft.EntityFrameworkCore;
using PeyDej.Models.Bases;

namespace PeyDej.Data;

public partial class PeyDejContext : DbContext
{
    public PeyDejContext(DbContextOptions<PeyDejContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<InspectionCategory> InspectionCategories { get; set; }
    public virtual DbSet<InspectionSubCategory> InspectionSubCategories { get; set; }
    public virtual DbSet<Machine> Machines { get; set; }
    public virtual DbSet<Motor> Motors { get; set; }
    public virtual DbSet<SparePart> SpareParts { get; set; }
    public virtual DbSet<SparePartMachine> SparePartMachines { get; set; }
    public virtual DbSet<SparePartMotor> SparePartMotors { get; set; }
    public virtual DbSet<SubCategory> SubCategories { get; set; }
}