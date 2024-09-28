import { LanguageEnum } from "./LanguageEnum";

export interface Language {
  languageId: LanguageEnum;
  name: string;
  code: string;
  flag: string;
}
