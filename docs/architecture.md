# Архитектура

```
Vue 3 + Vite              ASP.NET Core              PostgreSQL
localhost:5173  ───────►  localhost:5000  ───────►  обычно :5432
                  HTTP                     запросы к БД
```

Пример запроса целиком:

```
POST http://localhost:5000/api/retake-requests
Body: { "title": "Математический анализ", "disciplineId": 2, "description": "..." }
Response: 201 Created
```

**Подпись под схемой:**

> архитектура web-ИС семестра

Прямой стрелки от браузера к базе нет: иначе клиент получает доступ к данным и обходит серверные правила.