import { LanguageText } from "@shared/models/LanguageText";
import { AnswerModel } from "./Answer.model";

export interface AnswerGroupModel {
  number: number;
  content: LanguageText[];
  answers: AnswerModel[];
}
