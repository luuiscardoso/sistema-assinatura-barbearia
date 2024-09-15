﻿// <auto-generated />
using System;
using APIAssinaturaBarbearia.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIAssinaturaBarbearia.Migrations
{
    [DbContext(typeof(BdContext))]
    [Migration("20240915161403_AjustandoPrecicaoTime")]
    partial class AjustandoPrecicaoTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APIAssinaturaBarbearia.Models.Assinatura", b =>
                {
                    b.Property<int>("AssinaturaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssinaturaId"));

                    b.Property<DateTime>("Fim")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Inicio")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<TimeSpan>("TempoRestante")
                        .HasColumnType("time(0)");

                    b.HasKey("AssinaturaId");

                    b.ToTable("Assinaturas");
                });

            modelBuilder.Entity("APIAssinaturaBarbearia.Models.Cliente", b =>
                {
                    b.Property<int>("ClienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClienteId"));

                    b.Property<int>("AssinaturaId")
                        .HasColumnType("int");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("ClienteId");

                    b.HasIndex("AssinaturaId")
                        .IsUnique();

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("APIAssinaturaBarbearia.Models.Cliente", b =>
                {
                    b.HasOne("APIAssinaturaBarbearia.Models.Assinatura", "Assinatura")
                        .WithOne("Cliente")
                        .HasForeignKey("APIAssinaturaBarbearia.Models.Cliente", "AssinaturaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assinatura");
                });

            modelBuilder.Entity("APIAssinaturaBarbearia.Models.Assinatura", b =>
                {
                    b.Navigation("Cliente");
                });
#pragma warning restore 612, 618
        }
    }
}
