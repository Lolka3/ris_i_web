# API-контракт

Базовый путь: `/api/retake-requests`

| Метод и путь | Тело | Успех | Ошибка |
|---|---|---|---|
| GET /api/retake-requests | — | 200, массив или [] | — |
| GET /api/retake-requests/{id} | — | 200, объект | 404 |
| POST /api/retake-requests | title, disciplineId, description | 201, id, number, status: New | 400 |
| PATCH /api/retake-requests/{id}/assignee | assigneeUserId | 200 | 404 |
| PATCH /api/retake-requests/{id}/status | status, retakeDate, retakeTime, audience | 200 | 404, 409 |

Клиент не присылает при создании id, number, status, assigneeUserId — их определяет сервер.
DELETE не используется: отмена — это статус Cancelled, строка остаётся.

## Пример тела создания

```json
{
  "title": "Математический анализ",
  "disciplineId": 2,
  "description": "Пропустил экзамен по болезни, есть справка"
}
```

## Пример тела смены статуса (New → InProgress)

```json
{
  "status": "InProgress",
  "retakeDate": "2026-09-25",
  "retakeTime": "14:30",
  "audience": "А-312"
}
```

## Статусы

Только четыре кода: New, InProgress, Closed, Cancelled.
Подписи на экране — свои, коды в API и БД не меняются.