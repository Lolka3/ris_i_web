# API-контракт

Базовый путь: `/api/retake-requests`; локальный адрес: `http://localhost:5000`.

Все запросы заявок требуют аутентификационную cookie, кроме входа. Cookie
выдаётся endpoint `POST /api/auth/login` с телом `login` и `password`;
`GET /api/auth/me` возвращает пользователя текущей сессии, а
`POST /api/auth/logout` завершает её.

| Метод и путь | Тело / заголовок | Успех | Ошибка |
|---|---|---|---|
| POST `/api/auth/login` | login, password | 200, пользователь + cookie | 401 |
| GET `/api/auth/me` | cookie | 200, пользователь | 401 |
| POST `/api/auth/logout` | cookie | 204 | 401 |
| GET `/api/retake-requests` | cookie | 200, массив или `[]` | 401 |
| GET `/api/retake-requests/{id}` | cookie | 200, объект | 401, 404 |
| POST `/api/retake-requests` | роль Student; title, disciplineId, description | 201, id, number, status: New | 400, 401, 403 |
| PATCH `/api/retake-requests/{id}/assignee` | роль HeadOfDepartment; assigneeUserId | 200 | 400, 401, 403, 404, 409 |
| PATCH `/api/retake-requests/{id}/status` | роль назначенного Teacher; status; при InProgress — retakeDate, retakeTime, audience | 200 | 400, 401, 403, 404, 409 |

Клиент не присылает при создании id, number, status, assigneeUserId — их определяет сервер.
DELETE не используется: отмена — это статус Cancelled, строка остаётся.
При некорректном теле возвращается 400, при отсутствии заявки — 404, при запрещённом переходе — 409. Неожиданные ошибки логируются без тела запроса и возвращаются как безопасный ProblemDetails с кодом 500.
Идентификатор создателя берётся из аутентифицированной сессии. Назначать можно
только пользователя с ролью `Teacher`; менять статус может только назначенный
исполнитель. Данные и номера заявок хранятся в PostgreSQL и сохраняются после
перезапуска. Для отсутствующей дисциплины или пользователя возвращается 400.

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
