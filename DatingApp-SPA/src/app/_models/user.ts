import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    gender: string;
    city: string;
    country: string;
    age: number;
    knownAs: string;
    lastActive: Date;
    photoURL: string;

    lookingFor?: string;
    introduction?: string;
    interests?: string;
    photos?: Photo[];
}
