import { LanguageText } from "@shared/models/LanguageText";
import { QuestionType } from "@shared/types/QuestionEnum";
import { AnswerGroup } from "./AnswerGroup";

export interface Question {
  number: number;
  points: number;
  type: QuestionType;
  content: LanguageText[];
  answerGroups: AnswerGroup[];
}
