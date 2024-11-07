import { GUID } from "@shared/types/GUID";

export interface VerifiedAIAnswer {
  answerGroupId: GUID;
  attemptId: GUID;
  score: number;
  comment?: string;
}