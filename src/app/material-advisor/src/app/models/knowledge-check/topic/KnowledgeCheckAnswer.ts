import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheckAnswer {
  id: GUID;
  number: number;
  content: LanguageText[];
}
