import { GUID } from "@shared/types/GUID";

export class KnowledgeCheckListItemModel {
    id: GUID;
    name: string;
    number: number;
    time: number;
    startDate: Date;
    endDate: Date | undefined;
    usedAttempts: number;
    maxAttempts: number | undefined;

    constructor(id: GUID,
        name: string,
        number: number,
        time: number,
        startDate: Date,
        endDate: Date | undefined,
        usedAttempts: number,
        maxAttempts: number | undefined) {
        this.id = id;
        this.name = name;
        this.number = number;
        this.time = time;
        this.startDate = startDate;
        this.endDate = endDate;
        this.usedAttempts = usedAttempts;
        this.maxAttempts = maxAttempts;
    }
}
