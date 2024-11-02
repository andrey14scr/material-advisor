import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslationService } from '@shared/services/translation.service';
import { Router } from 'express';

@Component({
  selector: 'answers-verification',
  standalone: true,
  imports: [],
  templateUrl: './answers-verification.component.html',
  styleUrl: './answers-verification.component.scss'
})
export class AnswersVerificationComponent implements OnInit {
  private route = inject(ActivatedRoute);
  
  constructor(
    private router: Router,
    private translationService: TranslationService,
  ){ }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
  }
  
}
