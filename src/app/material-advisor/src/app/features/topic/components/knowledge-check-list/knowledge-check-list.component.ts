import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { GUID } from '@shared/types/GUID';
import { KnowledgeCheckListItem } from '@models/knowledge-check/KnowledgeCheckListItem';
import { KnowledgeCheckService } from '@services/knowledge-check.service';
import { KnowledgeCheckDialogService } from '@services/knowledge-check-dialog.service';

@Component({
  selector: 'knowledge-check-list',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './knowledge-check-list.component.html',
  styleUrl: './knowledge-check-list.component.scss'
})
export class KnowledgeChecksComponent implements OnInit {
  @Input() topicId!: GUID;
  isLoading: boolean = true;
  knowledgeChecks: KnowledgeCheckListItem[] = [];

  constructor(
    private knowledgeCheckService: KnowledgeCheckService,
    private knowledgeCheckDialogService: KnowledgeCheckDialogService,
  ) { }

  ngOnInit(): void {
    if (this.topicId) {
      this.getKnowledgeChecksByTopicId(this.topicId);
    }
    else {
      this.isLoading = false;
    }
  }

  getKnowledgeChecksByTopicId(topicId: string) {
    this.knowledgeCheckService.getByTopicId(topicId).subscribe({
      next: (data) => {
        this.knowledgeChecks = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching knowledge checks', error);
      }
    });
  }

  openKnowledgeCheckDialog(id?: GUID) {
    this.knowledgeCheckDialogService.openKnowledgeCheckDialog(this.topicId, this.knowledgeChecks, id);
  }
}