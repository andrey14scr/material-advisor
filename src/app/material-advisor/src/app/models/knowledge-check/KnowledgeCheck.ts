import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheck {
  id: GUID;
  topicId: GUID;
  name: string;
  description: string;
  startDate: Date;
  endDate: Date | null;
  time: number;
  maxAttempts: number | null;
  usedAttempts: number;
  groupIds: GUID[];
}