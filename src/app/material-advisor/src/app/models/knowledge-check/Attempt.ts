import { GUID } from "@shared/types/GUID";

export interface Attempt {
  id: GUID;
  startDate: Date;
  knowledgeCheckId: GUID;
}