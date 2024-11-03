import { LanguageText } from "@shared/models/LanguageText";
import { KnowledgeCheckAnswer } from "./KnowledgeCheckAnswer";
import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheckAnswerGroup {
  id: GUID;
  number: number;
  isTechnical: boolean;
  content: LanguageText[];
  answers: KnowledgeCheckAnswer[];
}
