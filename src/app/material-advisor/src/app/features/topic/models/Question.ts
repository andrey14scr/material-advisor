import { LanguageText } from "@shared/models/LanguageText";
import { QuestionType } from "@shared/types/QuestionEnum";
import { AnswerGroupModel } from "./AnswerGroup";

export interface QuestionModel {
  number: number;
  points: number;
  type: QuestionType;
  content: LanguageText[];
  answerGroups: AnswerGroupModel[];
}
