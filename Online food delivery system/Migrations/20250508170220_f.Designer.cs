﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Online_food_delivery_system.Models;

#nullable disable

namespace Online_food_delivery_system.Migrations
{
    [DbContext(typeof(FoodDbContext))]
    [Migration("20250508170220_f")]
    partial class f
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Online_food_delivery_system.Models.Agent", b =>
                {
                    b.Property<int>("AgentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AgentID"));

                    b.Property<string>("AgentContact")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("AgentID");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Delivery", b =>
                {
                    b.Property<int>("DeliveryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeliveryID"));

                    b.Property<int?>("AgentID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EstimatedTimeOfArrival")
                        .HasColumnType("datetime2");

                    b.Property<int?>("OrderID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeliveryID");

                    b.HasIndex("AgentID");

                    b.HasIndex("OrderID")
                        .IsUnique()
                        .HasFilter("[OrderID] IS NOT NULL");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.MenuItem", b =>
                {
                    b.Property<int>("ItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal?>("Price")
                        .IsRequired()
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int?>("RestaurantID")
                        .HasColumnType("int");

                    b.HasKey("ItemID");

                    b.HasIndex("RestaurantID");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderID"));

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int?>("RestaurantID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OrderID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("RestaurantID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.OrderMenuItem", b =>
                {
                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("ItemID")
                        .HasColumnType("int");

                    b.HasKey("OrderID", "ItemID");

                    b.HasIndex("ItemID");

                    b.ToTable("OrderMenuItems");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Payment", b =>
                {
                    b.Property<int>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentID"));

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int?>("DeliveryID")
                        .HasColumnType("int");

                    b.Property<int?>("OrderID")
                        .HasColumnType("int");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PaymentID");

                    b.HasIndex("DeliveryID");

                    b.HasIndex("OrderID")
                        .IsUnique()
                        .HasFilter("[OrderID] IS NOT NULL");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Restaurant", b =>
                {
                    b.Property<int>("RestaurantID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RestaurantID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Availability")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RestaurantContact")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("RestaurantName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("RestaurantID");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Delivery", b =>
                {
                    b.HasOne("Online_food_delivery_system.Models.Agent", "Agent")
                        .WithMany("Deliveries")
                        .HasForeignKey("AgentID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Online_food_delivery_system.Models.Order", "Order")
                        .WithOne("Delivery")
                        .HasForeignKey("Online_food_delivery_system.Models.Delivery", "OrderID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Agent");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.MenuItem", b =>
                {
                    b.HasOne("Online_food_delivery_system.Models.Restaurant", "Restaurant")
                        .WithMany("MenuItems")
                        .HasForeignKey("RestaurantID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Order", b =>
                {
                    b.HasOne("Online_food_delivery_system.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Online_food_delivery_system.Models.Restaurant", "Restaurant")
                        .WithMany("Orders")
                        .HasForeignKey("RestaurantID");

                    b.Navigation("Customer");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.OrderMenuItem", b =>
                {
                    b.HasOne("Online_food_delivery_system.Models.MenuItem", "MenuItem")
                        .WithMany("OrderMenuItems")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Online_food_delivery_system.Models.Order", "Order")
                        .WithMany("OrderMenuItems")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuItem");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Payment", b =>
                {
                    b.HasOne("Online_food_delivery_system.Models.Delivery", "Delivery")
                        .WithMany()
                        .HasForeignKey("DeliveryID");

                    b.HasOne("Online_food_delivery_system.Models.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("Online_food_delivery_system.Models.Payment", "OrderID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Delivery");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Agent", b =>
                {
                    b.Navigation("Deliveries");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.MenuItem", b =>
                {
                    b.Navigation("OrderMenuItems");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Order", b =>
                {
                    b.Navigation("Delivery");

                    b.Navigation("OrderMenuItems");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("Online_food_delivery_system.Models.Restaurant", b =>
                {
                    b.Navigation("MenuItems");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
