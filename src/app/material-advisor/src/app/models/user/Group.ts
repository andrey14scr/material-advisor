import { User } from "@shared/models/User";
import { GUID } from "@shared/types/GUID";

export interface Group {
  id: GUID;
  name: string;
  users: User[];
}