import { NavLink, Outlet } from 'react-router-dom';
import { cn } from '@/lib/utils';
import { Home, Users, Briefcase, Inbox } from 'lucide-react';

const navItems = [
    { to: '/', label: 'Club', icon: Home, end: true },
    { to: '/squad', label: 'Squad', icon: Users },
    { to: '/staff', label: 'Staff', icon: Briefcase },
    { to: '/inbox', label: 'Inbox', icon: Inbox },
];

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
            </nav>
            <main className="flex-1 p-6 overflow-y-auto">
                <Outlet />
            </main>
        </div>
    );
}
