import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";
import { QuestionModel } from "./QuestionModel";

export class TopicModel {
  id: GUID | null;
  number: number;
  version: number;
  texts: LanguageText[];
  questions: QuestionModel[];

  constructor(
    id: GUID | null, 
    number: number, 
    version: number, 
    texts: LanguageText[], 
    questions: QuestionModel[]) {
      this.id = id;
      this.number = number;
      this.version = version;
      this.texts = texts;
      this.questions = questions;
  }
}