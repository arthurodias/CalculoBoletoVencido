﻿// <auto-generated />
using System;
using Boleto.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Boleto.Infrastructure.Migrations
{
    [DbContext(typeof(BoletoDbContext))]
    [Migration("20240520144257_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("Boleto.Domain.Entities.BoletoEntity", b =>
                {
                    b.Property<string>("BarCode")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FineAmountCalculated")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("InterestAmountCalculated")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OriginalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("TEXT");

                    b.HasKey("BarCode");

                    b.ToTable("Boletos");
                });
#pragma warning restore 612, 618
        }
    }
}
