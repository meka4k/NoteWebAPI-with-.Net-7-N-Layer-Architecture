using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Note> Notes { get; set; }
	public DbSet<Tag> Tags { get; set; }


	public override int SaveChanges()
	{

		foreach (var item in ChangeTracker.Entries())
		{
			if (item.Entity is Note entityTracker)

				switch (item.State)
				{
					case EntityState.Added:
						entityTracker.CreatedAt = DateTime.Now;
						break;
					case EntityState.Modified:
						Entry(entityTracker).Property(x => x.CreatedAt).IsModified = false;
						break;

				}
		}
		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var item in ChangeTracker.Entries())
		{
			if (item.Entity is Note entityTracker)

				switch (item.State)
				{
					case EntityState.Added:
						entityTracker.CreatedAt = DateTime.Now;
						break;
					case EntityState.Modified:
						Entry(entityTracker).Property(x => x.CreatedAt).IsModified = false;
						break;
				}

		}
		return base.SaveChangesAsync(cancellationToken);
	}




	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<NoteTag>().HasKey(nt => new { nt.NoteId, nt.TagId });

		modelBuilder.Entity<NoteTag>().HasOne(nt => nt.Tag).WithMany(t => t.Notes).HasForeignKey(nt => nt.TagId);
		modelBuilder.Entity<NoteTag>().HasOne(nt => nt.Note).WithMany(t => t.Tags).HasForeignKey(nt => nt.NoteId);
	}
}
