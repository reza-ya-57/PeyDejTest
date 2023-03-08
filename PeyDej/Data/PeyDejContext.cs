using Microsoft.EntityFrameworkCore;
using PeyDej.Models.Bases;

namespace PeyDej.Data;

public partial class PeyDejContext : DbContext
{
    public PeyDejContext(DbContextOptions<PeyDejContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SparePart> SpareParts { get; set; }
    public virtual DbSet<Motor> Motors { get; set; }
}