import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TasksService } from '../tasks.service';
import { Priority, Task, Status } from '../../../models/task';

type TaskFormModel = {
  id: FormControl<number | null>;
  title: FormControl<string>;
  description: FormControl<string>;
  priority: FormControl<Priority>;
  dueDate: FormControl<string>; // yyyy-MM-dd
  status: FormControl<Status>;
};

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss'],
})
export class TaskFormComponent implements OnInit, OnChanges {
  private fb = inject(FormBuilder);
  private api = inject(TasksService);

  @Input() editTask: Task | null = null;
  @Output() saved = new EventEmitter<void>();
  @Output() cancelEdit = new EventEmitter<void>();

  submitted = false;

  priorityLabel: Record<Priority, string> = { Low: 'נמוכה', Medium: 'בינונית', High: 'גבוהה' };
  statusLabel: Record<Status, string> = { Pending: 'ממתינה', InProgress: 'בתהליך', Completed: 'הושלמה' };

  priorities: Priority[] = ['Low', 'Medium', 'High'];
  statuses: Status[] = ['Pending', 'InProgress', 'Completed'];

  form: FormGroup<TaskFormModel> = this.fb.group<TaskFormModel>({
    id: this.fb.control<number | null>(null),
    title: this.fb.nonNullable.control('', { validators: [Validators.required] }),
    description: this.fb.nonNullable.control(''),
    priority: this.fb.nonNullable.control<Priority>('Medium', { validators: [Validators.required] }),
    dueDate: this.fb.nonNullable.control('', { validators: [Validators.required, Validators.pattern(/^\d{4}-\d{2}-\d{2}$/)] }),
    status: this.fb.nonNullable.control<Status>('Pending', { validators: [Validators.required] }),
  }, { updateOn: 'submit' });

  get c() { return this.form.controls; }
  get isEdit() { return !!this.c.id.value && !!this.editTask; }

  ngOnInit(): void {
    if (this.editTask) this.patchFromTask(this.editTask);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['editTask']) {
      this.submitted = false;
      if (this.editTask) this.patchFromTask(this.editTask);
      else this.resetForm();
    }
  }

  private toDateInput(value?: string | null): string {
    if (!value) return '';
    const d = new Date(value);
    if (isNaN(d.getTime())) return '';
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  private patchFromTask(t: Task) {
    this.form.patchValue({
      id: t.id ?? null,
      title: t.title ?? '',
      description: t.description ?? '',
      priority: (t.priority as Priority) ?? 'Medium',
      dueDate: this.toDateInput(t.dueDate ?? ''),
      status: (t.status as Status) ?? 'Pending',
    });
  }

  private resetForm() {
    this.form.reset({
      id: null,
      title: '',
      description: '',
      priority: 'Medium',
      dueDate: '',
      status: 'Pending',
    });
  }

  submit(): void {
    this.submitted = true;
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    const payload: Task = {
      id: this.c.id.value!,
      title: this.c.title.value,
      description: this.c.description.value || undefined,
      priority: this.c.priority.value,
      status: this.c.status.value,
      dueDate: this.c.dueDate.value,
    };

    const req$ = this.isEdit ? this.api.update(payload.id, payload) : this.api.add(payload);
    req$.subscribe({
      next: () => {
        this.saved.emit();
        if (this.isEdit) {
        } else {
          this.resetForm();
          this.submitted = false;
        }
      },
      error: (e) => { alert('שמירה נכשלה'); console.error(e); }
    });
  }

  cancel(): void {
    this.cancelEdit.emit();
    this.resetForm();
    this.submitted = false;
  }
  public loadForEdit(task: Task): void {
    this.submitted = false;
    this.editTask = task;
    this.patchFromTask(task);   // משתמש בפונקציה שכבר כתבת
  }

}
