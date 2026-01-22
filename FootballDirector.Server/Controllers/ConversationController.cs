using FootballDirector.Contracts;
using FootballDirector.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballDirector.Server.Controllers;

[ApiController]
public class ConversationController(GameDbContext db) : ControllerBase
{
    /// <summary>
    /// Get all NPC-initiated conversations (inbox view).
    /// </summary>
    [HttpGet("api/inbox")]
    [ProducesResponseType<IEnumerable<ConversationSummary>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ConversationSummary>>> GetInbox()
    {
        var conversations = await db.Conversations
            .Include(c => c.Messages)
            .Where(c => c.InitiatedByNpc)
            .OrderByDescending(c => c.LastMessageAt)
            .ToListAsync();

        var inbox = conversations.Select(ToSummary);
        return Ok(inbox);
    }

    /// <summary>
    /// Get a specific conversation with full message history.
    /// </summary>
    [HttpGet("api/conversation/{id:int}")]
    [ProducesResponseType<Conversation>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Conversation>> GetById(int id)
    {
        var conversation = await db.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conversation is null)
            return NotFound();
        return Ok(conversation);
    }

    /// <summary>
    /// Get all conversations with a specific person (for person detail view).
    /// </summary>
    [HttpGet("api/person/{personId:int}/conversations")]
    [ProducesResponseType<IEnumerable<ConversationSummary>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ConversationSummary>>> GetByPerson(int personId)
    {
        var conversations = await db.Conversations
            .Include(c => c.Messages)
            .Where(c => c.PersonId == personId)
            .OrderByDescending(c => c.LastMessageAt)
            .ToListAsync();

        var summaries = conversations.Select(ToSummary);
        return Ok(summaries);
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
