﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AWA.Models;

namespace AWA.Migrations
{
    [DbContext(typeof(AgroContext))]
    partial class AgroContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AWA.Models.Assignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<long?>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("Location");

                    b.HasKey("AssignmentId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("AWA.Models.Attachment", b =>
                {
                    b.Property<int>("AttachmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.HasKey("AttachmentId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("AWA.Models.Cargo", b =>
                {
                    b.Property<int>("CargoId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Date");

                    b.Property<string>("Direction");

                    b.Property<int>("EmptyLoad");

                    b.Property<int>("FullLoad");

                    b.Property<int>("NetLoad");

                    b.Property<string>("Type");

                    b.HasKey("CargoId");

                    b.ToTable("Cargos");
                });

            modelBuilder.Entity("AWA.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<bool>("IsArchived");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("AWA.Models.Machine", b =>
                {
                    b.Property<int>("MachineId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.Property<string>("Status");

                    b.Property<string>("Tag");

                    b.Property<string>("Type");

                    b.HasKey("MachineId");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("AWA.Models.RoadPlate", b =>
                {
                    b.Property<int>("RoadplateId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Direction");

                    b.Property<int>("Large");

                    b.Property<int>("Plastic");

                    b.Property<int>("Small");

                    b.HasKey("RoadplateId");

                    b.ToTable("RoadPlates");
                });

            modelBuilder.Entity("AWA.Models.Timesheet", b =>
                {
                    b.Property<int>("TimesheetId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssignmentId");

                    b.Property<bool>("IsVerified");

                    b.Property<int>("UserId");

                    b.HasKey("TimesheetId");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("UserId");

                    b.ToTable("Timesheets");
                });

            modelBuilder.Entity("AWA.Models.TimesheetRecord", b =>
                {
                    b.Property<int>("TimesheetRecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AttachmentId");

                    b.Property<string>("Description");

                    b.Property<long>("EndTime");

                    b.Property<int?>("MachineId");

                    b.Property<long>("StartTime");

                    b.Property<int>("TimesheetId");

                    b.Property<long>("TotalTime");

                    b.Property<string>("WorkType");

                    b.HasKey("TimesheetRecordId");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("MachineId");

                    b.HasIndex("TimesheetId");

                    b.ToTable("TimesheetRecords");
                });

            modelBuilder.Entity("AWA.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsArchived");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PasswordEncrypted");

                    b.Property<int>("Role");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AWA.Models.Assignment", b =>
                {
                    b.HasOne("AWA.Models.Customer", "Customer")
                        .WithMany("Assignments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AWA.Models.Timesheet", b =>
                {
                    b.HasOne("AWA.Models.Assignment", "Assignment")
                        .WithMany("Timesheets")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AWA.Models.User", "User")
                        .WithMany("Timesheets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AWA.Models.TimesheetRecord", b =>
                {
                    b.HasOne("AWA.Models.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId");

                    b.HasOne("AWA.Models.Machine", "Machine")
                        .WithMany()
                        .HasForeignKey("MachineId");

                    b.HasOne("AWA.Models.Timesheet", "Timesheet")
                        .WithMany("Records")
                        .HasForeignKey("TimesheetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
