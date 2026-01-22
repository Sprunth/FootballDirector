using FootballDirector.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FootballDirector.Core.Data;

public static class GameDataExtensions
{
    public static IServiceCollection AddGameDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<GameDbContext>(options =>
            options.UseSqlite(connectionString));
        return services;
    }

    public static void EnsureGameDatabaseCreated(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();

        // Check if schema is outdated (missing tables) and recreate if needed
        // This is appropriate for development - in production you'd use migrations
        if (db.Database.CanConnect() && !TableExists(db, "GameClock"))
        {
            db.Database.EnsureDeleted();
        }

        db.Database.EnsureCreated();

        // Initialize game clock if not present
        if (!db.GameClock.Any())
        {
            db.GameClock.Add(new GameClock(
                Id: 1,
                CurrentDate: new DateTime(2024, 7, 1),
                Season: 2024,
                Phase: SeasonPhase.PreSeason));
            db.SaveChanges();
        }

        // Seed data if database is empty
        if (!db.Footballers.Any())
        {
            SeedData(db);
        }
    }

    private static bool TableExists(GameDbContext db, string tableName)
    {
        var connection = db.Database.GetDbConnection();
        connection.Open();
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
            var result = command.ExecuteScalar();
            return result != null;
        }
        finally
        {
            connection.Close();
        }
    }

    private static void SeedData(GameDbContext db)
    {
        // Footballers - DateOfBirth calculated so age is correct as of game start (July 1, 2024)
        db.Footballers.AddRange(
            new Footballer(1, "Danny", "Fletcher", new DateTime(1997, 3, 15), "England",
                new Personality(PersonalityType.Maverick, new Backstory(
                    "Grew up on a council estate in Sheffield with a hardworking single mother.",
                    "Scoring a hat-trick on his Premier League debut as an 18-year-old substitute.",
                    "Runs a charity providing sports equipment to underprivileged schools.")),
                "LW", 81, 92, 78, 74, 85, 45, 72),
            new Footballer(2, "Pablo", "Moreno", new DateTime(2007, 8, 22), "Spain",
                new Personality(PersonalityType.Virtuoso, new Backstory(
                    "Raised in a working-class neighborhood near Valencia by immigrant parents.",
                    "Being scouted by a top academy at just 8 years old after a street football video went viral.",
                    "Still lives with his grandmother who taught him his first skills with a tennis ball.")),
                "RW", 83, 88, 76, 79, 90, 32, 56),
            new Footballer(3, "Magnus", "Lindqvist", new DateTime(2000, 1, 8), "Norway",
                new Personality(PersonalityType.Warrior, new Backstory(
                    "Born into a family of cross-country skiers in Trondheim who wanted him to follow tradition.",
                    "Scoring 5 goals in a cup final after his team was down 2-0 at halftime.",
                    "Practices meditation daily and is obsessed with sleep optimization and recovery routines.")),
                "ST", 91, 89, 93, 65, 80, 45, 88),
            new Footballer(4, "Tyler", "Chambers", new DateTime(2003, 5, 30), "England",
                new Personality(PersonalityType.Heartbeat, new Backstory(
                    "Grew up in Birmingham with parents who ran a local youth football club.",
                    "Captained his country's U-21 team to a European championship.",
                    "Speaks fluent German after a teenage exchange program sparked his love of languages.")),
                "CAM", 88, 78, 82, 83, 86, 68, 78),
            new Footballer(5, "Luuk", "de Groot", new DateTime(1991, 11, 3), "Netherlands",
                new Personality(PersonalityType.Mentor, new Backstory(
                    "Lost his father to illness when he was a teenager in Rotterdam.",
                    "Being released by his boyhood club as a youngster and almost giving up on football.",
                    "Plays chess competitively and uses it to sharpen his tactical reading of the game.")),
                "CB", 89, 62, 60, 72, 55, 92, 86),
            new Footballer(6, "Sergio", "Vidal", new DateTime(1996, 4, 17), "Spain",
                new Personality(PersonalityType.Strategist, new Backstory(
                    "Grew up in a middle-class Seville family with parents who valued education.",
                    "Winning Player of the Tournament after a dominant European Championship.",
                    "Has a degree in economics and considered becoming a financial analyst.")),
                "CDM", 90, 58, 72, 88, 79, 88, 82),
            new Footballer(7, "Thierry", "Dubois", new DateTime(1998, 9, 12), "France",
                new Personality(PersonalityType.Showman, new Backstory(
                    "Raised in the Paris suburbs by a father who coached amateur football and a mother who was a track athlete.",
                    "Becoming a World Cup winner at 20 and gracing magazine covers worldwide.",
                    "Donates his entire national team salary to charity and runs a foundation for youth athletics.")),
                "ST", 91, 97, 89, 80, 92, 36, 78),
            new Footballer(8, "Iker", "Ruiz", new DateTime(2002, 2, 28), "Spain",
                new Personality(PersonalityType.Introvert, new Backstory(
                    "Grew up in a small coastal town in Galicia, far from mainland football academies.",
                    "Playing over 60 matches in a single season at age 19 for club and country.",
                    "Prefers staying home playing video games to going out, and is known for his quiet demeanor.")),
                "CM", 87, 72, 75, 88, 88, 72, 65)
        );

        // Staff - DateOfBirth calculated so age is correct as of game start (July 1, 2024)
        db.Staff.AddRange(
            new StaffMember(100, "Roberto", "Santini", new DateTime(1972, 6, 20), "Italy",
                new Personality(PersonalityType.Strategist, new Backstory(
                    "Grew up in a small town near Milan, son of a factory worker who never missed a Sunday match.",
                    "Leading a struggling Serie B team to promotion and a domestic cup final in the same season.",
                    "Collects vintage tactical boards and has an impressive library of football philosophy books.")),
                StaffRole.Manager,
                Specialization: null,
                Attacking: null, Defending: null, Goalkeeping: null, Tactics: 17, Communication: null,
                ManManagement: 15, Motivation: 16, MediaHandling: 12,
                InjuryPrevention: null, Recovery: null,
                JudgingAbility: null, JudgingPotential: null,
                BusinessAcumen: null, Negotiation: null,
                Wealth: null, Ambition: null),
            new StaffMember(101, "Yuki", "Nakamura", new DateTime(1986, 4, 5), "Japan",
                new Personality(PersonalityType.Virtuoso, new Backstory(
                    "Raised in Osaka by parents who ran a youth football academy.",
                    "Developing three players who went on to play for the national team.",
                    "Creates detailed video analysis packages set to classical music for player motivation.")),
                StaffRole.Coach,
                Specialization: CoachSpecialization.Attacking,
                Attacking: 16, Defending: 10, Goalkeeping: 5, Tactics: 14, Communication: 15,
                ManManagement: null, Motivation: null, MediaHandling: null,
                InjuryPrevention: null, Recovery: null,
                JudgingAbility: null, JudgingPotential: null,
                BusinessAcumen: null, Negotiation: null,
                Wealth: null, Ambition: null),
            new StaffMember(102, "Graham", "Whitmore", new DateTime(1979, 10, 11), "England",
                new Personality(PersonalityType.Mentor, new Backstory(
                    "Former professional defender who played over 400 matches in the lower leagues.",
                    "Keeping a record 12 clean sheets during his playing days in a single season.",
                    "Runs a popular podcast interviewing legendary defenders about the art of defending.")),
                StaffRole.Coach,
                Specialization: CoachSpecialization.Defending,
                Attacking: 8, Defending: 18, Goalkeeping: 7, Tactics: 15, Communication: 14,
                ManManagement: null, Motivation: null, MediaHandling: null,
                InjuryPrevention: null, Recovery: null,
                JudgingAbility: null, JudgingPotential: null,
                BusinessAcumen: null, Negotiation: null,
                Wealth: null, Ambition: null),
            new StaffMember(103, "Maria", "Ferreira", new DateTime(1983, 7, 25), "Portugal",
                new Personality(PersonalityType.Strategist, new Backstory(
                    "Daughter of a legendary Portuguese scout who discovered several world-class talents.",
                    "Recommending a player for £500k who was later sold for £40 million.",
                    "Speaks six languages fluently and travels over 200 days per year watching football.")),
                StaffRole.Scout,
                Specialization: null,
                Attacking: null, Defending: null, Goalkeeping: null, Tactics: null, Communication: null,
                ManManagement: null, Motivation: null, MediaHandling: null,
                InjuryPrevention: null, Recovery: null,
                JudgingAbility: 17, JudgingPotential: 19,
                BusinessAcumen: null, Negotiation: null,
                Wealth: null, Ambition: null),
            new StaffMember(104, "Anders", "Bergström", new DateTime(1988, 12, 3), "Sweden",
                new Personality(PersonalityType.Heartbeat, new Backstory(
                    "Former sports science student who specialized in elite athlete rehabilitation.",
                    "Helping a star player return from a career-threatening injury ahead of schedule.",
                    "Practices yoga daily and has completed multiple ultramarathons.")),
                StaffRole.Physio,
                Specialization: null,
                Attacking: null, Defending: null, Goalkeeping: null, Tactics: null, Communication: null,
                ManManagement: null, Motivation: null, MediaHandling: null,
                InjuryPrevention: 16, Recovery: 18,
                JudgingAbility: null, JudgingPotential: null,
                BusinessAcumen: null, Negotiation: null,
                Wealth: null, Ambition: null),
            new StaffMember(105, "Victoria", "Ashworth", new DateTime(1976, 2, 14), "England",
                new Personality(PersonalityType.Strategist, new Backstory(
                    "Former investment banker who fell in love with football through her children's youth teams.",
                    "Negotiating a stadium naming rights deal worth three times the previous valuation.",
                    "The club was founded by her great-grandfather, making her the fourth generation to lead it.")),
                StaffRole.ChiefExecutive,
                Specialization: null,
                Attacking: null, Defending: null, Goalkeeping: null, Tactics: null, Communication: null,
                ManManagement: null, Motivation: null, MediaHandling: null,
                InjuryPrevention: null, Recovery: null,
                JudgingAbility: null, JudgingPotential: null,
                BusinessAcumen: 17, Negotiation: 15,
                Wealth: null, Ambition: null)
        );

        // Club
        db.Clubs.Add(new Club(
            Id: 1,
            Name: "Ashworth United",
            Stadium: "Greenfield Park",
            League: "Premier Division",
            LeaguePosition: 7,
            Finances: new ClubFinances(
                Balance: 12_500_000,
                TransferBudget: 8_000_000,
                WageBudget: 450_000,
                CurrentWages: 385_000),
            Counts: new ClubCounts(
                Footballers: 8,
                Staff: 6,
                UnreadMessages: 2)));

        // Conversations and Messages
        var conv1 = new Conversation(1, 3, "Magnus Lindqvist", "Footballer", InitiatedByNpc: true,
            new DateTime(2024, 1, 15, 10, 30, 0), new DateTime(2024, 1, 15, 11, 45, 0),
            IsRead: false, "Contract Discussion",
            [
                new Message(1, 1, FromPlayer: false, "Boss, I wanted to talk about my contract situation. I've been performing well and I think it's time we discussed an extension.", new DateTime(2024, 1, 15, 10, 30, 0)),
                new Message(2, 1, FromPlayer: true, "Magnus, you've been excellent this season. What did you have in mind?", new DateTime(2024, 1, 15, 10, 45, 0)),
                new Message(3, 1, FromPlayer: false, "I'm looking for a longer commitment and terms that reflect my contribution. I want to be here long-term, but I also need to feel valued.", new DateTime(2024, 1, 15, 11, 45, 0)),
            ]);

        var conv2 = new Conversation(2, 103, "Maria Ferreira", "Scout", InitiatedByNpc: true,
            new DateTime(2024, 1, 14, 9, 0, 0), new DateTime(2024, 1, 14, 9, 15, 0),
            IsRead: false, "Youth Prospect Recommendation",
            [
                new Message(4, 2, FromPlayer: false, "I've been watching a young midfielder in the Portuguese second division. 19 years old, exceptional vision. I think he could be special.", new DateTime(2024, 1, 14, 9, 0, 0)),
                new Message(5, 2, FromPlayer: true, "Tell me more. What makes him stand out?", new DateTime(2024, 1, 14, 9, 10, 0)),
                new Message(6, 2, FromPlayer: false, "His passing range is remarkable for his age. He sees space before it opens. Reminds me of the player I recommended five years ago who's now worth £40m.", new DateTime(2024, 1, 14, 9, 15, 0)),
            ]);

        var conv3 = new Conversation(3, 100, "Roberto Santini", "Manager", InitiatedByNpc: false,
            new DateTime(2024, 1, 10, 14, 0, 0), new DateTime(2024, 1, 10, 14, 30, 0),
            IsRead: true, "Formation Discussion",
            [
                new Message(7, 3, FromPlayer: true, "Roberto, I've been thinking about the squad balance. What formation do you think suits our current players best?", new DateTime(2024, 1, 10, 14, 0, 0)),
                new Message(8, 3, FromPlayer: false, "With our attacking talent, I favor a 4-3-3. It lets us utilize the wingers and gives Chambers space to operate. But we need a holding midfielder to make it work.", new DateTime(2024, 1, 10, 14, 15, 0)),
                new Message(9, 3, FromPlayer: true, "Vidal could anchor the midfield. He has the tactical awareness.", new DateTime(2024, 1, 10, 14, 20, 0)),
                new Message(10, 3, FromPlayer: false, "Exactly what I was thinking. Vidal reads the game beautifully. With him holding, we can be more adventurous going forward.", new DateTime(2024, 1, 10, 14, 30, 0)),
            ]);

        var conv4 = new Conversation(4, 7, "Thierry Dubois", "Footballer", InitiatedByNpc: true,
            new DateTime(2024, 1, 8, 16, 0, 0), new DateTime(2024, 1, 8, 16, 10, 0),
            IsRead: true, "Media Appearance Request",
            [
                new Message(11, 4, FromPlayer: false, "I've been invited to a charity gala next week. It would be great publicity for my foundation and the club. May I have permission to attend?", new DateTime(2024, 1, 8, 16, 0, 0)),
                new Message(12, 4, FromPlayer: true, "Of course, Thierry. Your charity work reflects well on everyone. Just make sure you're fresh for the weekend match.", new DateTime(2024, 1, 8, 16, 10, 0)),
            ]);

        db.Conversations.AddRange(conv1, conv2, conv3, conv4);

        db.SaveChanges();
    }
}
