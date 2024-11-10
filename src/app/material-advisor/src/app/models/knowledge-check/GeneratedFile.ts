import { GUID } from "@shared/types/GUID";

export interface GeneratedFile {
  id: GUID;
  file?: string;
  fileName?: string;
  generatedAt: Date;
}