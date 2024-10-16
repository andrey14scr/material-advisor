import { LanguageText } from "@shared/models/LanguageText";
import { GUID } from "@shared/types/GUID";
import { QuestionModel } from "./Question";

export interface TopicModel {
  id: GUID | null;
  version: number;
  name: LanguageText[];
  questions: QuestionModel[];
}