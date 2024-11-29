using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using optique.Models;

namespace optique.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Societe> Societes { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Arrivage> Arrivages { get; set; }
        public DbSet<ArrivageDetails> ArrivageDetails { get; set; }
        public DbSet<Distribution> Distributions { get; set; }
        public DbSet<DistributionDetails> DistributionDetails { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vente> Ventes { get; set; }
        public DbSet<DetailsPaiement> DetailsPaiements { get; set; }
        public DbSet<MouvementArticle> MouvementArticles { get; set; }
        public DbSet<RetourFournisseur> RetourFournisseurs { get; set; }

        public DbSet<RefTypeDePaiement> RefTypeDePaiements { get; set; }

         public DbSet<RefDevise> RefDevises { get; set; }

         public DbSet<RefType> RefTypes { get; set; }

         public DbSet<RefMarque> RefMarques { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This is required to call the base method for Identity configurations

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            // Société - Arrivage
            modelBuilder.Entity<Societe>()
                .HasMany(s => s.Arrivages)
                .WithOne(a => a.Societe)
                .HasForeignKey(a => a.SocieteId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Arrivage>()
    .HasOne(a => a.RefStatutDistribution)
    .WithMany()
    .HasForeignKey(a => a.StatutId)
    .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Article>()
        .HasOne(a => a.Marque)  // Utilise RefMarque comme entité de navigation
        .WithMany(m => m.Articles)
        .HasForeignKey(a => a.MarqueId);    

            // Fournisseur - Arrivage
            modelBuilder.Entity<Fournisseur>()
                .HasMany(f => f.Arrivages)
                .WithOne(a => a.Fournisseur)
                .HasForeignKey(a => a.FournisseurId)
                .OnDelete(DeleteBehavior.NoAction);

            // Arrivage - ArrivageDetails
            modelBuilder.Entity<Arrivage>()
                .HasMany(a => a.ArrivageDetails)
                .WithOne(ad => ad.Arrivage)
                .HasForeignKey(ad => ad.ArrivageId)
                .OnDelete(DeleteBehavior.NoAction);

           

            // ArrivageDetails - Article
            modelBuilder.Entity<ArrivageDetails>()
                .HasOne(ad => ad.Article)
                .WithMany(a => a.ArrivageDetails)
                .HasForeignKey(ad => ad.ArticleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Distribution - DistributionDetails
            modelBuilder.Entity<Distribution>()
                .HasMany(d => d.DistributionDetails)
                .WithOne(dd => dd.Distribution)
                .HasForeignKey(dd => dd.DistributionId)
                .OnDelete(DeleteBehavior.NoAction);

            // DistributionDetails - ArrivageDetails
            modelBuilder.Entity<DistributionDetails>()
                .HasOne(dd => dd.ArrivageDetails)
                .WithMany(ad => ad.DistributionDetails)
                .HasForeignKey(dd => dd.ArrivageDetailsId)
                .OnDelete(DeleteBehavior.NoAction);

            // Client - Distribution
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Distributions)
                .WithOne(d => d.Client)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            // Client - Vente
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Ventes)
                .WithOne(v => v.Client)
                .HasForeignKey(v => v.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            // Vente - DetailsPaiement
            modelBuilder.Entity<Vente>()
                .HasMany(v => v.DetailsPaiements)
                .WithOne(dp => dp.Vente)
                .HasForeignKey(dp => dp.VenteId)
                .OnDelete(DeleteBehavior.NoAction);

            

            // MouvementArticle - DistributionDetails
            modelBuilder.Entity<MouvementArticle>()
                .HasOne(ma => ma.DistributionDetails)
                .WithMany(dd => dd.MouvementArticles)
                .HasForeignKey(ma => ma.DistributionDetailsId)
                .OnDelete(DeleteBehavior.NoAction);


            // MouvementArticle - ArrivageDetails
            modelBuilder.Entity<MouvementArticle>()
                .HasOne(ma => ma.ArrivageDetails)
                .WithMany(ad => ad.MouvementArticles)
                .HasForeignKey(ma => ma.ArrivageDetailsId)
                .OnDelete(DeleteBehavior.NoAction);

            // MouvementArticle - RetourFournisseur
            modelBuilder.Entity<MouvementArticle>()
                .HasOne(ma => ma.RetourFournisseur)
                .WithMany(rf => rf.MouvementArticles)
                .HasForeignKey(ma => ma.RetourFournisseurId)
                .OnDelete(DeleteBehavior.NoAction);   

            // MouvementArticle - Vente
            modelBuilder.Entity<MouvementArticle>()
                .HasOne(ma => ma.Vente)
                .WithMany(v => v.MouvementArticles)
                .HasForeignKey(ma => ma.VenteId)
                .OnDelete(DeleteBehavior.NoAction);     

            // relation entre RetourFournisseur et ArrivageDetails
            modelBuilder.Entity<RetourFournisseur>()
                .HasOne(rf => rf.ArrivageDetails)
                .WithMany(ad => ad.RetourFournisseurs)
                .HasForeignKey(rf => rf.ArrivageDetailsId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ArrivageDetails>()
            .HasMany(ad => ad.RetourFournisseurs)
            .WithOne(rf => rf.ArrivageDetails)
            .HasForeignKey(rf => rf.ArrivageDetailsId);  



       

            // Configure decimal properties
            modelBuilder.Entity<Arrivage>()
                .Property(a => a.MontantFacture)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ArrivageDetails>()
                .Property(ad => ad.PrixAchatNetDevise)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ArrivageDetails>()
                .Property(ad => ad.PrixAchatNetMAD)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ArrivageDetails>()
                .Property(ad => ad.PrixDachatDevise)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ArrivageDetails>()
                .Property(ad => ad.PrixDachatMAD)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ArrivageDetails>()
                .Property(ad => ad.TauxRemise)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Article>()
                .Property(a => a.PrixDeVenteClientFinal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Article>()
                .Property(a => a.PrixDeVenteInter)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Article>()
                .Property(a => a.PrixDeVenteRevendeur)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DetailsPaiement>()
                .Property(dp => dp.Montant)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DistributionDetails>()
                .Property(dd => dd.PrixDeVente)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Vente>()
                .Property(v => v.PrixDeVente)
                .HasColumnType("decimal(18,2)");

                // Ignorer les migrations pour RefMarques et RefTypes si les tables existent déjà
    


modelBuilder.Entity<Article>()
    .HasOne(a => a.Type)
    .WithMany(t => t.Articles)
    .HasForeignKey(a => a.TypeId);

modelBuilder.Entity<Article>()
    .HasOne(a => a.Fournisseur)
    .WithMany(f => f.Articles)
    .HasForeignKey(a => a.FournisseurId);

        }
    }
}





