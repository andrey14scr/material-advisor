import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";
import { KnowledgeCheckQuestion } from "./KnowledgeCheckQuestion";

export interface KnowledgeCheckTopic {
  id: GUID;
  name: LanguageText[];
  questions: KnowledgeCheckQuestion[];
}