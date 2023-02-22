import { Data } from "./Data";

export class Agenda {
    id: number = 0;
    nome?: string;
    data?: string;
    hora?: string;
    descricao?: string;
    local?: string;
    participantes?: string;
    criadoPor?: string;
    particular: boolean = false;
    editar: boolean = false
}


