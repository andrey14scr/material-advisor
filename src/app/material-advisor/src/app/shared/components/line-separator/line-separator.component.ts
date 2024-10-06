import { Component, Input } from '@angular/core';

@Component({
  selector: 'line-separator',
  standalone: true,
  imports: [],
  templateUrl: './line-separator.component.html',
  styleUrl: './line-separator.component.scss'
})
export class LineSeparatorComponent {
  @Input() labelText!: string;
}
