import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheckListItem {
  id: GUID;
  name: string;
  time: number;
  startDate: Date;
  endDate?: Date;
  usedAttempts: number;
  maxAttempts?: number;
  isSubmitted: boolean;
  isVerified: boolean;
  passScore: number;
  maxScore: number;
  score?: number;
}