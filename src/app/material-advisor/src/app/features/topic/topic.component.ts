import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-topic',
  standalone: true,
  imports: [],
  templateUrl: './topic.component.html',
  styleUrl: './topic.component.scss'
})
export class TopicComponent implements OnInit {
  model: any;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    const topicId = this.route.snapshot.paramMap.get('id');
  }
}