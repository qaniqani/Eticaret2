using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AdminProject.Infrastructure.Mappers;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Infrastructure
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext() : base("name=AdminDbContext")
        {
            
        }
        public AdminDbContext(string connectionString = "AdminDbContext")
            : base(connectionString)
        {
            Database.SetInitializer<AdminDbContext>(null);
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<GalleryDetail> GalleryDetails { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<CategoryProductAssg> CategoryProductAssgs { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<MeasureDetail> MeasureDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderTemp> OrderTemps { get; set; }
        public DbSet<OrderProductAssg> OrderProductAssgs { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductMeasureAssg> ProductMeasureAssgs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyItem> PropertyItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Bulletin> Bulletines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations
                .Add(new AdminMapper())
                .Add(new CategoryMapper())
                .Add(new ContentMapper())
                .Add(new FileMapper())
                .Add(new GalleryMapper())
                .Add(new GalleryDetailMapper())
                .Add(new LanguageMapper())
                .Add(new SettingsMapper())
                .Add(new AddressMapper())
                .Add(new BankMapper())
                .Add(new BasketMapper())
                .Add(new BrandMapper())
                .Add(new CargoMapper())
                .Add(new CategoryProductAssgMapper())
                .Add(new CityMapper())
                .Add(new CommentMapper())
                .Add(new CountryMapper())
                .Add(new InvoiceMapper())
                .Add(new MeasureMapper())
                .Add(new MeasureDetailMapper())
                .Add(new OrderMapper())
                .Add(new OrderTempMapper())
                .Add(new OrderProductAssgMapper())
                .Add(new PictureMapper())
                .Add(new ProductMapper())
                .Add(new ProductMeasureAssgMapper())
                .Add(new RegionMapper())
                .Add(new CampaignMapper())
                .Add(new UserMapper())
                .Add(new PropertyMapper())
                .Add(new PropertyItemMapper())
                .Add(new FavoriteMapper())
                .Add(new BulletinMapper())
                .Add(new SliderMapper());
        }
    }
}