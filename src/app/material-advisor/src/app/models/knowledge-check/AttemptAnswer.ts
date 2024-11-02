import { GUID } from "@shared/types/GUID";

export interface AttemptAnswer {
  answerGroupId: GUID;
  value?: string;
}