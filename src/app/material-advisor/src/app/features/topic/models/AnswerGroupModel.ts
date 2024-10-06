import { LanguageText } from "@shared/models/LanguageText";
import { AnswerModel } from "./AnswerModel";

export class AnswerGroupModel {
  number: number;
  content: LanguageText[];
  answers: AnswerModel[];

  constructor(
    number: number,
    content: LanguageText[],
    answers: AnswerModel[]) {
    this.number = number;
    this.content = content;
    this.answers = answers;
  }
}
