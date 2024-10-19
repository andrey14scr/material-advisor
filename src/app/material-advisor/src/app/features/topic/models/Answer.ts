import { LanguageText } from "@shared/models/LanguageText";

export interface AnswerModel {
  number: number;
  points: number;
  content: LanguageText[];
}
