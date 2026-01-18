import type { components } from './types';

export type Footballer = components['schemas']['Footballer'];

export async function getFootballers(): Promise<Footballer[]> {
    const response = await fetch('/api/Footballer');
    if (!response.ok) {
        throw new Error(`Failed to fetch footballers: ${response.statusText}`);
    }
    return response.json();
}

export async function getFootballerById(id: number): Promise<Footballer> {
    const response = await fetch(`/api/Footballer/${id}`);
    if (!response.ok) {
        throw new Error(`Failed to fetch footballer: ${response.statusText}`);
    }
    return response.json();
}
