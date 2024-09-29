import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { TopicModel } from './models/Topic.model';
import { MatCardModule } from '@angular/material/card';
import { TopicService } from './services/topic.service';
import { TextsInputComponent } from "@shared/components/texts-input/texts-input.component";
import { QuestionsInputComponent } from './components/questions-input/questions-input.component';

@Component({
  selector: 'topic',  
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCardModule,
    TextsInputComponent,
    QuestionsInputComponent
  ],
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.scss']
})
export class TopicComponent implements OnInit {
  currentTopic: TopicModel = new TopicModel(null, 0, 0, [], []);
  topicForm: FormGroup;
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);

  constructor(private topicService: TopicService, private router: Router) {
    this.topicForm = this.fb.group({
      number: [this.currentTopic.number, [Validators.required, Validators.min(1)]],
      texts: this.fb.array([]),
      questions: this.fb.array([]),
    });

    this.topicForm.valueChanges.subscribe((formValues) => {
      this.currentTopic.number = formValues.number;
      this.currentTopic.texts = formValues.texts;
      this.currentTopic.questions = formValues.questions;
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.getTopicById(id);
    }
  }

  get textsFormArray() {
    return this.topicForm.get('texts') as FormArray;
  }

  get questionsFormArray() {
    return this.topicForm.get('questions') as FormArray;
  }

  getTopicById(id: string): void {
    this.topicService.getTopic(id).subscribe({
      next: (data) => {
        this.currentTopic = data;
        this.topicForm.patchValue({
          number: this.currentTopic.number,
          texts: this.currentTopic.texts,
        });
      },
      error: (error) => {
        console.error('Error fetching the topic', error);
      }
    });
  }

  onSubmit(): void {
    if (this.topicForm.invalid) {
      return;
    }

    this.currentTopic.number = this.topicForm.value.number;

    const { id, ...withoutId } = this.currentTopic;

    const body = this.currentTopic.id ? this.currentTopic : withoutId;

    console.log(body);

    // this.topicService.postTopic(body).subscribe({
    //   next: () => {
    //     this.router.navigate(['/main-page']);
    //   },
    //   error: (error) => {
    //     console.error('Error updating topic', error);
    //   }
    // });
  }
}