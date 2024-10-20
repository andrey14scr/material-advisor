import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheckListItem {
  id: GUID;
  name: string;
  time: number;
  startDate: Date;
  endDate: Date | undefined;
  usedAttempts: number;
  maxAttempts: number | undefined;
}
