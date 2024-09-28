import { LanguageText } from "@shared/models/LanguageText";
import { AnswerModel } from "./AnswerModel";

export class AnswerGroupModel {
  number: number;
  texts: LanguageText[];
  answers: AnswerModel[];

  constructor(
    number: number,
    texts: LanguageText[],
    answers: AnswerModel[]) {
    this.number = number;
    this.texts = texts;
    this.answers = answers;
  }
}
