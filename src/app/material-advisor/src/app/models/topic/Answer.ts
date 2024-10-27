import { LanguageText } from "@shared/models/LanguageText";

export interface Answer {
  number: number;
  isCorrect: boolean;
  points: number;
  content: LanguageText[];
}
