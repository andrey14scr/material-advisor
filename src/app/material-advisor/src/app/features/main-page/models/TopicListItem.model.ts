import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";
import { KnowledgeCheckListItemModel } from "./KnowledgeCheckListItem.model";

export class TopicListItemModel {
    id: GUID;
    number: number;
    owner: string;
    name: LanguageText[];
    knowledgeChecks: KnowledgeCheckListItemModel[];

    constructor(id: GUID, number: number, owner: string, name: LanguageText[], knowledgeChecks: KnowledgeCheckListItemModel[]) {
        this.id = id;
        this.number = number;
        this.owner = owner;
        this.name = name;
        this.knowledgeChecks = knowledgeChecks;
    }
}
