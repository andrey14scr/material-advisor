import { KnowledgeCheckListItem } from "@models/knowledge-check/KnowledgeCheckListItem";
import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";

export interface TopicListItem {
  id: GUID;
  version: number;
  number: number;
  owner: string;
  name: LanguageText[];
  knowledgeChecks: KnowledgeCheckListItem[];
}
