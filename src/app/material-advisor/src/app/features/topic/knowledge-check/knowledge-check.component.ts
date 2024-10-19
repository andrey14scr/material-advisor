import { Component, Inject } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { KnowledgeCheckService } from './services/knowledge-check.service';
import { Group } from './models/Group';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GroupService } from './services/group.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-knowledge-check',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './knowledge-check.component.html',
  styleUrl: './knowledge-check.component.scss'
})
export class KnowledgeCheckComponent {
  form: FormGroup;
  groups: Group[] = [];
  isLoading = true;

  constructor(
    private fb: FormBuilder,
    private knowledgeCheckService: KnowledgeCheckService,
    private groupService: GroupService,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<KnowledgeCheckComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: [''],
      maxAttempts: [''],
      time: [''],
      dropdown: [''],
    });
  }

  ngOnInit(): void {
    this.groupService.getGroups().subscribe({
      next: (groups) => {
        this.groups = groups;
        this.isLoading = false;
      },
      error: (error) => {
        this.snackBar.open('', 'Close', { duration: 2000 });
        this.isLoading = false;
      }
    });

    if (this.data && this.data.id) {
      this.knowledgeCheckService.getKnowledgeCheck(this.data.id).subscribe((item) => {
        this.form.patchValue(item);
      });
    }
  }

  onSave(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}