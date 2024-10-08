import { Component } from '@angular/core';
import { TopicsService as TopicsService } from './services/topics.service';
import { NgFor, NgIf } from '@angular/common';
import { TopicListItemModel } from './models/TopicListItem.model';
import { TranslationService } from '@shared/services/translation.service';
import { map } from 'rxjs';
import { LanguageText } from '@shared/models/LanguageText';
import { AuthService } from '@shared/services/auth.service';
import { Router } from '@angular/router';
import { GUID } from '@shared/types/GUID';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { KnowledgeCheckListItemModel } from './models/KnowledgeCheckListItem.model';
import { TopicService } from '@features/topic/services/topic.service';
import { ConfirmDialogComponent } from '@shared/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [NgFor, NgIf, MatIconModule, MatButtonModule],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {
  topics: TopicListItemModel[] = [];
  currentTag: string | undefined;
  
  constructor(private translationService: TranslationService, 
    private topicsService: TopicsService,
    private topicService: TopicService,
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router) {}

  ngOnInit(): void {
    this.topicsService.getTopics()
      .pipe(
        map(data => 
          data.sort((a, b) => a.number - b.number)
            .map(item => ({
              ...item,
              knowledgeChecks: item.knowledgeChecks.sort((x, y) => x.number - y.number)
            }))
        )
      )
      .subscribe(
        data => this.setTopics(data)
      );
  }

  setTopics(data: TopicListItemModel[]): void {
    this.topics = data;
  }

  translate(key: string){
    return this.translationService.translate(key);
  }

  translateText(texts: LanguageText[]): string {
    return this.translationService.translateText(texts);
  }

  isOwner(model: TopicListItemModel): boolean {
    return this.authService.getCurrentUser()?.name === model.owner;
  }

  navigateToTopicDetails(topicId: GUID): void {
    this.router.navigate([`/topic/${topicId}`]);
  }

  navigateToKnowledgeCheckDetails(knowledgeCheckId: GUID): void {
    this.router.navigate([`/knowledge-check/${knowledgeCheckId}`]);
  }

  isKnowledgeCheckActive(model: KnowledgeCheckListItemModel): boolean {
    const now = new Date();
    return model.startDate >= now 
      && (model.endDate == undefined || model.endDate >= now) 
      && (model.maxAttempts == undefined || model.usedAttempts < model.maxAttempts);
  }

  deleteTopic(id: GUID): void {
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
