import { LanguageEnum } from "@shared/types/LanguageEnum";

export class LanguageText {
    text: string;
    languageId: LanguageEnum;

    constructor(text: string, languageId: LanguageEnum) {
        this.text = text;
        this.languageId = languageId;
    }
}

