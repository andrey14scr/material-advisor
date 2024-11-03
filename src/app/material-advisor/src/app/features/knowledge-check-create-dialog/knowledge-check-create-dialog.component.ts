import { Component, Inject } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule, DatePipe } from '@angular/common';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { LoaderComponent } from "@shared/components/loader/loader.component";
import { KnowledgeCheckService } from '@services/knowledge-check.service';
import { GroupService } from '@services/group.service';
import { Group } from '@models/user/Group';
import { DateAdapter, MAT_DATE_FORMATS, MAT_NATIVE_DATE_FORMATS, NativeDateAdapter } from '@angular/material/core';
import {NgxMatTimepickerModule} from 'ngx-mat-timepicker';

@Component({
  selector: 'knowledge-check-create-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    LoaderComponent,
    NgxMatTimepickerModule,
  ],
  providers: [ 
    {provide: DateAdapter, useClass: NativeDateAdapter}, 
    {provide: MAT_DATE_FORMATS, useValue: MAT_NATIVE_DATE_FORMATS},
    DatePipe
  ],
  templateUrl: './knowledge-check-create-dialog.component.html',
  styleUrl: './knowledge-check-create-dialog.component.scss'
})
export class KnowledgeCheckCreateDialogComponent {
  form: FormGroup;
  groups: Group[] = [];
  isLoading = true;

  constructor(
    private fb: FormBuilder,
    private knowledgeCheckService: KnowledgeCheckService,
    private groupService: GroupService,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<KnowledgeCheckCreateDialogComponent>,
    private datePipe: DatePipe,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.form = this.fb.group({
      id: [null],
      name: ['', Validators.required],
      description: ['', Validators.required],
      startDate: [new Date(), Validators.required],
      startTime: ['00:00', Validators.required],
      endDate: [null],
      endTime: [null],
      maxAttempts: [''],
      time: [''],
      groupIds: [[]],
      passScore: [''],
      isAttemptOverrided: [false],
      isManualOnlyVerification: [false],
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
        this.form.patchValue({
          startTime: this.datePipe.transform(item.startDate, 'HH:mm'),
          endTime: item.endDate ? this.datePipe.transform(item.endDate, 'HH:mm') : '',
        });
        this.isLoading = false;
      });
    }
    else {
      this.isLoading = false;
    }
  }

  onSave() {
    if (this.form.valid) {
      const [startTimeHours, startTimeMinutes] = this.form.value.startTime.split(':').map(Number);
      
      const startDate = new Date(this.form.value.startDate);
      startDate.setHours(startTimeHours, startTimeMinutes);
      
      let endDate: Date | null = null;
      
      if (this.form.value.endDate && this.form.value.endTime) {
        const [endTimeHours, endTimeMinutes] = this.form.value.endTime.split(':').map(Number);
        endDate = new Date(this.form.value.endDate);
        endDate.setHours(endTimeHours, endTimeMinutes);
      }

      const form = {
        name: this.form.value.name,
        description: this.form.value.description,
        startDate: startDate,
        endDate: endDate,
        maxAttempts: this.form.value.maxAttempts,
        time: this.form.value.time,
        groupIds: this.form.value.groupIds,
        passScore: this.form.value.passScore,
        isAttemptOverrided: this.form.value.isAttemptOverrided,
        isManualOnlyVerification: this.form.value.isManualOnlyVerification,
      };

      if (this.data.id) {
        const formWithId = {...form, id: this.data.id};
        this.dialogRef.close(formWithId);
      }
      else {
        this.dialogRef.close(form);
      }
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}