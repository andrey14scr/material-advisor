import { GUID } from "@shared/types/GUID";

export interface User {
  id: GUID;
  name: string;
  email: string;
}