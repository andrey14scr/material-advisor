﻿// <auto-generated />
using System;
using MaterialAdvisor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MaterialAdvisor.Data.Migrations
{
    [DbContext(typeof(MaterialAdvisorContext))]
    [Migration("20240630170503_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AnswerEntityLanguageTextEntity", b =>
                {
                    b.Property<Guid>("AnswersId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TextsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AnswersId", "TextsId");

                    b.HasIndex("TextsId");

                    b.ToTable("AnswerEntityLanguageTextEntity");
                });

            modelBuilder.Entity("AnswerGroupEntityLanguageTextEntity", b =>
                {
                    b.Property<Guid>("AnswerGroupsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TextsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AnswerGroupsId", "TextsId");

                    b.HasIndex("TextsId");

                    b.ToTable("AnswerGroupEntityLanguageTextEntity");
                });

            modelBuilder.Entity("LanguageTextEntityQuestionEntity", b =>
                {
                    b.Property<Guid>("QuestionsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TextsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionsId", "TextsId");

                    b.HasIndex("TextsId");

                    b.ToTable("LanguageTextEntityQuestionEntity");
                });

            modelBuilder.Entity("LanguageTextEntityTopicEntity", b =>
                {
                    b.Property<Guid>("TextsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TopicsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TextsId", "TopicsId");

                    b.HasIndex("TopicsId");

                    b.ToTable("LanguageTextEntityTopicEntity");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.AnswerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnswerGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Number")
                        .HasColumnType("tinyint");

                    b.Property<double>("Points")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AnswerGroupId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.AnswerGroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Number")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("AnswerGroups");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.LanguageEntity", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.LanguageTextEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("LanguageId")
                        .HasColumnType("tinyint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("LanguageTexts");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.QuestionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Number")
                        .HasColumnType("tinyint");

                    b.Property<double>("Points")
                        .HasColumnType("float");

                    b.Property<Guid>("TopicId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Version")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.TopicEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AnswerEntityLanguageTextEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.AnswerEntity", null)
                        .WithMany()
                        .HasForeignKey("AnswersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaterialAdvisor.Data.Entities.LanguageTextEntity", null)
                        .WithMany()
                        .HasForeignKey("TextsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AnswerGroupEntityLanguageTextEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.AnswerGroupEntity", null)
                        .WithMany()
                        .HasForeignKey("AnswerGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaterialAdvisor.Data.Entities.LanguageTextEntity", null)
                        .WithMany()
                        .HasForeignKey("TextsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LanguageTextEntityQuestionEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.QuestionEntity", null)
                        .WithMany()
                        .HasForeignKey("QuestionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaterialAdvisor.Data.Entities.LanguageTextEntity", null)
                        .WithMany()
                        .HasForeignKey("TextsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LanguageTextEntityTopicEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.LanguageTextEntity", null)
                        .WithMany()
                        .HasForeignKey("TextsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MaterialAdvisor.Data.Entities.TopicEntity", null)
                        .WithMany()
                        .HasForeignKey("TopicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.AnswerEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.AnswerGroupEntity", "AnswerGroup")
                        .WithMany("Answers")
                        .HasForeignKey("AnswerGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnswerGroup");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.AnswerGroupEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.QuestionEntity", "Question")
                        .WithMany("AnswerGroups")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.LanguageTextEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.LanguageEntity", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.QuestionEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.TopicEntity", "Topic")
                        .WithMany("Questions")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.TopicEntity", b =>
                {
                    b.HasOne("MaterialAdvisor.Data.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.AnswerGroupEntity", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.QuestionEntity", b =>
                {
                    b.Navigation("AnswerGroups");
                });

            modelBuilder.Entity("MaterialAdvisor.Data.Entities.TopicEntity", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
