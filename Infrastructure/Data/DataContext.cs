using System;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : IdentityDbContext<AppUser>
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Invoice>()
			.HasMany(x => x.InvoiceItems)
			.WithOne(x => x.Invoice)
			.OnDelete(DeleteBehavior.Cascade);
	}

	// Tables
    public DbSet<Client> Clients { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Estimate> Estimates { get; set; }
    public DbSet<EstimateStatus> EstimateStatuses { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectStatus> ProjectStatuses { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<TimeRecord> TimeRecords { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<WorkspaceUserMapping> WorkspaceUserMappings { get; set; }
}