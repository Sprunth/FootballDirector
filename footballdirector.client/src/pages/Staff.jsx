import { useState, useEffect } from 'react';
import { getStaff } from '@/api/api';
import { Card, CardContent, Badge, StatBar } from '@/components/ui';
import { cn } from '@/lib/utils';

const ROLE_LABELS = {
    0: 'Coach', 1: 'Manager', 2: 'Physio', 3: 'Scout', 4: 'Chief Executive', 5: 'Club Owner',
    'Coach': 'Coach', 'Manager': 'Manager', 'Physio': 'Physio', 'Scout': 'Scout',
    'ChiefExecutive': 'Chief Executive', 'ClubOwner': 'Club Owner',
};

const SPECIALIZATION_LABELS = {
    0: 'Attacking', 1: 'Defending', 2: 'Goalkeeping', 3: 'Fitness', 4: 'Set Piece',
};

export function Staff() {
    const [staff, setStaff] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedStaff, setSelectedStaff] = useState(null);

    useEffect(() => {
        getStaff()
            .then(data => {
                setStaff(data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    if (loading) return <div className="p-8 text-center text-muted-foreground">Loading staff...</div>;
    if (error) return <div className="p-8 text-center text-danger">Error: {error}</div>;

    const getRoleLabel = (role) => ROLE_LABELS[role] || role;

    const getBadge = (member) => {
        const role = getRoleLabel(member.role);
        if (member.specialization !== null && member.specialization !== undefined) {
            const spec = SPECIALIZATION_LABELS[member.specialization] || member.specialization;
            return `${spec} ${role}`;
        }
        return role;
    };

    const getStaffStats = (member) => {
        const stats = [];
        if (member.attacking != null) stats.push({ label: 'ATT', value: member.attacking });
        if (member.defending != null) stats.push({ label: 'DEF', value: member.defending });
        if (member.goalkeeping != null) stats.push({ label: 'GK', value: member.goalkeeping });
        if (member.tactics != null) stats.push({ label: 'TAC', value: member.tactics });
        if (member.communication != null) stats.push({ label: 'COM', value: member.communication });
        if (member.manManagement != null) stats.push({ label: 'MAN', value: member.manManagement });
        if (member.motivation != null) stats.push({ label: 'MOT', value: member.motivation });
        if (member.mediaHandling != null) stats.push({ label: 'MED', value: member.mediaHandling });
        if (member.injuryPrevention != null) stats.push({ label: 'PREV', value: member.injuryPrevention });
        if (member.recovery != null) stats.push({ label: 'REC', value: member.recovery });
        if (member.judgingAbility != null) stats.push({ label: 'JA', value: member.judgingAbility });
        if (member.judgingPotential != null) stats.push({ label: 'JP', value: member.judgingPotential });
        if (member.businessAcumen != null) stats.push({ label: 'BUS', value: member.businessAcumen });
        if (member.negotiation != null) stats.push({ label: 'NEG', value: member.negotiation });
        if (member.ambition != null) stats.push({ label: 'AMB', value: member.ambition });
        return stats;
    };

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-2xl font-bold tracking-tight">Staff</h1>
                <p className="text-muted-foreground">{staff.length} staff members</p>
            </div>

            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-3">
                {staff.map(member => (
                    <StaffCard
                        key={member.id}
                        member={member}
                        badge={getBadge(member)}
                        selected={selectedStaff?.id === member.id}
                        onClick={() => setSelectedStaff(member)}
                    />
                ))}
            </div>

            {selectedStaff && (
                <StaffDetail
                    member={selectedStaff}
                    badge={getBadge(selectedStaff)}
                    stats={getStaffStats(selectedStaff)}
                />
            )}
        </div>
    );
}

function StaffCard({ member, badge, selected, onClick }) {
    return (
        <Card
            className={cn(
                "cursor-pointer transition-all hover:border-primary/50",
                selected && "border-primary"
            )}
            onClick={onClick}
        >
            <CardContent className="p-3">
                <Badge variant="secondary" className="mb-2 text-xs">{badge}</Badge>
                <div className="font-medium truncate">
                    {member.firstName} {member.lastName}
                </div>
                <div className="flex justify-between text-xs text-muted-foreground mt-1">
                    <span>{member.nationality}</span>
                    <span>Age {member.age}</span>
                </div>
            </CardContent>
        </Card>
    );
}

function StaffDetail({ member, badge, stats }) {
    return (
        <Card>
            <CardContent className="p-6">
                <div className="flex flex-col md:flex-row md:items-start md:justify-between gap-6">
                    <div className="flex-1">
                        <h2 className="text-xl font-bold">
                            {member.firstName} {member.lastName}
                        </h2>
                        <p className="text-muted-foreground">
                            {badge} | {member.nationality} | Age {member.age}
                        </p>

                        {stats.length > 0 && (
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
                                            maxValue={20}
                                        />
                                    ))}
                                </div>
                            </div>
                        )}
                    </div>

                    {member.personality && (
                        <div className="flex-1 md:border-l md:border-border md:pl-6">
                            <h3 className="text-xs font-medium text-muted-foreground uppercase tracking-wide mb-3">
                                Personality
                            </h3>
                            <Badge className="mb-4">{member.personality.type}</Badge>

                            {member.personality.backstory && (
                                <div className="space-y-4 text-sm">
                                    <div>
                                        <div className="text-xs text-muted-foreground uppercase mb-1">Background</div>
                                        <p className="text-foreground/90">{member.personality.backstory.upbringing}</p>
                                    </div>
                                    <div>
                                        <div className="text-xs text-muted-foreground uppercase mb-1">Defining Moment</div>
                                        <p className="text-foreground/90">{member.personality.backstory.coreMemory}</p>
                                    </div>
                                    <div>
                                        <div className="text-xs text-muted-foreground uppercase mb-1">Fun Fact</div>
                                        <p className="text-foreground/90">{member.personality.backstory.funFact}</p>
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
