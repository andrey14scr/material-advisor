import { LanguageText } from "@shared/models/LanguageText";
import { QuestionType } from "@shared/types/QuestionEnum";
import { KnowledgeCheckAnswerGroup } from "./KnowledgeCheckAnswerGroup";
import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheckQuestion {
  id: GUID;
  number: number;
  type: QuestionType;
  content: LanguageText[];
  answerGroups: KnowledgeCheckAnswerGroup[];
}
