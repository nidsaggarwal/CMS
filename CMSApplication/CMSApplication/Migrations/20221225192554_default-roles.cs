﻿using CMSApplication.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMSApplication.Migrations
{
    /// <inheritdoc />
    public partial class defaultroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var role in UserRole.Admin.GetEnumValues())
              
                migrationBuilder.InsertData(
                    table: "AspNetRoles",
                    columns: new[] { "CreatedDate", "Name", "NormalizedName" },
                    values: new object[] { DateTime.Now,  role.ToString(), role.ToString() }
                    );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
