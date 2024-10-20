import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheck {
  id: GUID | null;
  topicId: GUID | null;
  name: string;
  description: string;
  startDate: Date;
  endDate: Date;
  time: number;
  maxAttempts: number;
  groupIds: GUID[];
}