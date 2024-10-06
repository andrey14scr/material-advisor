import { LanguageText } from "@shared/models/LanguageText";
import { QuestionEnum } from "@shared/types/QuestionEnum";
import { AnswerGroupModel } from "./AnswerGroupModel";


export class QuestionModel {
  number: number;
  points: number;
  type: QuestionEnum;
  content: LanguageText[];
  answerGroups: AnswerGroupModel[];

  constructor(
    number: number,
    points: number,
    type: QuestionEnum,
    content: LanguageText[],
    answerGroups: AnswerGroupModel[]) {
    this.number = number;
    this.points = points;
    this.type = type;
    this.content = content;
    this.answerGroups = answerGroups;
  }
}
