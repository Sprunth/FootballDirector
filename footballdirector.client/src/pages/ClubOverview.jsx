import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getClub } from '@/api/api';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui';
import { cn } from '@/lib/utils';
import { TrendingUp, Users, Briefcase, Inbox } from 'lucide-react';

export function ClubOverview() {
    const [club, setClub] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        getClub()
            .then(data => {
                setClub(data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    if (loading) return <div className="p-8 text-center text-muted-foreground">Loading club data...</div>;
    if (error) return <div className="p-8 text-center text-danger">Error: {error}</div>;
    if (!club) return null;

    const formatCurrency = (value) => {
        return new Intl.NumberFormat('en-GB', {
            style: 'currency',
            currency: 'GBP',
            maximumFractionDigits: 0
        }).format(value);
    };

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-2xl font-bold tracking-tight">{club.name}</h1>
                <p className="text-muted-foreground">{club.stadium} | {club.league}</p>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {/* League Position */}
                <Card>
                    <CardHeader className="pb-2">
                        <CardTitle className="text-sm font-medium text-muted-foreground flex items-center gap-2">
                            <TrendingUp className="h-4 w-4" />
                            League Position
                        </CardTitle>
                    </CardHeader>
                    <CardContent>
                        <div className="text-4xl font-bold text-primary">{club.leaguePosition}</div>
                        <p className="text-sm text-muted-foreground">{club.league}</p>
                    </CardContent>
                </Card>

                {/* Finances */}
                <Card className="md:col-span-2 lg:col-span-1">
                    <CardHeader className="pb-2">
                        <CardTitle className="text-sm font-medium text-muted-foreground">
                            Finances
                        </CardTitle>
                    </CardHeader>
                    <CardContent className="space-y-2">
                        <FinanceRow
                            label="Balance"
                            value={formatCurrency(club.finances.balance)}
                            positive={club.finances.balance >= 0}
                        />
                        <FinanceRow
                            label="Transfer Budget"
                            value={formatCurrency(club.finances.transferBudget)}
                        />
                        <FinanceRow
                            label="Wage Budget"
                            value={`${formatCurrency(club.finances.wageBudget)}/wk`}
                        />
                        <FinanceRow
                            label="Current Wages"
                            value={`${formatCurrency(club.finances.currentWages)}/wk`}
                        />
                    </CardContent>
                </Card>

                {/* Quick Links */}
                <QuickLinkCard
                    icon={Users}
                    title="Squad"
                    value={club.counts.footballers}
                    label="Players"
                    onClick={() => navigate('/squad')}
                />
                <QuickLinkCard
                    icon={Briefcase}
                    title="Staff"
                    value={club.counts.staff}
                    label="Staff Members"
                    onClick={() => navigate('/staff')}
                />
                <QuickLinkCard
                    icon={Inbox}
                    title="Inbox"
                    value={club.counts.unreadMessages}
                    label="Unread Messages"
                    onClick={() => navigate('/inbox')}
                    highlight={club.counts.unreadMessages > 0}
                />
            </div>
        </div>
    );
}

function FinanceRow({ label, value, positive }) {
    return (
        <div className="flex justify-between items-center py-1 border-b border-border last:border-0">
            <span className="text-sm text-muted-foreground">{label}</span>
            <span className={cn(
                "text-sm font-medium",
                positive === true && "text-success",
                positive === false && "text-danger"
            )}>
                {value}
            </span>
        </div>
    );
}

function QuickLinkCard({ icon: Icon, title, value, label, onClick, highlight }) {
    return (
        <Card
            className={cn(
                "cursor-pointer transition-all hover:border-primary/50",
                highlight && "border-primary/30"
            )}
            onClick={onClick}
        >
            <CardHeader className="pb-2">
                <CardTitle className="text-sm font-medium text-muted-foreground flex items-center gap-2">
                    <Icon className="h-4 w-4" />
                    {title}
                </CardTitle>
            </CardHeader>
            <CardContent>
                <div className={cn(
                    "text-4xl font-bold",
                    highlight ? "text-primary" : "text-foreground"
                )}>
                    {value}
                </div>
                <p className="text-sm text-muted-foreground">{label}</p>
            </CardContent>
        </Card>
    );
}
