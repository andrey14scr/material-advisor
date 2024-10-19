import { LanguageText } from "@shared/models/LanguageText";
import { AnswerModel } from "./Answer";

export interface AnswerGroupModel {
  number: number;
  content: LanguageText[];
  answers: AnswerModel[];
}
