import { cn } from "@/lib/utils";

export function Progress({ value, max = 100, className, indicatorClassName }) {
    const percentage = Math.min(100, Math.max(0, (value / max) * 100));

    return (
        <div
            className={cn(
                "h-2 w-full overflow-hidden rounded-sm bg-muted",
                className
            )}
        >
            <div
                className={cn("h-full transition-all duration-300", indicatorClassName)}
                style={{ width: `${percentage}%` }}
            />
        </div>
    );
}

// Stat bar with label and value display
export function StatBar({ label, value, maxValue = 99 }) {
    const percentage = (value / maxValue) * 100;

    const getColorClass = () => {
        if (percentage >= 80) return "bg-success";
        if (percentage >= 60) return "bg-warning";
        return "bg-danger";
    };

    return (
        <div className="flex items-center gap-2">
            <span className="w-10 text-xs font-medium text-muted-foreground">
                {label}
            </span>
            <Progress
                value={value}
                max={maxValue}
                className="flex-1"
                indicatorClassName={getColorClass()}
            />
            <span className="w-6 text-right text-xs font-semibold">
                {value}
            </span>
        </div>
    );
}
