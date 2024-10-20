import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { GUID } from '@shared/types/GUID';
import { KnowledgeCheck } from './models/KnowledgeCheck';
import { KnowledgeCheckService } from './services/knowledge-check.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'knowledge-check',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './knowledge-check.component.html',
  styleUrl: './knowledge-check.component.scss'
})
export class KnowledgeCheckComponent implements OnInit {
  @Input() topicId!: GUID | null;
  isLoading: boolean = true;
  knowledgeChecks: KnowledgeCheck[] = [];

  constructor(
    private dialog: MatDialog,
    private knowledgeCheckService: KnowledgeCheckService
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

  openKnowledgeCheckDialog(id?: number) {
    const dialogRef = this.dialog.open(KnowledgeCheckComponent, {
      width: '600px',
      data: { id },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        result.topicId = this.topicId;
        this.knowledgeCheckService.postKnowledgeCheck(result).subscribe((knowledgeCheck) => {
          this.knowledgeChecks.push(knowledgeCheck);
        });
      }
    });
  }

  editKnowledgeCheck(item: any) {
    this.openKnowledgeCheckDialog(item.id);
  }
}