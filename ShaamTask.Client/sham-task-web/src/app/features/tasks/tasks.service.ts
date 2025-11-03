import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../../models/task';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class TasksService {
  private base = `${environment.apiBaseUrl}/tasks`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Task[]> {
    return this.http.get<Task[]>(this.base);
  }
  add(task: Omit<Task, 'id'>): Observable<Task> {
    return this.http.post<Task>(this.base, task);
  }
  update(id: number, task: Omit<Task, 'id'>): Observable<Task> {
    return this.http.put<Task>(`${this.base}/${id}`, task);
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
