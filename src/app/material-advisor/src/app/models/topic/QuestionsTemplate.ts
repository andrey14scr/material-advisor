import { QuestionType } from "@shared/types/QuestionEnum";

export interface QuestionsTemplate {
  count: number;
  type: QuestionType;
  answersCount?: number;
}