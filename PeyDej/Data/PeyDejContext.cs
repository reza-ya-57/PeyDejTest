using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using PeyDej.Models.Bases;
using PeyDej.Models.Bases.Views;
using PeyDej.Models.Inspection;
using PeyDej.Models.Inspection.Views;
using PeyDej.Models.Report;
using PeyDej.Models.Users;

using System.Reflection.Emit;
using PeyDej.Models.File;

namespace PeyDej.Data;

public partial class PeyDejContext : IdentityDbContext<PeyDejUser>
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
    public virtual DbSet<MachineMotor> MachineMotors { get; set; } = null!;
    public virtual DbSet<SparePart> SpareParts { get; set; } = null!;
    public virtual DbSet<SparePartMachine> SparePartMachines { get; set; } = null!;
    public virtual DbSet<SparePartMotor> SparePartMotors { get; set; } = null!;
    public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;
    public virtual DbSet<MotorIS> MotorISs { get; set; } = null!;
    public virtual DbSet<MachineIS> MachineISs { get; set; } = null!;
    public virtual DbSet<MachineLubricationIS> MachineLubrications { get; set; } = null!;
    public virtual DbSet<Person> Persons { get; set; } = null!;
    public virtual DbSet<Categories> VwCategories { get; set; } = null!;
    public virtual DbSet<VwInspectionSubCategoryIS> VwInspectionSubCategoryISs { get; set; } = null!;
    public virtual DbSet<RepairRequest> RepairRequests { get; set; }
    public virtual DbSet<RepairReport> RepairReports { get; set; }
    public virtual DbSet<DailyStatisticsProduction> DailyStatisticsProduction { get; set; }
    //public virtual DbSet<DailyProductionStatistic> ProductionStatistics { get; set; } = null!;
    //public virtual DbSet<DailyStatistic> DailyStatistics { get; set; } = null!;
    public virtual DbSet<LoadingReport> LoadingReports { get; set; } = null!;
    public virtual DbSet<FileInformation> FileInformations { get; set; } = null!;
    public virtual DbSet<MachineIR> MachineIRs { get; set; } = null!;
    public virtual DbSet<MotorIR> MotorIRs { get; set; } = null!;
    public virtual DbSet<RepairUnitAgendumOrder> RepairUnitAgendumOrders { get; set; } = null!;
    public virtual DbSet<RepairUnitAgendumOrderActionType> RepairUnitAgendumOrderActionTypes { get; set; } = null!;
    public virtual DbSet<RepairReportSparePart> RepairReportSpareParts { get; set; } = null!;
    public virtual DbSet<DailyDryProductIR> DailyDryProductIrs { get; set; } = null!;
    public virtual DbSet<DailyWetProductIR> DailyWetProductIrs{ get; set; } = null!;

}