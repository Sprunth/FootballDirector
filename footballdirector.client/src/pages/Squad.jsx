import { useState, useEffect } from 'react';
import { getFootballers } from '@/api/api';
import { Card, CardContent, Badge, StatBar } from '@/components/ui';
import { cn } from '@/lib/utils';

export function Squad() {
    const [footballers, setFootballers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedPlayer, setSelectedPlayer] = useState(null);

    useEffect(() => {
        getFootballers()
            .then(data => {
                setFootballers(data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    if (loading) return <div className="p-8 text-center text-muted-foreground">Loading squad...</div>;
    if (error) return <div className="p-8 text-center text-danger">Error: {error}</div>;

    const getPlayerStats = (player) => [
        { label: 'PAC', value: player.pace },
        { label: 'SHO', value: player.shooting },
        { label: 'PAS', value: player.passing },
        { label: 'DRI', value: player.dribbling },
        { label: 'DEF', value: player.defending },
        { label: 'PHY', value: player.physical },
    ];

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-2xl font-bold tracking-tight">Squad</h1>
                <p className="text-muted-foreground">{footballers.length} players</p>
            </div>

            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-3">
                {footballers.map(player => (
                    <PlayerCard
                        key={player.id}
                        player={player}
                        selected={selectedPlayer?.id === player.id}
                        onClick={() => setSelectedPlayer(player)}
                    />
                ))}
            </div>

            {selectedPlayer && (
                <PlayerDetail
                    player={selectedPlayer}
                    stats={getPlayerStats(selectedPlayer)}
                />
            )}
        </div>
    );
}

function PlayerCard({ player, selected, onClick }) {
    return (
        <Card
            className={cn(
                "cursor-pointer transition-all hover:border-primary/50",
                selected && "border-primary"
            )}
            onClick={onClick}
        >
            <CardContent className="p-3">
                <div className="flex items-start justify-between mb-2">
                    <span className="text-2xl font-bold text-primary">
                        {player.overallRating}
                    </span>
                    <Badge variant="secondary">{player.position}</Badge>
                </div>
                <div className="font-medium truncate">
                    {player.firstName} {player.lastName}
                </div>
                <div className="flex justify-between text-xs text-muted-foreground mt-1">
                    <span>{player.nationality}</span>
                    <span>Age {player.age}</span>
                </div>
            </CardContent>
        </Card>
    );
}

function PlayerDetail({ player, stats }) {
    return (
        <Card>
            <CardContent className="p-6">
                <div className="flex flex-col md:flex-row md:items-start md:justify-between gap-6">
                    <div className="flex-1">
                        <h2 className="text-xl font-bold">
                            {player.firstName} {player.lastName}
                        </h2>
                        <p className="text-muted-foreground">
                            {player.position} | {player.nationality} | Age {player.age}
                        </p>

                        <div className="mt-6">
                            <h3 className="text-xs font-medium text-muted-foreground uppercase tracking-wide mb-3">
                                Attributes
                            </h3>
                            <div className="grid grid-cols-2 gap-x-6 gap-y-2">
                                {stats.map(stat => (
                                    <StatBar
                                        key={stat.label}
                                        label={stat.label}
                                        value={stat.value}
                                        maxValue={99}
                                    />
                                ))}
                            </div>
                        </div>
                    </div>

                    {player.personality && (
                        <div className="flex-1 md:border-l md:border-border md:pl-6">
                            <h3 className="text-xs font-medium text-muted-foreground uppercase tracking-wide mb-3">
                                Personality
                            </h3>
                            <Badge className="mb-4">{player.personality.type}</Badge>

                            {player.personality.backstory && (
                                <div className="space-y-4 text-sm">
                                    <div>
                                        <div className="text-xs text-muted-foreground uppercase mb-1">Background</div>
                                        <p className="text-foreground/90">{player.personality.backstory.upbringing}</p>
                                    </div>
                                    <div>
                                        <div className="text-xs text-muted-foreground uppercase mb-1">Defining Moment</div>
                                        <p className="text-foreground/90">{player.personality.backstory.coreMemory}</p>
                                    </div>
                                    <div>
                                        <div className="text-xs text-muted-foreground uppercase mb-1">Fun Fact</div>
                                        <p className="text-foreground/90">{player.personality.backstory.funFact}</p>
                                    </div>
                                </div>
                            )}
                        </div>
                    )}
                </div>
            </CardContent>
        </Card>
    );
}
