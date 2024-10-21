import { LanguageText } from "@shared/models/LanguageText";
import { Answer } from "./Answer";

export interface AnswerGroup {
  number: number;
  content: LanguageText[];
  answers: Answer[];
}
