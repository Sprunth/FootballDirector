import { useState, useEffect } from 'react';
import { getInbox, getConversation } from '@/api/api';
import { Card, CardContent, Badge } from '@/components/ui';
import { cn } from '@/lib/utils';
import { Mail, MailOpen } from 'lucide-react';

export function Inbox() {
    const [conversations, setConversations] = useState([]);
    const [selectedConversation, setSelectedConversation] = useState(null);
    const [loading, setLoading] = useState(true);
    const [loadingConversation, setLoadingConversation] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        getInbox()
            .then(data => {
                setConversations(data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    const handleSelectConversation = async (summary) => {
        setLoadingConversation(true);
        try {
            const full = await getConversation(summary.id);
            setSelectedConversation(full);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoadingConversation(false);
        }
    };

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-GB', {
            day: 'numeric',
            month: 'short',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    if (loading) return <div className="p-8 text-center text-muted-foreground">Loading inbox...</div>;
    if (error) return <div className="p-8 text-center text-danger">Error: {error}</div>;

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-2xl font-bold tracking-tight">Inbox</h1>
                <p className="text-muted-foreground">{conversations.length} conversations</p>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
                {/* Conversation List */}
                <div className="space-y-2">
                    {conversations.map(conv => (
                        <ConversationItem
                            key={conv.id}
                            conversation={conv}
                            selected={selectedConversation?.id === conv.id}
                            onClick={() => handleSelectConversation(conv)}
                            formatDate={formatDate}
                        />
                    ))}

                    {conversations.length === 0 && (
                        <Card>
                            <CardContent className="p-8 text-center text-muted-foreground">
                                No messages in your inbox
                            </CardContent>
                        </Card>
                    )}
                </div>

                {/* Message Thread */}
                <div>
                    {loadingConversation && (
                        <Card>
                            <CardContent className="p-8 text-center text-muted-foreground">
                                Loading conversation...
                            </CardContent>
                        </Card>
                    )}

                    {selectedConversation && !loadingConversation && (
                        <Card className="sticky top-6">
                            <CardContent className="p-4">
                                <div className="mb-4 pb-4 border-b border-border">
                                    <h2 className="font-semibold">
                                        {selectedConversation.subject || 'Conversation'}
                                    </h2>
                                    <p className="text-sm text-muted-foreground">
                                        with {selectedConversation.personName} ({selectedConversation.personRole})
                                    </p>
                                </div>

                                <div className="space-y-3 max-h-96 overflow-y-auto">
                                    {selectedConversation.messages.map(msg => (
                                        <Message
                                            key={msg.id}
                                            message={msg}
                                            formatDate={formatDate}
                                        />
                                    ))}
                                </div>
                            </CardContent>
                        </Card>
                    )}

                    {!selectedConversation && !loadingConversation && (
                        <Card>
                            <CardContent className="p-8 text-center text-muted-foreground">
                                Select a conversation to view
                            </CardContent>
                        </Card>
                    )}
                </div>
            </div>
        </div>
    );
}

function ConversationItem({ conversation, selected, onClick, formatDate }) {
    const isUnread = !conversation.isRead;

    return (
        <Card
            className={cn(
                "cursor-pointer transition-all hover:border-primary/50",
                selected && "border-primary",
                isUnread && "border-l-2 border-l-primary"
            )}
            onClick={onClick}
        >
            <CardContent className="p-3">
                <div className="flex items-start gap-3">
                    <div className="mt-0.5">
                        {isUnread ? (
                            <Mail className="h-4 w-4 text-primary" />
                        ) : (
                            <MailOpen className="h-4 w-4 text-muted-foreground" />
                        )}
                    </div>
                    <div className="flex-1 min-w-0">
                        <div className="flex items-center justify-between gap-2">
                            <span className={cn(
                                "font-medium truncate",
                                isUnread && "text-foreground",
                                !isUnread && "text-muted-foreground"
                            )}>
                                {conversation.personName}
                            </span>
                            <span className="text-xs text-muted-foreground whitespace-nowrap">
                                {formatDate(conversation.lastMessageAt || conversation.startedAt)}
                            </span>
                        </div>
                        <Badge variant="outline" className="text-xs mt-1">
                            {conversation.personRole}
                        </Badge>
                        {conversation.subject && (
                            <div className={cn(
                                "text-sm mt-1 truncate",
                                isUnread ? "text-foreground" : "text-muted-foreground"
                            )}>
                                {conversation.subject}
                            </div>
                        )}
                        {conversation.lastMessagePreview && (
                            <div className="text-xs text-muted-foreground mt-1 truncate">
                                {conversation.lastMessagePreview}
                            </div>
                        )}
                    </div>
                </div>
            </CardContent>
        </Card>
    );
}

function Message({ message, formatDate }) {
    const isFromPlayer = message.fromPlayer;

    return (
        <div className={cn(
            "flex",
            isFromPlayer ? "justify-end" : "justify-start"
        )}>
            <div className={cn(
                "max-w-[85%] rounded-lg px-3 py-2",
                isFromPlayer
                    ? "bg-primary text-primary-foreground rounded-br-sm"
                    : "bg-muted text-foreground rounded-bl-sm"
            )}>
                <p className="text-sm">{message.content}</p>
                <p className={cn(
                    "text-xs mt-1",
                    isFromPlayer ? "text-primary-foreground/60" : "text-muted-foreground"
                )}>
                    {formatDate(message.sentAt)}
                </p>
            </div>
        </div>
    );
}
