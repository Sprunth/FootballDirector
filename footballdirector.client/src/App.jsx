import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Layout } from './components/shared';
import { ClubOverview, Squad, Staff, Inbox } from './pages';

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route index element={<ClubOverview />} />
                    <Route path="squad" element={<Squad />} />
                    <Route path="staff" element={<Staff />} />
                    <Route path="inbox" element={<Inbox />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default App;
