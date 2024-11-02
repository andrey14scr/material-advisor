import { GUID } from "@shared/types/GUID";

export interface KnowledgeCheckTopicListItem {
  id: GUID;
  name: string;
  startDate: Date;
  endDate?: Date;
}