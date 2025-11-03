# Task Management Application – שָׁע"ם רמה ג'

מ# ShamTask – מבחן מפתח תוכנה רמה ג' (שע"ם)

מערכת לניהול משימות פשוטה שנבנתה במסגרת מבחן מפתח תוכנה רמה ג׳.  
הפרויקט כולל צד שרת מבוסס ‎ASP.NET Core‎ וצד לקוח מבוסס ‎Angular‎.

---

## 🚀 צד שרת (Backend)

- פותח ב־**ASP.NET Core 8.0**
- הנתונים נשמרים בקובץ ‎`Data/tasks.json`‎
- כולל טיפול מלא ב־CORS וב־HTTPS
- נקודות קצה עיקריות:

| Method | Endpoint       | Description          |
|--------|----------------|----------------------|
| GET    | `/tasks`       | קבלת כל המשימות     |
| POST   | `/tasks`       | הוספת משימה חדשה    |
| PUT    | `/tasks/{id}`  | עדכון משימה קיימת   |
| DELETE | `/tasks/{id}`  | מחיקת משימה          |

####  הפעלה מקומית
```bash
dotnet restore
dotnet run
׳

---
שרת ה־API נגיש גם אונליין:
https://shaam.creativehandsco.com/swagger

צד לקוח (Frontend)

פותח ב־Angular עם Reactive Forms

עיצוב באמצעות Bootstrap 5

מתקשר ל־API דרך קריאות ‎HTTPClient‎

כולל הוספה, עריכה, מחיקה ורשימת משימות

🔗 הפעלה אונליין

האפליקציה זמינה לצפייה ולבדיקה:
 https://stackblitz.com/edit/stackblitz-starters-izzidrst



## General Description

The system allows managing a list of tasks (To-Do list).  
Users can:
- Add new tasks via a reactive form.
- Edit existing tasks.
- Delete tasks.
- View all tasks in a responsive list.

Each task contains:
- **Title** (required)
- **Description** (optional)
- **Priority** (Low / Medium / High)
- **Due Date** (required)
- **Status** (Pending / In Progress / Completed)

---

##  Technologies Used

### Client Side
- **Angular 20**
- **TypeScript 5.8**
- **Bootstrap 5.3**
- **Reactive Forms**
- **RxJS 7**

### Server Side
- **.NET 8 (ASP.NET Core Web API)**
- **C# 12**
- **JSON file storage** (tasks.json)

---

## System Architecture (Overview)

- **Client (Angular):**  
  Structured by modules, components, and services.  
  Uses `TasksService` to communicate with the backend via HTTP.

- **Server (.NET 8 API):**  
  Provides REST endpoints to manage tasks stored in a local JSON file:
  - `GET /tasks` – Get all tasks  
  - `POST /tasks` – Add new task  
  - `PUT /tasks/{id}` – Update existing task  
  - `DELETE /tasks/{id}` – Delete task  

---

## Author

**Name:** Rami Issa  
**Exam:** שע"ם – Software Developer Level 3  
**Date:** November 2025  

---

## Notes

This README file will be **updated later** to include:
- Setup and run instructions for both Client and Server
- API base URL configuration
- StackBlitz link for the client app

---
