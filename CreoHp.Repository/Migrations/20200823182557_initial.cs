﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CreoHp.Repository.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 256, nullable: true),
                    LastName = table.Column<string>(maxLength: 256, nullable: true),
                    IsBlocked = table.Column<bool>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BadCollectionUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadCollectionUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phrases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedByUserId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phrases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phrases_AspNetUsers_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedByUserId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_AspNetUsers_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhraseCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedByUserId = table.Column<Guid>(nullable: true),
                    PhraseId = table.Column<Guid>(nullable: false),
                    OriginalText = table.Column<string>(maxLength: 450, nullable: false),
                    SearchCount = table.Column<int>(nullable: false),
                    CanSearch = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhraseCollections_AspNetUsers_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhraseCollections_Phrases_PhraseId",
                        column: x => x.PhraseId,
                        principalTable: "Phrases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhraseTags",
                columns: table => new
                {
                    PhraseId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseTags", x => new { x.PhraseId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PhraseTags_Phrases_PhraseId",
                        column: x => x.PhraseId,
                        principalTable: "Phrases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhraseTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagRelations",
                columns: table => new
                {
                    ParentTagId = table.Column<Guid>(nullable: false),
                    ChildTagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagRelations", x => new { x.ChildTagId, x.ParentTagId });
                    table.ForeignKey(
                        name: "FK_TagRelations_Tags_ChildTagId",
                        column: x => x.ChildTagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TagRelations_Tags_ParentTagId",
                        column: x => x.ParentTagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "ModifiedByUserId", "Name", "Position", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("ab3a769a-e800-4e48-97b5-a577baf5ac86"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(2817), false, null, "Neutral", 0, 0, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(2817) },
                    { new Guid("778be6b6-9233-454b-9511-9afef5dd9b53"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5041), false, null, "Info", 0, 2, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5041) },
                    { new Guid("7ec86883-c05d-4e55-92bb-58e7931d92af"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5036), false, null, "Shopping", 12, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5036) },
                    { new Guid("5af6ac0b-d645-4a2a-9943-08599a63573e"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5028), false, null, "Intertainment", 11, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5028) },
                    { new Guid("134e8818-a5c8-49ea-b279-29fca5ff6ce9"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5023), false, null, "Travels", 10, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5023) },
                    { new Guid("82415242-c3b9-45b8-b854-92dcdb5e2cfb"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5017), false, null, "Creation", 9, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5017) },
                    { new Guid("48f1d60c-b834-415e-bae1-4b2c867ea93d"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5012), false, null, "Money", 8, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5012) },
                    { new Guid("454fc2a6-7c0b-4a89-840c-d9378da0c77d"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5007), false, null, "Сareer", 7, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5007) },
                    { new Guid("0bdcfdbd-2b7e-480b-98e0-93c2fc61b458"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5047), false, null, "Tip", 1, 2, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5047) },
                    { new Guid("32eb46b5-6d6f-4c41-8495-47ad37e4b28d"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5001), false, null, "Study", 6, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5001) },
                    { new Guid("66956a46-cfa5-4c4f-af55-718d9d97d850"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4989), false, null, "Love", 4, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4989) },
                    { new Guid("ddac2fa4-964e-475f-b1ff-31fa9105fbd7"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4971), false, null, "Children", 3, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4971) },
                    { new Guid("92fd3c2d-32e1-4e99-a17c-1e1814ae538e"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4965), false, null, "Family", 2, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4965) },
                    { new Guid("b472de0a-951a-4518-abbe-0528043e53a4"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4960), false, null, "Health", 1, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4960) },
                    { new Guid("cca290de-81f5-482d-b27c-c5aea33b0886"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4954), false, null, "Common", 0, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4954) },
                    { new Guid("a21869d5-c093-48cb-9ce0-c80d2e08d5bd"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4947), false, null, "Negative", 2, 0, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4947) },
                    { new Guid("c1f30ee9-ec04-448c-a5cc-3fe22c64e52a"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4922), false, null, "Positive", 1, 0, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4922) },
                    { new Guid("f07f9df2-fcb0-4027-bfc8-67438342458f"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4996), false, null, "Friendship", 5, 1, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(4996) },
                    { new Guid("b8d72746-f8be-40f6-b905-23d9e2d46741"), new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5052), false, null, "Warning", 2, 2, new DateTime(2020, 8, 23, 18, 25, 57, 604, DateTimeKind.Utc).AddTicks(5052) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PhraseCollections_ModifiedByUserId",
                table: "PhraseCollections",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhraseCollections_OriginalText",
                table: "PhraseCollections",
                column: "OriginalText",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhraseCollections_PhraseId",
                table: "PhraseCollections",
                column: "PhraseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phrases_ModifiedByUserId",
                table: "Phrases",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Phrases_Text",
                table: "Phrases",
                column: "Text",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhraseTags_TagId",
                table: "PhraseTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TagRelations_ParentTagId",
                table: "TagRelations",
                column: "ParentTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ModifiedByUserId",
                table: "Tags",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name_Type",
                table: "Tags",
                columns: new[] { "Name", "Type" },
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BadCollectionUrls");

            migrationBuilder.DropTable(
                name: "PhraseCollections");

            migrationBuilder.DropTable(
                name: "PhraseTags");

            migrationBuilder.DropTable(
                name: "TagRelations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Phrases");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
