import { Compartilhados } from "./Compartilhados";

export class AgendaPut {
    id: number = 0;
    nome?: string;
    data?: string;
    hora?: string;
    descricao?: string;
    local?: string;
    participantes?: string;
    criadoPor?: string;
    particular: boolean = true;
    editar: boolean = false
    compartilhados?: [Compartilhados]
}
