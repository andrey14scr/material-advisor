import { Component, OnInit } from '@angular/core';
import { Group } from './models/Group';
import { GroupService } from './services/group.service';
import { MatDialog } from '@angular/material/dialog';
import { CreateGroupComponent } from './components/create-group/create-group.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'group',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MaterialModule,
    CreateGroupComponent
],
  templateUrl: './group.component.html',
  styleUrl: './group.component.scss'
})
export class GroupComponent implements OnInit {
  groupsAsOwner: Group[] = [];
  groupsAsMember: Group[] = [];
  public isCreateGroupDialogVisible = false;

  constructor(private groupService: GroupService, private dialog: MatDialog) {}

  ngOnInit() {
    this.loadGroups();
  }

  loadGroups() {
    this.groupService.getGroupsAsOwner().subscribe((groups) => {
      this.groupsAsOwner = groups;
    });

    this.groupService.getGroupsAsMember().subscribe((groups) => {
      this.groupsAsMember = groups;
    });
  }

  openCreateGroupDialog() {
    const dialogRef = this.dialog.open(CreateGroupComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadGroups();
      }
    });
  }
}