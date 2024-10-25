import { LanguageText } from "@shared/models/LanguageText";

export interface Answer {
  number: number;
  isRight: boolean;
  points: number;
  content: LanguageText[];
}
