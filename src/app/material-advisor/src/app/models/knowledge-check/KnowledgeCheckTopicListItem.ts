import { GUID } from "@shared/types/GUID";
import { GeneratedFile } from "./GeneratedFile";

export interface KnowledgeCheckTopicListItem {
  id: GUID;
  name: string;
  startDate: Date;
  endDate?: Date;
  attemptsToVerifyCount: number;
  dataCount?: number;
}