using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AWA.Models;

namespace AWA.Migrations
{
    [DbContext(typeof(AgroContext))]
    [Migration("20160817223936_databaseinit")]
    partial class databaseinit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<int?>("TimesheetId");

                    b.HasKey("AttachmentId");

                    b.HasIndex("TimesheetId");

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

            modelBuilder.Entity("AWA.Models.EmployeeAssignment", b =>
                {
                    b.Property<int>("AssignmentId");

                    b.Property<int>("UserId");

                    b.Property<int>("EmployeeAssignmentId");

                    b.Property<bool>("IsVerified");

                    b.Property<int?>("TimesheetId");

                    b.HasKey("AssignmentId", "UserId");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("TimesheetId");

                    b.HasIndex("UserId");

                    b.ToTable("EmployeeAssignments");
                });

            modelBuilder.Entity("AWA.Models.Machine", b =>
                {
                    b.Property<int>("MachineId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.Property<string>("Status");

                    b.Property<string>("Tag");

                    b.Property<int?>("TimesheetId");

                    b.Property<string>("Type");

                    b.HasKey("MachineId");

                    b.HasIndex("TimesheetId");

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

                    b.Property<string>("Description");

                    b.Property<int>("EmployeeAssignmentId");

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.Property<DateTime>("TotalTime");

                    b.Property<string>("WorkType");

                    b.HasKey("TimesheetId");

                    b.ToTable("Timesheets");
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

            modelBuilder.Entity("AWA.Models.Attachment", b =>
                {
                    b.HasOne("AWA.Models.Timesheet")
                        .WithMany("Attachments")
                        .HasForeignKey("TimesheetId");
                });

            modelBuilder.Entity("AWA.Models.EmployeeAssignment", b =>
                {
                    b.HasOne("AWA.Models.Assignment", "Assignment")
                        .WithMany("EmployeeAssignments")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AWA.Models.Timesheet")
                        .WithMany("EmployeeAssignments")
                        .HasForeignKey("TimesheetId");

                    b.HasOne("AWA.Models.User", "User")
                        .WithMany("EmployeeAssignments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AWA.Models.Machine", b =>
                {
                    b.HasOne("AWA.Models.Timesheet")
                        .WithMany("Machines")
                        .HasForeignKey("TimesheetId");
                });
        }
    }
}
