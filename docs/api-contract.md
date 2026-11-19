# API-контракт

Базовый путь: `/api/retake-requests`; локальный адрес: `http://localhost:5000`.

| Метод и путь | Тело / заголовок | Успех | Ошибка |
|---|---|---|---|
| GET `/api/retake-requests` | — | 200, массив или `[]` | — |
| GET `/api/retake-requests/{id}` | — | 200, объект | 404 |
| POST `/api/retake-requests` | title, disciplineId, description | 201, id, number, status: New | 400 |
| PATCH `/api/retake-requests/{id}/assignee` | assigneeUserId; `X-User-Id` | 200 | 400, 404, 409 |
| PATCH `/api/retake-requests/{id}/status` | status; при InProgress — retakeDate, retakeTime, audience; `X-User-Id` | 200 | 400, 404, 409 |

Клиент не присылает при создании id, number, status, assigneeUserId — их определяет сервер.
DELETE не используется: отмена — это статус Cancelled, строка остаётся.
Для PATCH заголовок `X-User-Id` задаёт учебного текущего пользователя: назначить можно только себя, менять статус — только свою (назначенную) заявку. Полноценной аутентификации пока нет.
При некорректном теле возвращается 400, при отсутствии заявки — 404, при запрещённом переходе — 409. Неожиданные ошибки логируются без тела запроса и возвращаются как безопасный ProblemDetails с кодом 500.
Данные хранятся в памяти и сбрасываются после перезапуска.

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
  "retakeTime": "14:30:00",
  "audience": "А-312"
}
```

## Статусы

Только четыре кода: New, InProgress, Closed, Cancelled.
Подписи на экране — свои, коды в API и БД не меняются.
