// Centralized API functions for all endpoints

// Club
export async function getClub() {
    const response = await fetch('/api/club');
    if (!response.ok) throw new Error(`Failed to fetch club: ${response.statusText}`);
    return response.json();
}

// Footballers
export async function getFootballers() {
    const response = await fetch('/api/footballers');
    if (!response.ok) throw new Error(`Failed to fetch footballers: ${response.statusText}`);
    return response.json();
}

export async function getFootballerById(id) {
    const response = await fetch(`/api/footballers/${id}`);
    if (!response.ok) throw new Error(`Failed to fetch footballer: ${response.statusText}`);
    return response.json();
}

// Staff
export async function getStaff(role = null) {
    const url = role ? `/api/staff?role=${role}` : '/api/staff';
    const response = await fetch(url);
    if (!response.ok) throw new Error(`Failed to fetch staff: ${response.statusText}`);
    return response.json();
}

export async function getStaffById(id) {
    const response = await fetch(`/api/staff/${id}`);
    if (!response.ok) throw new Error(`Failed to fetch staff member: ${response.statusText}`);
    return response.json();
}

// Conversations
export async function getInbox() {
    const response = await fetch('/api/inbox');
    if (!response.ok) throw new Error(`Failed to fetch inbox: ${response.statusText}`);
    return response.json();
}

export async function getConversation(id) {
    const response = await fetch(`/api/conversation/${id}`);
    if (!response.ok) throw new Error(`Failed to fetch conversation: ${response.statusText}`);
    return response.json();
}

export async function getPersonConversations(personId) {
    const response = await fetch(`/api/person/${personId}/conversations`);
    if (!response.ok) throw new Error(`Failed to fetch conversations: ${response.statusText}`);
    return response.json();
}
