import { GUID } from "@shared/types/GUID";

export interface VerifiedAnswer {
  answerGroupId: GUID;
  attemptId: GUID;
  score: number;
  comment?: string;
}