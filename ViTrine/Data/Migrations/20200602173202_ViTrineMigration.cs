using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ViTrine.Data.Migrations
{
    public partial class ViTrineMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lojas",
                columns: table => new
                {
                    LojaId = table.Column<Guid>(nullable: false),
                    UrlFotoLoja = table.Column<string>(nullable: false),
                    NomeLoja = table.Column<string>(maxLength: 256, nullable: false),
                    CategoriaLoja = table.Column<string>(maxLength: 256, nullable: false),
                    TelefoneLoja = table.Column<string>(nullable: false),
                    CidadeLoja = table.Column<string>(maxLength: 256, nullable: false),
                    EnderecoLoja = table.Column<string>(maxLength: 256, nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lojas", x => x.LojaId);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ChatId = table.Column<Guid>(nullable: false),
                    AutorMensagem = table.Column<string>(maxLength: 256, nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    LojaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ChatId);
                    table.ForeignKey(
                        name: "FK_Chats_Lojas_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Lojas",
                        principalColumn: "LojaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favoritos",
                columns: table => new
                {
                    FavoritoId = table.Column<Guid>(nullable: false),
                    LojaId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favoritos", x => x.FavoritoId);
                    table.ForeignKey(
                        name: "FK_Favoritos_Lojas_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Lojas",
                        principalColumn: "LojaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    ProdutoId = table.Column<Guid>(nullable: false),
                    UrlFotoProduto = table.Column<string>(nullable: false),
                    NomeProduto = table.Column<string>(maxLength: 256, nullable: false),
                    CategoriaProduto = table.Column<string>(maxLength: 256, nullable: false),
                    PromocaoProduto = table.Column<bool>(nullable: false),
                    PrecoProduto = table.Column<double>(nullable: false),
                    DescricaoProduto = table.Column<string>(maxLength: 256, nullable: false),
                    LojaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.ProdutoId);
                    table.ForeignKey(
                        name: "FK_Produtos_Lojas_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Lojas",
                        principalColumn: "LojaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mensagens",
                columns: table => new
                {
                    MensagemId = table.Column<Guid>(nullable: false),
                    TextoMensagem = table.Column<string>(maxLength: 512, nullable: false),
                    DataMensagem = table.Column<DateTime>(nullable: false),
                    ChatId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagens", x => x.MensagemId);
                    table.ForeignKey(
                        name: "FK_Mensagens_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_LojaId",
                table: "Chats",
                column: "LojaId");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_LojaId",
                table: "Favoritos",
                column: "LojaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_ChatId",
                table: "Mensagens",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_LojaId",
                table: "Produtos",
                column: "LojaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favoritos");

            migrationBuilder.DropTable(
                name: "Mensagens");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Lojas");
        }
    }
}
