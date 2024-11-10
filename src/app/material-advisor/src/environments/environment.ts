import { LanguageEnum } from "@shared/types/LanguageEnum";
import { IEnvironment } from "./IEnvironment";

export const environment: IEnvironment = {
  production: false,
  apiUrl: 'https://localhost:7100',
  hashKey: 'some-secret-hash-key'
};