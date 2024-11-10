import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";

export interface TopicListItem<TKnowledgeCheck> {
  id: GUID;
  version: number;
  owner: string;
  name: LanguageText[];
  knowledgeChecks: TKnowledgeCheck[];
  generatedAt?: Date;
}