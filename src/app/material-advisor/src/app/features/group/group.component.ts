import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@shared/modules/matetial/material.module';
import { CommonModule } from '@angular/common';
import { Group } from '@models/user/Group';
import { GroupService } from '@services/group.service';
import { GroupCreateDialogComponent } from './components/group-create-dialog/group-create-dialog.component';
import { TranslationService } from '@shared/services/translation.service';

@Component({
  selector: 'group',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MaterialModule,
  ],
  templateUrl: './group.component.html',
  styleUrl: './group.component.scss'
})
export class GroupComponent implements OnInit {
  groupsAsOwner: Group[] = [];
  groupsAsMember: Group[] = [];
  public isCreateGroupDialogVisible = false;

  constructor(private groupService: GroupService, private translationService: TranslationService, private dialog: MatDialog) {}

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
    const dialogRef = this.dialog.open(GroupCreateDialogComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadGroups();
      }
    });
  }

  t(key: string): string {
    return this.translationService.translate(key);
  }
}