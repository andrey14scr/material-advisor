import { Component, Inject } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { LoaderComponent } from "@shared/components/loader/loader.component";
import { KnowledgeCheckService } from '@services/knowledge-check.service';
import { GroupService } from '@services/group.service';
import { Group } from '@models/user/Group';
import { DateAdapter, MAT_DATE_FORMATS, MAT_NATIVE_DATE_FORMATS, NativeDateAdapter } from '@angular/material/core';

@Component({
  selector: 'knowledge-check-create-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    LoaderComponent,
  ],
  providers: [ {provide: DateAdapter, useClass: NativeDateAdapter}, {provide: MAT_DATE_FORMATS, useValue: MAT_NATIVE_DATE_FORMATS}, ],
  templateUrl: './knowledge-check-create-dialog.component.html',
  styleUrl: './knowledge-check-create-dialog.component.scss'
})
export class KnowledgeCheckComponentCreateDialog {
  form: FormGroup;
  groups: Group[] = [];
  isLoading = true;

  constructor(
    private fb: FormBuilder,
    private knowledgeCheckService: KnowledgeCheckService,
    private groupService: GroupService,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<KnowledgeCheckComponentCreateDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.form = this.fb.group({
      id: [null],
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
      },
      error: (error) => {
        this.snackBar.open('', 'Close', { duration: 2000 });
        this.isLoading = false;
      }
    });

    if (this.data && this.data.id) {
      this.knowledgeCheckService.getKnowledgeCheck(this.data.id).subscribe((item) => {
        this.form.patchValue(item);
        this.isLoading = false;
      });
    }
    else {
      this.isLoading = false;
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