import { LanguageText } from "@shared/models/LanguageText";
import { Answer } from "./Answer";

export interface AnswerGroup {
  number: number;
  isTechnical: boolean;
  content: LanguageText[];
  answers: Answer[];
}
