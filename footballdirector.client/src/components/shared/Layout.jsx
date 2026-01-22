import { useState, useEffect } from 'react';
import { NavLink, Outlet } from 'react-router-dom';
import { cn } from '@/lib/utils';
import { Home, Users, Briefcase, Inbox, Calendar, ChevronRight } from 'lucide-react';
import { getGameClock, advanceGameClock } from '@/api/api';

const navItems = [
    { to: '/', label: 'Club', icon: Home, end: true },
    { to: '/squad', label: 'Squad', icon: Users },
    { to: '/staff', label: 'Staff', icon: Briefcase },
    { to: '/inbox', label: 'Inbox', icon: Inbox },
];

const phaseLabels = {
    PreSeason: 'Pre-Season',
    EarlySeason: 'Early Season',
    TransferWindow: 'Transfer Window',
    LateSeason: 'Late Season',
    EndOfSeason: 'End of Season',
};

function GameClock() {
    const [clock, setClock] = useState(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        getGameClock().then(setClock).catch(console.error);
    }, []);

    const handleAdvance = async () => {
        setLoading(true);
        try {
            const updated = await advanceGameClock(1);
            setClock(updated);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    if (!clock) {
        return (
            <div className="p-3 border-t border-border">
                <div className="text-xs text-muted-foreground">Loading...</div>
            </div>
        );
    }

    const date = new Date(clock.currentDate);
    const formattedDate = date.toLocaleDateString('en-GB', {
        day: 'numeric',
        month: 'short',
        year: 'numeric',
    });

    return (
        <div className="p-3 border-t border-border space-y-2">
            <div className="flex items-center gap-2 text-xs text-muted-foreground">
                <Calendar className="h-3 w-3" />
                <span>{formattedDate}</span>
            </div>
            <div className="text-xs">
                <span className="text-muted-foreground">Season </span>
                <span className="text-foreground font-medium">{clock.season}/{clock.season + 1}</span>
            </div>
            <div className="text-xs">
                <span className="px-1.5 py-0.5 rounded bg-primary/20 text-primary font-medium">
                    {phaseLabels[clock.phase] || clock.phase}
                </span>
            </div>
            <button
                onClick={handleAdvance}
                disabled={loading}
                className={cn(
                    "w-full mt-2 flex items-center justify-center gap-1 px-2 py-1.5 rounded text-xs font-medium transition-colors",
                    "bg-primary text-primary-foreground hover:bg-primary/90",
                    "disabled:opacity-50 disabled:cursor-not-allowed"
                )}
            >
                {loading ? 'Advancing...' : 'Advance Day'}
                <ChevronRight className="h-3 w-3" />
            </button>
        </div>
    );
}

export function Layout() {
    return (
        <div className="flex min-h-screen">
            <nav className="w-52 bg-sidebar border-r border-border flex flex-col">
                <div className="p-4 border-b border-border">
                    <h1 className="text-lg font-bold text-foreground tracking-tight">
                        Football Director
                    </h1>
                </div>
                <div className="flex-1 p-2 space-y-1">
                    {navItems.map(({ to, label, icon: Icon, end }) => (
                        <NavLink
                            key={to}
                            to={to}
                            end={end}
                            className={({ isActive }) =>
                                cn(
                                    "flex items-center gap-3 px-3 py-2 rounded-md text-sm font-medium transition-colors",
                                    isActive
                                        ? "bg-primary text-primary-foreground"
                                        : "text-sidebar-foreground hover:bg-sidebar-accent"
                                )
                            }
                        >
                            <Icon className="h-4 w-4" />
                            {label}
                        </NavLink>
                    ))}
                </div>
                <GameClock />
            </nav>
            <main className="flex-1 p-6 overflow-y-auto">
                <Outlet />
            </main>
        </div>
    );
}
