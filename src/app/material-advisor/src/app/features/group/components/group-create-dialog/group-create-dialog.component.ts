import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { GroupService } from '@services/group.service';
import { User } from '@shared/models/User';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { TranslationService } from '@shared/services/translation.service';
import { UserService } from '@shared/services/user.service';

@Component({
  selector: 'group-create-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MaterialModule,
  ],
  templateUrl: './group-create-dialog.component.html',
  styleUrl: './group-create-dialog.component.scss'
})
export class GroupCreateDialogComponent implements OnInit {
  groupForm: FormGroup;
  users: User[] = [];
  pageNumber = 1;

  constructor(
    private fb: FormBuilder,
    private translationService: TranslationService,
    private dialogRef: MatDialogRef<GroupCreateDialogComponent>,
    private userService: UserService,
    private groupService: GroupService
  ) {
    this.groupForm = this.fb.group({
      name: [''],
      members: [[]]
    });
  }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pageNumber, 100).subscribe((users) => {
      this.users = [...this.users, ...users];
    });
  }

  onScroll() {
    this.pageNumber++;
    this.loadUsers();
  }

  onCreate() {
    if (this.groupForm.valid) {
      const group = {
        name: this.groupForm.value.name,
        users: this.groupForm.value.members
      };
      this.groupService.postGroup(group).subscribe(() => {
        this.dialogRef.close(true);
      });
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

  t(key: string): string {
    return this.translationService.translate(key);
  }
}
