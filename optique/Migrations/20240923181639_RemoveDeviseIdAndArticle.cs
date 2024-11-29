using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace optique.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeviseIdAndArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefDevises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefDevises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefMarques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvecNumSerie = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefMarques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefStatutDistribution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefStatutDistribution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefTypeClient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefTypeClient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefTypeDePaiements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefTypeDePaiements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefTypeRetour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefTypeRetour", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Societes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomSociete = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Societes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Fournisseurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomFournisseur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ICE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pays = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviseId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fournisseurs_RefDevises_DeviseId",
                        column: x => x.DeviseId,
                        principalTable: "RefDevises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomClient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeClientId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_RefTypeClient_TypeClientId",
                        column: x => x.TypeClientId,
                        principalTable: "RefTypeClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Arrivages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateArrivage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocieteId = table.Column<int>(type: "int", nullable: false),
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    NumBL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumFacture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontantFacture = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatutId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arrivages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arrivages_Fournisseurs_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Arrivages_RefStatutDistribution_StatutId",
                        column: x => x.StatutId,
                        principalTable: "RefStatutDistribution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Arrivages_Societes_SocieteId",
                        column: x => x.SocieteId,
                        principalTable: "Societes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarqueId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Couleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    PrixDeVenteInter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrixDeVenteRevendeur = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrixDeVenteClientFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CodeBarre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Fournisseurs_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_RefMarques_MarqueId",
                        column: x => x.MarqueId,
                        principalTable: "RefMarques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_RefTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RefTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Distributions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SocieteId = table.Column<int>(type: "int", nullable: false),
                    DateDistribution = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatutDistributionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Distributions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Distributions_RefStatutDistribution_StatutDistributionId",
                        column: x => x.StatutDistributionId,
                        principalTable: "RefStatutDistribution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Distributions_Societes_SocieteId",
                        column: x => x.SocieteId,
                        principalTable: "Societes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArrivageDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrivageId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    NumSerie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantiteRecuParArticle = table.Column<int>(type: "int", nullable: false),
                    PrixDachatDevise = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrixDachatMAD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TauxRemise = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrixAchatNetDevise = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrixAchatNetMAD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateArrivage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FichierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrivageDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArrivageDetails_Arrivages_ArrivageId",
                        column: x => x.ArrivageId,
                        principalTable: "Arrivages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArrivageDetails_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ventes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDeVente = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrixDeVente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantiteVendu = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientIdAcheteur = table.Column<int>(type: "int", nullable: true),
                    NDeBon = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ventes_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ventes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ventes_Clients_ClientIdAcheteur",
                        column: x => x.ClientIdAcheteur,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DistributionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributionId = table.Column<int>(type: "int", nullable: false),
                    ArrivageDetailsId = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    PrixDeVente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumFacture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributionDetails_ArrivageDetails_ArrivageDetailsId",
                        column: x => x.ArrivageDetailsId,
                        principalTable: "ArrivageDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DistributionDetails_Distributions_DistributionId",
                        column: x => x.DistributionId,
                        principalTable: "Distributions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RetourFournisseurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantiteRetournee = table.Column<int>(type: "int", nullable: false),
                    MotifRetour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateRetour = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeRetourId = table.Column<int>(type: "int", nullable: false),
                    ArrivageDetailsId = table.Column<int>(type: "int", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ArrivageDetailsId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetourFournisseurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetourFournisseurs_ArrivageDetails_ArrivageDetailsId",
                        column: x => x.ArrivageDetailsId,
                        principalTable: "ArrivageDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RetourFournisseurs_ArrivageDetails_ArrivageDetailsId1",
                        column: x => x.ArrivageDetailsId1,
                        principalTable: "ArrivageDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RetourFournisseurs_RefTypeRetour_TypeRetourId",
                        column: x => x.TypeRetourId,
                        principalTable: "RefTypeRetour",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailsPaiements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenteId = table.Column<int>(type: "int", nullable: true),
                    ArrivageId = table.Column<int>(type: "int", nullable: true),
                    NCheque = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NLCN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateEcheance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TypeDePaiementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsPaiements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailsPaiements_Arrivages_ArrivageId",
                        column: x => x.ArrivageId,
                        principalTable: "Arrivages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetailsPaiements_RefTypeDePaiements_TypeDePaiementId",
                        column: x => x.TypeDePaiementId,
                        principalTable: "RefTypeDePaiements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailsPaiements_Ventes_VenteId",
                        column: x => x.VenteId,
                        principalTable: "Ventes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MouvementArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrivageDetailsId = table.Column<int>(type: "int", nullable: true),
                    RetourFournisseurId = table.Column<int>(type: "int", nullable: true),
                    DistributionDetailsId = table.Column<int>(type: "int", nullable: true),
                    VenteId = table.Column<int>(type: "int", nullable: true),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    DateDeCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MouvementArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MouvementArticles_ArrivageDetails_ArrivageDetailsId",
                        column: x => x.ArrivageDetailsId,
                        principalTable: "ArrivageDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MouvementArticles_DistributionDetails_DistributionDetailsId",
                        column: x => x.DistributionDetailsId,
                        principalTable: "DistributionDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MouvementArticles_RetourFournisseurs_RetourFournisseurId",
                        column: x => x.RetourFournisseurId,
                        principalTable: "RetourFournisseurs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MouvementArticles_Ventes_VenteId",
                        column: x => x.VenteId,
                        principalTable: "Ventes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArrivageDetails_ArrivageId",
                table: "ArrivageDetails",
                column: "ArrivageId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivageDetails_ArticleId",
                table: "ArrivageDetails",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Arrivages_FournisseurId",
                table: "Arrivages",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_Arrivages_SocieteId",
                table: "Arrivages",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_Arrivages_StatutId",
                table: "Arrivages",
                column: "StatutId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_FournisseurId",
                table: "Articles",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_MarqueId",
                table: "Articles",
                column: "MarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_TypeId",
                table: "Articles",
                column: "TypeId");

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
                name: "IX_Clients_TypeClientId",
                table: "Clients",
                column: "TypeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsPaiements_ArrivageId",
                table: "DetailsPaiements",
                column: "ArrivageId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsPaiements_TypeDePaiementId",
                table: "DetailsPaiements",
                column: "TypeDePaiementId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsPaiements_VenteId",
                table: "DetailsPaiements",
                column: "VenteId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionDetails_ArrivageDetailsId",
                table: "DistributionDetails",
                column: "ArrivageDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionDetails_DistributionId",
                table: "DistributionDetails",
                column: "DistributionId");

            migrationBuilder.CreateIndex(
                name: "IX_Distributions_ClientId",
                table: "Distributions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Distributions_SocieteId",
                table: "Distributions",
                column: "SocieteId");

            migrationBuilder.CreateIndex(
                name: "IX_Distributions_StatutDistributionId",
                table: "Distributions",
                column: "StatutDistributionId");

            migrationBuilder.CreateIndex(
                name: "IX_Fournisseurs_DeviseId",
                table: "Fournisseurs",
                column: "DeviseId");

            migrationBuilder.CreateIndex(
                name: "IX_MouvementArticles_ArrivageDetailsId",
                table: "MouvementArticles",
                column: "ArrivageDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_MouvementArticles_DistributionDetailsId",
                table: "MouvementArticles",
                column: "DistributionDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_MouvementArticles_RetourFournisseurId",
                table: "MouvementArticles",
                column: "RetourFournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_MouvementArticles_VenteId",
                table: "MouvementArticles",
                column: "VenteId");

            migrationBuilder.CreateIndex(
                name: "IX_RetourFournisseurs_ArrivageDetailsId",
                table: "RetourFournisseurs",
                column: "ArrivageDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_RetourFournisseurs_ArrivageDetailsId1",
                table: "RetourFournisseurs",
                column: "ArrivageDetailsId1",
                unique: true,
                filter: "[ArrivageDetailsId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RetourFournisseurs_TypeRetourId",
                table: "RetourFournisseurs",
                column: "TypeRetourId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventes_ArticleId",
                table: "Ventes",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventes_ClientId",
                table: "Ventes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventes_ClientIdAcheteur",
                table: "Ventes",
                column: "ClientIdAcheteur");
        }

        /// <inheritdoc />
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
                name: "DetailsPaiements");

            migrationBuilder.DropTable(
                name: "MouvementArticles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RefTypeDePaiements");

            migrationBuilder.DropTable(
                name: "DistributionDetails");

            migrationBuilder.DropTable(
                name: "RetourFournisseurs");

            migrationBuilder.DropTable(
                name: "Ventes");

            migrationBuilder.DropTable(
                name: "Distributions");

            migrationBuilder.DropTable(
                name: "ArrivageDetails");

            migrationBuilder.DropTable(
                name: "RefTypeRetour");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Arrivages");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "RefTypeClient");

            migrationBuilder.DropTable(
                name: "RefStatutDistribution");

            migrationBuilder.DropTable(
                name: "Societes");

            migrationBuilder.DropTable(
                name: "Fournisseurs");

            migrationBuilder.DropTable(
                name: "RefMarques");

            migrationBuilder.DropTable(
                name: "RefTypes");

            migrationBuilder.DropTable(
                name: "RefDevises");
        }
    }
}
