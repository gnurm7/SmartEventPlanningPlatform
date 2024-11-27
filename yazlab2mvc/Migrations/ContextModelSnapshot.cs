﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using yazlab2mvc.Models;

#nullable disable

namespace yazlab2mvc.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("yazlab2mvc.Models.Etkinlikler", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Aciklama")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EtkinlikAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EtkinlikDurumu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("EtkinlikSuresi")
                        .HasColumnType("time");

                    b.Property<string>("Kategori")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Konum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OlusturanKullaniciID")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Saat")
                        .HasColumnType("time");

                    b.Property<DateTime>("Tarih")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("OlusturanKullaniciID");

                    b.ToTable("Etkinlikler");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Katilimcilar", b =>
                {
                    b.Property<int>("KullaniciID")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("EtkinlikID")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("KullaniciID", "EtkinlikID");

                    b.HasIndex("EtkinlikID");

                    b.ToTable("Katilimcilar");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Kullanicilar", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cinsiyet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DogumTarihi")
                        .HasColumnType("datetime2");

                    b.Property<string>("Eposta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IlgiAlanlari")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Konum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KullaniciAdi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilFotografi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sifre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelefonNumarasi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Kullanicilar");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Mesajlar", b =>
                {
                    b.Property<int>("MesajID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MesajID"));

                    b.Property<int>("AliciID")
                        .HasColumnType("int");

                    b.Property<int>("EtkinlikID")
                        .HasColumnType("int");

                    b.Property<int>("GondericiID")
                        .HasColumnType("int");

                    b.Property<DateTime>("GonderimZamani")
                        .HasColumnType("datetime2");

                    b.Property<string>("MesajMetni")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MesajID");

                    b.HasIndex("AliciID");

                    b.HasIndex("EtkinlikID");

                    b.HasIndex("GondericiID");

                    b.ToTable("Mesajlar");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Puanlar", b =>
                {
                    b.Property<int>("KullaniciID")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<DateTime>("KazanilanTarih")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(1);

                    b.Property<int>("Puan")
                        .HasColumnType("int");

                    b.HasKey("KullaniciID", "KazanilanTarih");

                    b.ToTable("Puanlar");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Etkinlikler", b =>
                {
                    b.HasOne("yazlab2mvc.Models.Kullanicilar", "OlusturanKullanici")
                        .WithMany("OlusturduguEtkinlikler")
                        .HasForeignKey("OlusturanKullaniciID")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("OlusturanKullanici");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Katilimcilar", b =>
                {
                    b.HasOne("yazlab2mvc.Models.Etkinlikler", "Etkinlik")
                        .WithMany("Katilimcilar")
                        .HasForeignKey("EtkinlikID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("yazlab2mvc.Models.Kullanicilar", "Kullanici")
                        .WithMany("KatildigiEtkinlikler")
                        .HasForeignKey("KullaniciID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etkinlik");

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Mesajlar", b =>
                {
                    b.HasOne("yazlab2mvc.Models.Kullanicilar", "Alici")
                        .WithMany("AlinanMesajlar")
                        .HasForeignKey("AliciID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("yazlab2mvc.Models.Etkinlikler", "Etkinlik")
                        .WithMany("Mesajlar")
                        .HasForeignKey("EtkinlikID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("yazlab2mvc.Models.Kullanicilar", "Gonderici")
                        .WithMany("GonderilenMesajlar")
                        .HasForeignKey("GondericiID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Alici");

                    b.Navigation("Etkinlik");

                    b.Navigation("Gonderici");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Puanlar", b =>
                {
                    b.HasOne("yazlab2mvc.Models.Kullanicilar", "Kullanici")
                        .WithMany("Puanlar")
                        .HasForeignKey("KullaniciID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Etkinlikler", b =>
                {
                    b.Navigation("Katilimcilar");

                    b.Navigation("Mesajlar");
                });

            modelBuilder.Entity("yazlab2mvc.Models.Kullanicilar", b =>
                {
                    b.Navigation("AlinanMesajlar");

                    b.Navigation("GonderilenMesajlar");

                    b.Navigation("KatildigiEtkinlikler");

                    b.Navigation("OlusturduguEtkinlikler");

                    b.Navigation("Puanlar");
                });
#pragma warning restore 612, 618
        }
    }
}
