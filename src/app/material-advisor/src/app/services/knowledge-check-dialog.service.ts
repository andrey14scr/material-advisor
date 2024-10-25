import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { KnowledgeCheckService } from './knowledge-check.service';
import { KnowledgeCheckCreateDialogComponent } from '@features/knowledge-check-create-dialog/knowledge-check-create-dialog.component';
import { removeEmptyField } from '@shared/services/object-utils.service';
import { sortByStartDate } from '@shared/services/sort-utils.service';
import { GUID } from '@shared/types/GUID';
import { KnowledgeCheckListItem } from '@models/knowledge-check/KnowledgeCheckListItem';

@Injectable({
  providedIn: 'root'
})
export class KnowledgeCheckDialogService {
  constructor(
    private dialog: MatDialog,
    private knowledgeCheckService: KnowledgeCheckService
  ) { }

  openKnowledgeCheckDialog(topicId: GUID, knowledgeChecks: KnowledgeCheckListItem[], id?: GUID) {
    const dialogRef = this.dialog.open(KnowledgeCheckCreateDialogComponent, {
      width: '600px',
      data: { id },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        result.topicId = topicId;
        removeEmptyField(result, 'id');
        this.knowledgeCheckService.postKnowledgeCheck(result).subscribe((knowledgeCheck) => {
          if (result.id) {
            const index = knowledgeChecks.findIndex(item => item.id === result.id);
            knowledgeChecks[index] = knowledgeCheck;
            knowledgeChecks = sortByStartDate(knowledgeChecks);
          }
          else {
            knowledgeChecks.push(knowledgeCheck);
          }
        });
      }
    });
  }
}
