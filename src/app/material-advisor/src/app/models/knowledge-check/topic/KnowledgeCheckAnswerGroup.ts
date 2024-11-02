import { LanguageText } from "@shared/models/LanguageText";
import { KnowledgeCheckAnswer } from "./KnowledgeCheckAnswer";
import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheckAnswerGroup {
  id: GUID;
  number: number;
  content: LanguageText[];
  answers: KnowledgeCheckAnswer[];
}
