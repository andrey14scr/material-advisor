import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";
import { Question } from "./Question";

export interface Topic {
  id: GUID | null;
  version: number;
  name: LanguageText[];
  questions: Question[];
}