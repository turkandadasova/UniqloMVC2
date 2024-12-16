using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqloMVC.Migrations
{
    /// <inheritdoc />
    public partial class CreatedComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentProduct");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Comments",
                newName: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "Like",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductId",
                table: "Comments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Products_ProductId",
                table: "Comments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Products_ProductId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ProductId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Comments",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "CommentProduct",
                columns: table => new
                {
                    CommentsId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentProduct", x => new { x.CommentsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_CommentProduct_Comments_CommentsId",
                        column: x => x.CommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentProduct_ProductsId",
                table: "CommentProduct",
                column: "ProductsId");
        }
    }
}
