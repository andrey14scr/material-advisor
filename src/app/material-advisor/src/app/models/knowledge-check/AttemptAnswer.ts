import { GUID } from "@shared/types/GUID";

export interface AttemptAnswer {
  answerGroupId: GUID;
  values: string[];
}