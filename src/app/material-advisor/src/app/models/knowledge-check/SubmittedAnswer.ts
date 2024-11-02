import { GUID } from "@shared/types/GUID";

export interface SubmittedAnswer {
  attemptId: GUID;
  answerGroupId: GUID;
  value?: string;
}