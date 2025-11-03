import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { TaskListComponent } from './features/tasks/task-list/task-list.component';
import { TaskFormComponent } from './features/tasks/task-form/task-form.component';
import { Task } from './models/task';
import * as bootstrap from 'bootstrap';


@Component({
  selector: 'app-root',
  imports: [TaskListComponent, TaskFormComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements AfterViewInit {
  selected: Task | null = null;
  private modalInstance!: bootstrap.Modal;

  @ViewChild('taskFormModal') modalRef!: ElementRef<HTMLElement>;
  @ViewChild(TaskListComponent) taskList!: TaskListComponent;

  ngAfterViewInit() {
    this.modalInstance = new bootstrap.Modal(this.modalRef.nativeElement);
  }

  onEditRequest(task: Task) {
    this.selected = task;
    this.openModal();
  }

  onAddRequest() {
    this.selected = null;
    this.openModal();
  }

  onSaved() {
    this.modalInstance.hide();
    this.selected = null;  
     this.taskList.load();  
  }

  onCancel() {
    this.modalInstance.hide();
    this.selected = null;
  }

  private openModal() {
    this.modalInstance.show();
  }
}
