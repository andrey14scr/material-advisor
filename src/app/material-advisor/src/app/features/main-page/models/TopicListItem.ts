import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";
import { KnowledgeCheckListItem } from "./KnowledgeCheckListItem";

export interface TopicListItem {
  id: GUID;
  number: number;
  owner: string;
  name: LanguageText[];
  knowledgeChecks: KnowledgeCheckListItem[];
}
