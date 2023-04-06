using Microsoft.EntityFrameworkCore;
using PeyDej.Models.Bases;
using PeyDej.Models.Bases.Views;
using PeyDej.Models.Inspection;

namespace PeyDej.Data;

public partial class PeyDejContext : DbContext
{
  public PeyDejContext(DbContextOptions<PeyDejContext> options)
      : base(options)
  {
  }


  public virtual DbSet<Category> Categories { get; set; } = null!;
  public virtual DbSet<InspectionCategory> InspectionCategories { get; set; } = null!;
  public virtual DbSet<InspectionSubCategory> InspectionSubCategories { get; set; } = null!;
  public virtual DbSet<Machine> Machines { get; set; } = null!;
  public virtual DbSet<Motor> Motors { get; set; } = null!;
  public virtual DbSet<SparePart> SpareParts { get; set; } = null!;
  public virtual DbSet<SparePartMachine> SparePartMachines { get; set; } = null!;
  public virtual DbSet<SparePartMotor> SparePartMotors { get; set; } = null!;
  public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;
  public virtual DbSet<MotorIS> MotorISs { get; set; } = null!;
  public virtual DbSet<MachineIS> MachineISs { get; set; } = null!;
  public virtual DbSet<Person> Persons { get; set; } = null!;
  public virtual DbSet<Categories> VwCategories { get; set; } = null!;
}