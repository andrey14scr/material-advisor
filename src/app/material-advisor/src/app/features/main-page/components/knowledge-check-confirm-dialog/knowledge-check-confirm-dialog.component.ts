import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { KnowledgeCheckListItem } from '@models/knowledge-check/KnowledgeCheckListItem';
import { TopicListItem } from '@models/topic/TopicListItem';
import { LanguageText } from '@shared/models/LanguageText';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { TranslationService } from '@shared/services/translation.service';

@Component({
  selector: 'app-knowledge-check-confirm-dialog',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './knowledge-check-confirm-dialog.component.html',
  styleUrl: './knowledge-check-confirm-dialog.component.scss'
})
export class KnowledgeCheckConfirmDialogComponent {
  constructor(
    private translationService: TranslationService,
    public dialogRef: MatDialogRef<KnowledgeCheckConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { topic: TopicListItem, knowledgeCheck: KnowledgeCheckListItem }
  ) {
    console.log(data);
  }

  translateText(texts: LanguageText[]): string {
    return this.translationService.translateText(texts);
  }

  onAccept(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
