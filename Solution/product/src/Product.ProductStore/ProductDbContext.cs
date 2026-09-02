using Microsoft.EntityFrameworkCore;
using Product.Domain;

namespace Product.ProductStore;

public sealed class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Market> Markets => Set<Market>();
    public DbSet<ProductFamily> ProductFamilies => Set<ProductFamily>();
    public DbSet<Domain.Product> Products => Set<Domain.Product>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ProductTag> ProductTags => Set<ProductTag>();
    public DbSet<ProductMarket> ProductMarkets => Set<ProductMarket>();
    public DbSet<ProductItem> ProductItems => Set<ProductItem>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AssetMarket> AssetMarkets => Set<AssetMarket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("language");
            entity.HasKey(x => x.Code);
            entity.Property(x => x.Code).HasColumnName("code");
            entity.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
        });

        modelBuilder.Entity<Market>(entity =>
        {
            entity.ToTable("market");
            entity.HasKey(x => x.Code);
            entity.Property(x => x.Code).HasColumnName("code");
            entity.Property(x => x.Kind).HasColumnName("kind").IsRequired();
            entity.Property(x => x.Name).HasColumnName("name").IsRequired();
        });

        modelBuilder.Entity<ProductFamily>(entity =>
        {
            entity.ToTable("product_family");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Code).HasColumnName("code").IsRequired();
            entity.Property(x => x.Name).HasColumnName("name").IsRequired();
            entity.Property(x => x.Summary).HasColumnName("summary");
            entity.Property(x => x.Sequence).HasColumnName("sequence").IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Domain.Product>(entity =>
        {
            entity.ToTable("product");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.FamilyId).HasColumnName("family_id").IsRequired();
            entity.Property(x => x.Code).HasColumnName("code").IsRequired();
            entity.Property(x => x.Title).HasColumnName("title").IsRequired();
            entity.Property(x => x.Summary).HasColumnName("summary");
            entity.Property(x => x.Description).HasColumnName("description");
            entity.Property(x => x.ContentLanguage).HasColumnName("content_language").IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
            entity.HasOne<ProductFamily>()
                .WithMany()
                .HasForeignKey(x => x.FamilyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Language>()
                .WithMany()
                .HasForeignKey(x => x.ContentLanguage)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("tag");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Category).HasColumnName("category").IsRequired();
            entity.Property(x => x.Code).HasColumnName("code").IsRequired();
            entity.Property(x => x.Name).HasColumnName("name").IsRequired();
            entity.Property(x => x.IsPublic).HasColumnName("is_public").IsRequired();
            entity.Property(x => x.Sequence).HasColumnName("sequence").IsRequired();
            entity.HasIndex(x => new { x.Category, x.Code }).IsUnique();
        });

        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.ToTable("product_tag");
            entity.HasKey(x => new { x.ProductId, x.TagId });
            entity.Property(x => x.ProductId).HasColumnName("product_id");
            entity.Property(x => x.TagId).HasColumnName("tag_id");
            entity.HasOne<Domain.Product>()
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Tag>()
                .WithMany()
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductMarket>(entity =>
        {
            entity.ToTable("product_market");
            entity.HasKey(x => new { x.ProductId, x.MarketCode });
            entity.Property(x => x.ProductId).HasColumnName("product_id");
            entity.Property(x => x.MarketCode).HasColumnName("market_code");
            entity.Property(x => x.LaunchedOn).HasColumnName("launched_on");
            entity.HasOne<Domain.Product>()
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Market>()
                .WithMany()
                .HasForeignKey(x => x.MarketCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductItem>(entity =>
        {
            entity.ToTable("product_item");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.ProductId).HasColumnName("product_id").IsRequired();
            entity.Property(x => x.Kind).HasColumnName("kind").IsRequired();
            entity.Property(x => x.Code).HasColumnName("code").IsRequired();
            entity.Property(x => x.Sequence).HasColumnName("sequence").IsRequired();
            entity.Property(x => x.Title).HasColumnName("title").IsRequired();
            entity.Property(x => x.Summary).HasColumnName("summary");
            entity.Property(x => x.Grouping).HasColumnName("grouping");
            entity.Property(x => x.IsOptional).HasColumnName("is_optional").IsRequired();
            entity.HasIndex(x => new { x.ProductId, x.Code }).IsUnique();
            entity.HasIndex(x => new { x.ProductId, x.Kind, x.Sequence }).IsUnique();
            entity.HasIndex(x => new { x.Id, x.ProductId }).IsUnique();
            entity.HasOne<Domain.Product>()
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("asset");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.ProductId).HasColumnName("product_id").IsRequired();
            entity.Property(x => x.ItemId).HasColumnName("item_id");
            entity.Property(x => x.Role).HasColumnName("role").IsRequired();
            entity.Property(x => x.Kind).HasColumnName("kind").IsRequired();
            entity.Property(x => x.LanguageCode).HasColumnName("language_code");
            entity.Property(x => x.Title).HasColumnName("title");
            entity.Property(x => x.GroupCode).HasColumnName("group_code");
            entity.Property(x => x.Provider).HasColumnName("provider").IsRequired();
            entity.Property(x => x.ProviderAssetId).HasColumnName("provider_asset_id");
            entity.Property(x => x.StreamUrl).HasColumnName("stream_url");
            entity.Property(x => x.DownloadUrl).HasColumnName("download_url");
            entity.Property(x => x.AllowStream).HasColumnName("allow_stream").IsRequired();
            entity.Property(x => x.AllowDownload).HasColumnName("allow_download").IsRequired();
            entity.Property(x => x.DurationSeconds).HasColumnName("duration_seconds");
            entity.Property(x => x.FileSizeBytes).HasColumnName("file_size_bytes");
            entity.HasOne<Domain.Product>()
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<ProductItem>()
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            entity.HasOne<Language>()
                .WithMany()
                .HasForeignKey(x => x.LanguageCode)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        modelBuilder.Entity<AssetMarket>(entity =>
        {
            entity.ToTable("asset_market");
            entity.HasKey(x => new { x.AssetId, x.MarketCode });
            entity.Property(x => x.AssetId).HasColumnName("asset_id");
            entity.Property(x => x.MarketCode).HasColumnName("market_code");
            entity.HasOne<Asset>()
                .WithMany()
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Market>()
                .WithMany()
                .HasForeignKey(x => x.MarketCode)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
