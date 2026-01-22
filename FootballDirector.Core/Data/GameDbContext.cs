using FootballDirector.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FootballDirector.Core.Data;

public class GameDbContext : DbContext
{
    public DbSet<Footballer> Footballers => Set<Footballer>();
    public DbSet<StaffMember> Staff => Set<StaffMember>();
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<GameClock> GameClock => Set<GameClock>();

    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureFootballer(modelBuilder);
        ConfigureStaffMember(modelBuilder);
        ConfigureClub(modelBuilder);
        ConfigureConversation(modelBuilder);
        ConfigureMessage(modelBuilder);
        ConfigureGameClock(modelBuilder);
    }

    private static void ConfigureFootballer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Footballer>(entity =>
        {
            entity.ToTable("Footballers");
            entity.HasKey(f => f.Id);

            // Configure nested owned types: Personality -> Backstory
            entity.OwnsOne(f => f.Personality, personality =>
            {
                personality.Property(p => p.Type).HasColumnName("PersonalityType");
                personality.OwnsOne(p => p.Backstory, backstory =>
                {
                    backstory.Property(b => b.Upbringing).HasColumnName("BackstoryUpbringing");
                    backstory.Property(b => b.CoreMemory).HasColumnName("BackstoryCoreMemory");
                    backstory.Property(b => b.FunFact).HasColumnName("BackstoryFunFact");
                });
            });
        });
    }

    private static void ConfigureStaffMember(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StaffMember>(entity =>
        {
            entity.ToTable("Staff");
            entity.HasKey(s => s.Id);

            // Configure nested owned types: Personality -> Backstory
            entity.OwnsOne(s => s.Personality, personality =>
            {
                personality.Property(p => p.Type).HasColumnName("PersonalityType");
                personality.OwnsOne(p => p.Backstory, backstory =>
                {
                    backstory.Property(b => b.Upbringing).HasColumnName("BackstoryUpbringing");
                    backstory.Property(b => b.CoreMemory).HasColumnName("BackstoryCoreMemory");
                    backstory.Property(b => b.FunFact).HasColumnName("BackstoryFunFact");
                });
            });
        });
    }

    private static void ConfigureClub(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>(entity =>
        {
            entity.ToTable("Clubs");
            entity.HasKey(c => c.Id);

            // Configure owned types
            entity.OwnsOne(c => c.Finances, finances =>
            {
                finances.Property(f => f.Balance).HasColumnName("Balance");
                finances.Property(f => f.TransferBudget).HasColumnName("TransferBudget");
                finances.Property(f => f.WageBudget).HasColumnName("WageBudget");
                finances.Property(f => f.CurrentWages).HasColumnName("CurrentWages");
            });

            entity.OwnsOne(c => c.Counts, counts =>
            {
                counts.Property(c => c.Footballers).HasColumnName("FootballerCount");
                counts.Property(c => c.Staff).HasColumnName("StaffCount");
                counts.Property(c => c.UnreadMessages).HasColumnName("UnreadMessageCount");
            });
        });
    }

    private static void ConfigureConversation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.ToTable("Conversations");
            entity.HasKey(c => c.Id);

            // Configure the Messages navigation property
            entity.HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore the Messages property for basic queries (load separately)
            entity.Navigation(c => c.Messages).AutoInclude(false);
        });
    }

    private static void ConfigureMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Messages");
            entity.HasKey(m => m.Id);
        });
    }

    private static void ConfigureGameClock(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameClock>(entity =>
        {
            entity.ToTable("GameClock");
            entity.HasKey(c => c.Id);
        });
    }
}
