import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslationService } from '@shared/services/translation.service';
import { map } from 'rxjs';
import { LanguageText } from '@shared/models/LanguageText';
import { AuthService } from '@shared/services/auth.service';
import { Router } from '@angular/router';
import { GUID } from '@shared/types/GUID';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { TopicListItem } from '@models/topic/TopicListItem';
import { TopicService } from '@services/topic.service';
import { KnowledgeCheckListItem } from '@models/knowledge-check/KnowledgeCheckListItem';
import { sortByStartDate } from '@shared/services/sort-utils.service';
import { KnowledgeCheckDialogService } from '@services/knowledge-check-dialog.service';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {
  topics: TopicListItem[] = [];
  currentTag: string | undefined;
  
  constructor(private translationService: TranslationService, 
    private topicService: TopicService,
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router,
    private knowledgeCheckDialogService: KnowledgeCheckDialogService,
  ) {}

  ngOnInit() {
    this.topicService.getTopics()
      .pipe(
        map(data => 
          data.sort((a, b) => a.number - b.number)
            .map(item => ({
              ...item,
              knowledgeChecks: sortByStartDate(item.knowledgeChecks)
            }))
        )
      )
      .subscribe(
        data => this.setTopics(data)
      );
  }

  setTopics(data: TopicListItem[]) {
    this.topics = data;
  }

  translate(key: string){
    return this.translationService.translate(key);
  }

  translateText(texts: LanguageText[]): string {
    return this.translationService.translateText(texts);
  }

  isOwner(model: TopicListItem): boolean {
    return this.authService.getCurrentUser()?.name === model.owner;
  }

  navigateToTopicDetails(topicId: GUID) {
    this.router.navigate([`/topic/${topicId}`]);
  }

  navigateToKnowledgeCheckDetails(knowledgeCheckId: GUID) {
    this.router.navigate([`/knowledge-check/${knowledgeCheckId}`]);
  }

  isKnowledgeCheckActive(model: KnowledgeCheckListItem): boolean {
    const now = new Date();
    return model.startDate >= now 
      && (model.endDate == undefined || model.endDate >= now) 
      && (model.maxAttempts == undefined || model.usedAttempts < model.maxAttempts);
  }

  deleteTopic(id: GUID) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Are you sure you want to proceed?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.topicService.deleteTopic(id).subscribe({
          next: (response) => {
            if (response) {
              this.topics = this.topics.filter(topic => topic.id !== id);
            }
            else {
              console.error('Topic was not deleted');
            }
          },
          error: (error) => {
            console.error('Error deleting topic', error);
          }
        });
      }
    });
  }

  openKnowledgeCheckDialog(topic: TopicListItem, id?: GUID) {
    this.knowledgeCheckDialogService.openKnowledgeCheckDialog(topic.id, topic.knowledgeChecks, id);
  }
}
