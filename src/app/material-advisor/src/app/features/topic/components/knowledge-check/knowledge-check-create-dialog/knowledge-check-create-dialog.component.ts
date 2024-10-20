import { Component, Inject } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Group } from './models/Group';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GroupService } from './services/group.service';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { KnowledgeCheckService } from '../services/knowledge-check.service';

@Component({
  selector: 'knowledge-check-create-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './knowledge-check-create-dialog.component.html',
  styleUrl: './knowledge-check-create-dialog.component.scss'
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
      endDate: [null],
      maxAttempts: [''],
      time: [''],
      groups: [[]],
    });
  }

  ngOnInit() {
    this.groupService.getGroupsAsOwner().subscribe({
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

  onSave() {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}