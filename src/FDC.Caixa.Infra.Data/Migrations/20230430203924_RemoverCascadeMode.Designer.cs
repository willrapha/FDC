﻿// <auto-generated />
using System;
using FDC.Caixa.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FDC.Caixa.Infra.Data.Migrations
{
    [DbContext(typeof(FluxoDeCaixaContext))]
    [Migration("20230430203924_RemoverCascadeMode")]
    partial class RemoverCascadeMode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FDC.Caixa.Domain.Caixas.Entities.FluxoDeCaixa", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<int>("Situacao")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FluxoDeCaixa", (string)null);
                });

            modelBuilder.Entity("FDC.Caixa.Domain.Caixas.Entities.Movimentacao", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("FluxoDeCaixaId")
                        .HasColumnType("bigint");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("FluxoDeCaixaId");

                    b.ToTable("Movimentacao", (string)null);
                });

            modelBuilder.Entity("FDC.Caixa.Domain.Caixas.Entities.Movimentacao", b =>
                {
                    b.HasOne("FDC.Caixa.Domain.Caixas.Entities.FluxoDeCaixa", "FluxoDeCaixa")
                        .WithMany("Movimentacoes")
                        .HasForeignKey("FluxoDeCaixaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FluxoDeCaixa");
                });

            modelBuilder.Entity("FDC.Caixa.Domain.Caixas.Entities.FluxoDeCaixa", b =>
                {
                    b.Navigation("Movimentacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
