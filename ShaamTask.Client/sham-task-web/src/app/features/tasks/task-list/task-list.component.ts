import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, Priority, Status } from '../../../models/task';
import { TasksService } from '../tasks.service';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  @Output() edit = new EventEmitter<Task>();
  @Output() deleted = new EventEmitter<void>();
  
  tasks: Task[] = [];
  loading = false;

  priorityLabel: Record<string, string> = {
    Low: 'נמוכה',
    Medium: 'בינונית',
    High: 'גבוהה'
  };

  statusLabel: Record<string, string> = {
    Pending: 'ממתינה',
    InProgress: 'בתהליך',
    Done: 'הושלמה',
    Completed: 'הושלמה'
  };

  constructor(private api: TasksService) { }

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.api.getAll().subscribe({
      next: data => { this.tasks = data ?? []; this.loading = false; },
      error: _ => { this.loading = false; }
    });
  }

  onEdit(t: Task): void { this.edit.emit(t); }

  onDelete(id?: number): void {
    if (!id) return;
    if (!confirm('למחוק את המשימה?')) return;
    this.api.delete(id).subscribe({
      next: () => { this.load(); this.deleted.emit(); },
      error: _ => { }
    });
  }

  priorityClass(p: Priority | string | undefined): string {
    switch (String(p)) {
      case 'Low': return 'text-bg-success';
      case 'Medium': return 'text-bg-warning';
      case 'High': return 'text-bg-danger';
      default: return 'text-bg-secondary';
    }
  }

  statusClass(s: Status | string | undefined): string {
    switch (String(s)) {
      case 'Pending': return 'text-bg-secondary';
      case 'InProgress': return 'text-bg-warning';
      case 'Done':
      case 'Completed': return 'text-bg-success';
      default: return 'text-bg-light';
    }
  }

  statusText(s: Status | string | undefined): string {
    const key = String(s);
    return this.statusLabel[key] ?? key ?? '—';
  }

  priorityText(p: Priority | string | undefined): string {
    const key = String(p);
    return this.priorityLabel[key] ?? key ?? '—';
  }
  openForm(task: Task | null) {
    this.edit.emit(task ? { ...task } : undefined);
  }  
}
