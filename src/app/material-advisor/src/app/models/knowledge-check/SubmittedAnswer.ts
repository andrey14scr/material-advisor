import { GUID } from "@shared/types/GUID";
import { AttemptAnswer } from "./AttemptAnswer";

export interface SubmittedAnswer extends AttemptAnswer {
  attemptId: GUID;
}