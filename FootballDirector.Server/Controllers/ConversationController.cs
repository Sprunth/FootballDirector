using FootballDirector.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FootballDirector.Server.Controllers;

[ApiController]
public class ConversationController : ControllerBase
{
    // Sample conversations - will come from game state service later
    private static readonly List<Conversation> Conversations =
    [
        new(1, 3, "Magnus Lindqvist", "Footballer", InitiatedByNpc: true,
            new DateTime(2024, 1, 15, 10, 30, 0), new DateTime(2024, 1, 15, 11, 45, 0),
            IsRead: false, "Contract Discussion",
            [
                new(1, 1, FromPlayer: false, "Boss, I wanted to talk about my contract situation. I've been performing well and I think it's time we discussed an extension.", new DateTime(2024, 1, 15, 10, 30, 0)),
                new(2, 1, FromPlayer: true, "Magnus, you've been excellent this season. What did you have in mind?", new DateTime(2024, 1, 15, 10, 45, 0)),
                new(3, 1, FromPlayer: false, "I'm looking for a longer commitment and terms that reflect my contribution. I want to be here long-term, but I also need to feel valued.", new DateTime(2024, 1, 15, 11, 45, 0)),
            ]),

        new(2, 103, "Maria Ferreira", "Scout", InitiatedByNpc: true,
            new DateTime(2024, 1, 14, 9, 0, 0), new DateTime(2024, 1, 14, 9, 15, 0),
            IsRead: false, "Youth Prospect Recommendation",
            [
                new(4, 2, FromPlayer: false, "I've been watching a young midfielder in the Portuguese second division. 19 years old, exceptional vision. I think he could be special.", new DateTime(2024, 1, 14, 9, 0, 0)),
                new(5, 2, FromPlayer: true, "Tell me more. What makes him stand out?", new DateTime(2024, 1, 14, 9, 10, 0)),
                new(6, 2, FromPlayer: false, "His passing range is remarkable for his age. He sees space before it opens. Reminds me of the player I recommended five years ago who's now worth £40m.", new DateTime(2024, 1, 14, 9, 15, 0)),
            ]),

        new(3, 100, "Roberto Santini", "Manager", InitiatedByNpc: false,
            new DateTime(2024, 1, 10, 14, 0, 0), new DateTime(2024, 1, 10, 14, 30, 0),
            IsRead: true, "Formation Discussion",
            [
                new(7, 3, FromPlayer: true, "Roberto, I've been thinking about the squad balance. What formation do you think suits our current players best?", new DateTime(2024, 1, 10, 14, 0, 0)),
                new(8, 3, FromPlayer: false, "With our attacking talent, I favor a 4-3-3. It lets us utilize the wingers and gives Chambers space to operate. But we need a holding midfielder to make it work.", new DateTime(2024, 1, 10, 14, 15, 0)),
                new(9, 3, FromPlayer: true, "Vidal could anchor the midfield. He has the tactical awareness.", new DateTime(2024, 1, 10, 14, 20, 0)),
                new(10, 3, FromPlayer: false, "Exactly what I was thinking. Vidal reads the game beautifully. With him holding, we can be more adventurous going forward.", new DateTime(2024, 1, 10, 14, 30, 0)),
            ]),

        new(4, 7, "Thierry Dubois", "Footballer", InitiatedByNpc: true,
            new DateTime(2024, 1, 8, 16, 0, 0), new DateTime(2024, 1, 8, 16, 10, 0),
            IsRead: true, "Media Appearance Request",
            [
                new(11, 4, FromPlayer: false, "I've been invited to a charity gala next week. It would be great publicity for my foundation and the club. May I have permission to attend?", new DateTime(2024, 1, 8, 16, 0, 0)),
                new(12, 4, FromPlayer: true, "Of course, Thierry. Your charity work reflects well on everyone. Just make sure you're fresh for the weekend match.", new DateTime(2024, 1, 8, 16, 10, 0)),
            ]),
    ];

    /// <summary>
    /// Get all NPC-initiated conversations (inbox view).
    /// </summary>
    [HttpGet("api/inbox")]
    [ProducesResponseType<IEnumerable<ConversationSummary>>(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ConversationSummary>> GetInbox()
    {
        var inbox = Conversations
            .Where(c => c.InitiatedByNpc)
            .OrderByDescending(c => c.LastMessageAt)
            .Select(ToSummary);
        return Ok(inbox);
    }

    /// <summary>
    /// Get a specific conversation with full message history.
    /// </summary>
    [HttpGet("api/conversation/{id:int}")]
    [ProducesResponseType<Conversation>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Conversation> GetById(int id)
    {
        var conversation = Conversations.FirstOrDefault(c => c.Id == id);
        if (conversation is null)
            return NotFound();
        return Ok(conversation);
    }

    /// <summary>
    /// Get all conversations with a specific person (for person detail view).
    /// </summary>
    [HttpGet("api/person/{personId:int}/conversations")]
    [ProducesResponseType<IEnumerable<ConversationSummary>>(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ConversationSummary>> GetByPerson(int personId)
    {
        var conversations = Conversations
            .Where(c => c.PersonId == personId)
            .OrderByDescending(c => c.LastMessageAt)
            .Select(ToSummary);
        return Ok(conversations);
    }

    private static ConversationSummary ToSummary(Conversation c)
    {
        var lastMessage = c.Messages.LastOrDefault();
        var preview = lastMessage?.Content;
        if (preview?.Length > 100)
            preview = preview[..100] + "...";

        return new ConversationSummary(
            c.Id, c.PersonId, c.PersonName, c.PersonRole,
            c.InitiatedByNpc, c.StartedAt, c.LastMessageAt,
            c.IsRead, c.Subject, preview);
    }
}
