import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MaterialModule } from '@shared/modules/matetial/material.module';

@Component({
  selector: 'file-input',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './file-input.component.html',
  styleUrl: './file-input.component.scss'
})
export class FileInputComponent {
  isDragging = false;
  fileList: File[] = [];
  @Input() isMultiple: boolean = false;
  @Output() change = new EventEmitter<Event>();

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      this.addFiles(event.dataTransfer.files);
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.addFiles(input.files);
    }
    this.change.emit(event);
  }

  addFiles(files: FileList) {
    for (let i = 0; i < files.length; i++) {
      this.fileList.push(files[i]);
    }
  }

  removeFile(index: number) {
    this.fileList.splice(index, 1);
  }
}
