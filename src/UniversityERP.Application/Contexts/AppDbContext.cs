﻿using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Interceptors;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Contexts;

internal class AppDbContext : DbContext
{
    private readonly BaseAuditableInterceptor _auditableInterceptor;

    public AppDbContext(DbContextOptions options, BaseAuditableInterceptor auditableInterceptor) : base(options)
    {
        _auditableInterceptor = auditableInterceptor;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Entity<Faculty>().HasQueryFilter(x => !x.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<User> Users { get; set; }
}
