﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelegramBot.Data.Engine;

#nullable disable

namespace TelegramBot.Data.Engine.Migrations.Schema
{
    [DbContext(typeof(DataContext))]
    [Migration("20241002120128_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("tbot")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TelegramBot.Domain.Accounting.Roles.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("normalized_name");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", "tbot");
                });

            modelBuilder.Entity("TelegramBot.Domain.Accounting.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("id");

                    b.Property<bool>("AutoDetectLanguage")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("auto_detect_language");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Language")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("language");

                    b.Property<string>("TelegramUserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("telegram_user_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("TelegramUserId")
                        .IsUnique()
                        .HasDatabaseName("ix_users_telegram_user_id");

                    b.ToTable("users", "tbot");
                });

            modelBuilder.Entity("TelegramBot.Domain.Analytics.UserCommand", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("id");

                    b.Property<string>("CommandName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("command_name");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_commands");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("ix_user_commands_created_at");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_commands_user_id");

                    b.ToTable("user_commands", "tbot");
                });

            modelBuilder.Entity("TelegramBot.Domain.Billing.Invoices.Invoice", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("id");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("chat_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("description");

                    b.Property<string>("MessageId")
                        .HasColumnType("text")
                        .HasColumnName("message_id");

                    b.Property<int>("Price")
                        .HasColumnType("integer")
                        .HasColumnName("price");

                    b.Property<string>("StartParameter")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("start_parameter");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string>("TelegramPaymentChargeId")
                        .HasColumnType("text")
                        .HasColumnName("telegram_payment_charge_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("title");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserId")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_invoices");

                    b.HasIndex("ChatId")
                        .HasDatabaseName("ix_invoices_chat_id");

                    b.HasIndex("Type")
                        .HasDatabaseName("ix_invoices_type");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_invoices_user_id");

                    b.ToTable("invoices", "tbot");
                });

            modelBuilder.Entity("user_role", b =>
                {
                    b.Property<string>("RolesId")
                        .HasColumnType("character varying(255)")
                        .HasColumnName("roles_id");

                    b.Property<string>("UsersId")
                        .HasColumnType("character varying(255)")
                        .HasColumnName("users_id");

                    b.HasKey("RolesId", "UsersId")
                        .HasName("pk_user_role");

                    b.HasIndex("UsersId")
                        .HasDatabaseName("ix_user_role_users_id");

                    b.ToTable("user_role", "tbot");
                });

            modelBuilder.Entity("TelegramBot.Domain.Analytics.UserCommand", b =>
                {
                    b.HasOne("TelegramBot.Domain.Accounting.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_user_commands_users_user_id");
                });

            modelBuilder.Entity("TelegramBot.Domain.Billing.Invoices.Invoice", b =>
                {
                    b.HasOne("TelegramBot.Domain.Accounting.Users.User", "User")
                        .WithMany("Invoices")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_invoices_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("user_role", b =>
                {
                    b.HasOne("TelegramBot.Domain.Accounting.Roles.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_role_roles_roles_id");

                    b.HasOne("TelegramBot.Domain.Accounting.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_role_users_users_id");
                });

            modelBuilder.Entity("TelegramBot.Domain.Accounting.Users.User", b =>
                {
                    b.Navigation("Invoices");
                });
#pragma warning restore 612, 618
        }
    }
}
