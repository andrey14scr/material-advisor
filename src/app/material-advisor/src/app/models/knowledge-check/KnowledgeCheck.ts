import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheck {
  id: GUID;
  topicId: GUID;
  name: string;
  description: string;
  startDate: Date;
  endDate?: Date;
  time: number;
  maxAttempts?: number;
  usedAttempts: number;
  groupIds: GUID[];
  passScore: number;
}