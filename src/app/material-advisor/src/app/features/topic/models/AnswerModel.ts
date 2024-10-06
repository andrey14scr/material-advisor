import { LanguageText } from "@shared/models/LanguageText";

export class AnswerModel {
  number: number;
  points: number;
  content: LanguageText[];

  constructor(
    number: number,
    points: number,
    content: LanguageText[]) {
    this.number = number;
    this.points = points;
    this.content = content;
  }
}
