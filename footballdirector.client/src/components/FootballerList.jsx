import { useState, useEffect } from 'react';
import { getFootballers } from '../api/footballerApi';
import './FootballerList.css';

export function FootballerList() {
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

    if (loading) return <div className="loading">Loading footballers...</div>;
    if (error) return <div className="error">Error: {error}</div>;

    return (
        <div className="footballer-container">
            <h2>Footballer Profiles</h2>
            <div className="footballer-grid">
                {footballers.map(player => (
                    <div
                        key={player.id}
                        className={`footballer-card ${selectedPlayer?.id === player.id ? 'selected' : ''}`}
                        onClick={() => setSelectedPlayer(player)}
                    >
                        <div className="card-header">
                            <span className="overall-rating">{player.overallRating}</span>
                            <span className="position">{player.position}</span>
                        </div>
                        <div className="card-name">
                            {player.firstName} {player.lastName}
                        </div>
                        <div className="card-info">
                            <span>{player.nationality}</span>
                            <span>Age: {player.age}</span>
                        </div>
                    </div>
                ))}
            </div>

            {selectedPlayer && (
                <div className="player-detail">
                    <h3>{selectedPlayer.firstName} {selectedPlayer.lastName}</h3>
                    <div className="stats-grid">
                        <StatBar label="PAC" value={selectedPlayer.pace} />
                        <StatBar label="SHO" value={selectedPlayer.shooting} />
                        <StatBar label="PAS" value={selectedPlayer.passing} />
                        <StatBar label="DRI" value={selectedPlayer.dribbling} />
                        <StatBar label="DEF" value={selectedPlayer.defending} />
                        <StatBar label="PHY" value={selectedPlayer.physical} />
                    </div>
                </div>
            )}
        </div>
    );
}

function StatBar({ label, value }) {
    const getColor = (val) => {
        if (val >= 80) return '#4ade80';
        if (val >= 60) return '#facc15';
        return '#f87171';
    };

    return (
        <div className="stat-bar">
            <span className="stat-label">{label}</span>
            <div className="stat-track">
                <div
                    className="stat-fill"
                    style={{
                        width: `${value}%`,
                        backgroundColor: getColor(value)
                    }}
                />
            </div>
            <span className="stat-value">{value}</span>
        </div>
    );
}
