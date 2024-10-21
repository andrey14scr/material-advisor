import { LanguageText } from "@shared/models/LanguageText";

export interface Answer {
  number: number;
  points: number;
  content: LanguageText[];
}
