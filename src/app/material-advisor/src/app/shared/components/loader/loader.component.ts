import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MaterialModule } from '@shared/modules/matetial/material.module';

@Component({
  selector: 'loader',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.scss'
})
export class LoaderComponent {
  @Input() isLoading: boolean = false;
  @Input() overlay: boolean = true;
}
