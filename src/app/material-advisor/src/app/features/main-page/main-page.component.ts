import { Component } from '@angular/core';
import { TopicsService as TopicsService } from './services/topics.service';
import { CommonModule } from '@angular/common';
import { TopicListItem } from './models/TopicListItem';
import { TranslationService } from '@shared/services/translation.service';
import { map } from 'rxjs';
import { LanguageText } from '@shared/models/LanguageText';
import { AuthService } from '@shared/services/auth.service';
import { Router } from '@angular/router';
import { GUID } from '@shared/types/GUID';
import { KnowledgeCheckListItem } from './models/KnowledgeCheckListItem';
import { TopicService } from '@features/topic/services/topic.service';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MaterialModule } from '@shared/modules/matetial/material.module';

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
    private topicsService: TopicsService,
    private topicService: TopicService,
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router) {}

  ngOnInit() {
    this.topicsService.getTopics()
      .pipe(
        map(data => 
          data.sort((a, b) => a.number - b.number)
            .map(item => ({
              ...item,
              knowledgeChecks: item.knowledgeChecks.sort((a, b) => new Date(b.startDate).getTime() - new Date(a.startDate).getTime())
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
}
