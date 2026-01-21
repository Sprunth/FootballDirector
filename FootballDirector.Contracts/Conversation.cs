namespace FootballDirector.Contracts;

/// <summary>
/// A conversation thread between the player and an NPC.
/// </summary>
public record Conversation(
    int Id,
    int PersonId,               // The NPC we're talking to
    string PersonName,          // Denormalized for display
    string PersonRole,          // "Footballer", "Coach", "Scout", etc.
    bool InitiatedByNpc,        // True = shows in Inbox, False = player started it
    DateTime StartedAt,
    DateTime? LastMessageAt,
    bool IsRead,
    string? Subject,            // Optional conversation topic
    List<Message> Messages);

/// <summary>
/// Summary for list views (without full message history).
/// </summary>
public record ConversationSummary(
    int Id,
    int PersonId,
    string PersonName,
    string PersonRole,
    bool InitiatedByNpc,
    DateTime StartedAt,
    DateTime? LastMessageAt,
    bool IsRead,
    string? Subject,
    string? LastMessagePreview); // First ~100 chars of last message

/// <summary>
/// A single message in a conversation.
/// </summary>
public record Message(
    int Id,
    int ConversationId,
    bool FromPlayer,            // True = player sent, False = NPC sent
    string Content,
    DateTime SentAt);
