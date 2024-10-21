import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { GroupService } from '@services/group.service';
import { User } from '@shared/models/User';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { UserService } from '@shared/services/user.service';

@Component({
  selector: 'create-group',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MaterialModule,
  ],
  templateUrl: './create-group.component.html',
  styleUrl: './create-group.component.scss'
})
export class CreateGroupComponent implements OnInit {
  groupForm: FormGroup;
  users: User[] = [];
  pageNumber = 1;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreateGroupComponent>,
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
}
