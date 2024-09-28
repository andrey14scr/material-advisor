import { LanguageText } from "@shared/models/LanguageText";

export class AnswerModel {
  number: number;
  points: number;
  texts: LanguageText[];

  constructor(
    number: number,
    points: number,
    texts: LanguageText[]) {
    this.number = number;
    this.points = points;
    this.texts = texts;
  }
}
