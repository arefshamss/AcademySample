using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Academy.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Captchas",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SiteKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SecretKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CaptchaType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captchas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaptchaSettings",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaptchaType = table.Column<int>(type: "int", nullable: false),
                    CaptchaSection = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptchaSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseCategories",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ParentId = table.Column<short>(type: "smallint", maxLength: 10, nullable: true),
                    Priority = table.Column<short>(type: "smallint", maxLength: 10, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseCategories_CourseCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CourseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailSmtps",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EamilAddress = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    SmtpAddress = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: true),
                    EnableSSL = table.Column<bool>(type: "bit", nullable: false),
                    EmailSmtpType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSmtps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<short>(type: "smallint", nullable: true),
                    UniqueName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    RoleSection = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Permissions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RoleSection = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsProviders",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SmsProviderType = table.Column<int>(type: "int", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsSettings",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmsProviderType = table.Column<int>(type: "int", nullable: false),
                    SmsSettingSection = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AvatarImageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActiveCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MobileActiveCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EmailActiveCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ActiveCodeExpireTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MobileActiveCodeExpireTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailActiveCodeExpireTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsMobileActive = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    IsBannedFromTicket = table.Column<bool>(type: "bit", nullable: false),
                    IsBannedFromComment = table.Column<bool>(type: "bit", nullable: false),
                    GoogleAuthenticationId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefaultSimpleSmtpId = table.Column<short>(type: "smallint", nullable: true),
                    DefaultMailServerSmtpId = table.Column<short>(type: "smallint", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Copyright = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Favicon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteSettings_EmailSmtps_DefaultMailServerSmtpId",
                        column: x => x.DefaultMailServerSmtpId,
                        principalTable: "EmailSmtps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiteSettings_EmailSmtps_DefaultSimpleSmtpId",
                        column: x => x.DefaultSimpleSmtpId,
                        principalTable: "EmailSmtps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionMappings",
                columns: table => new
                {
                    RoleId = table.Column<short>(type: "smallint", nullable: false),
                    PermissionId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionMappings", x => new { x.PermissionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RolePermissionMappings_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissionMappings_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    TicketStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    TicketPriority = table.Column<byte>(type: "tinyint", nullable: false),
                    TicketSection = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReadBySupporter = table.Column<bool>(type: "bit", nullable: false),
                    ReadByUser = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirthDate = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferralSource = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NationalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BirthCertificateNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInformations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleMappings",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<short>(type: "smallint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoleMappings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReadBySupporter = table.Column<bool>(type: "bit", nullable: false),
                    ReadByUser = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CaptchaSettings",
                columns: new[] { "Id", "CaptchaSection", "CaptchaType", "CreatedDate", "DeletedDate", "IsDeleted" },
                values: new object[,]
                {
                    { (short)1, 0, 1, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { (short)2, 3, 1, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { (short)3, 1, 1, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false },
                    { (short)4, 2, 1, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false }
                });

            migrationBuilder.InsertData(
                table: "Captchas",
                columns: new[] { "Id", "CaptchaType", "CreatedDate", "DeletedDate", "IsActive", "IsDeleted", "SecretKey", "SiteKey", "Title" },
                values: new object[,]
                {
                    { (short)1, 2, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, "o4berc6rqidu8zbd5uqs", "hfz7j1jzqn", "ar captcha" },
                    { (short)2, 0, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, "6LfhYIMqAAAAAMnSYhdAQQIQ0xfoT4jB0M3n4hjt", "6LfhYIMqAAAAANG-u6hSXn78NNaHkh9YC0Dl8A9k", "google recaptcha 2" },
                    { (short)3, 1, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, false, "6LdlYYMqAAAAAOBvpRDN6dYVJyF91VT91LZNi0Rk", "6LdlYYMqAAAAAMZPw2mzADp3pSNynHA2UQ5svTWA", "google recaptcha 3" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "DisplayName", "IsDeleted", "ParentId", "RoleSection", "UniqueName" },
                values: new object[] { (short)1, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "داشبورد ادمین", false, null, 0, "AdminDashboard" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "IsDeleted", "RoleName", "RoleSection" },
                values: new object[,]
                {
                    { (short)1, new DateTime(2025, 8, 6, 18, 11, 11, 521, DateTimeKind.Local).AddTicks(6578), null, false, "مدیر سیستم", 0 },
                    { (short)2, new DateTime(2025, 8, 6, 18, 11, 11, 521, DateTimeKind.Local).AddTicks(7003), null, false, "مدرس", 1 }
                });

            migrationBuilder.InsertData(
                table: "SiteSettings",
                columns: new[] { "Id", "Copyright", "CreatedDate", "DefaultMailServerSmtpId", "DefaultSimpleSmtpId", "DeletedDate", "Favicon", "IsDeleted", "Logo", "SiteName" },
                values: new object[] { (short)1, "تمامی حقوق مادی و معنوی این سایت متعلق به آکادمی می باشد و هرگونه کپی برداری غیرقانونی محسوب خواهد شد", new DateTime(2025, 8, 6, 18, 11, 11, 521, DateTimeKind.Local).AddTicks(4879), null, null, null, "favicon.ico", false, "logo.png", "آکادمی برنامه نویسان" });

            migrationBuilder.InsertData(
                table: "SmsProviders",
                columns: new[] { "Id", "ApiKey", "CreatedDate", "DeletedDate", "IsDeleted", "SmsProviderType", "Title" },
                values: new object[,]
                {
                    { (short)1, "16be6c43-2b7d-462f-8032-96b2472112c3", new DateTime(2025, 8, 6, 18, 11, 11, 520, DateTimeKind.Local).AddTicks(7657), null, false, 0, "کاوه نگار" },
                    { (short)2, "36216de3-e60e-4fe6-9932-87cd79a12d0c", new DateTime(2025, 8, 6, 18, 11, 11, 520, DateTimeKind.Local).AddTicks(8128), null, false, 1, "پارس  گرین" }
                });

            migrationBuilder.InsertData(
                table: "SmsSettings",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "IsDeleted", "SmsProviderType", "SmsSettingSection" },
                values: new object[,]
                {
                    { (short)1, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 0, 0 },
                    { (short)2, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 0, 1 },
                    { (short)3, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 0, 2 },
                    { (short)4, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 0, 3 },
                    { (short)5, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 0, 4 },
                    { (short)6, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 0, 5 }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "DisplayName", "IsDeleted", "ParentId", "RoleSection", "UniqueName" },
                values: new object[] { (short)2, new DateTime(2006, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "پاک کردن cache سایت", false, (short)1, 0, "ClearCache" });

            migrationBuilder.InsertData(
                table: "RolePermissionMappings",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { (short)1, (short)1 },
                    { (short)2, (short)1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_ParentId",
                table: "CourseCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ParentId",
                table: "Permissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionMappings_RoleId",
                table: "RolePermissionMappings",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteSettings_DefaultMailServerSmtpId",
                table: "SiteSettings",
                column: "DefaultMailServerSmtpId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteSettings_DefaultSimpleSmtpId",
                table: "SiteSettings",
                column: "DefaultSimpleSmtpId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_SenderId",
                table: "TicketMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_TicketId",
                table: "TicketMessages",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_UserId",
                table: "UserInformations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMappings_RoleId",
                table: "UserRoleMappings",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMappings_UserId",
                table: "UserRoleMappings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Captchas");

            migrationBuilder.DropTable(
                name: "CaptchaSettings");

            migrationBuilder.DropTable(
                name: "CourseCategories");

            migrationBuilder.DropTable(
                name: "RolePermissionMappings");

            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.DropTable(
                name: "SmsProviders");

            migrationBuilder.DropTable(
                name: "SmsSettings");

            migrationBuilder.DropTable(
                name: "TicketMessages");

            migrationBuilder.DropTable(
                name: "UserInformations");

            migrationBuilder.DropTable(
                name: "UserRoleMappings");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "EmailSmtps");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
