﻿using BookStore.Domain.Entities;
using BookStore.Domain.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Implementation
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ShippingDetail> ShippingDetails { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartLine> CartLines { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}
