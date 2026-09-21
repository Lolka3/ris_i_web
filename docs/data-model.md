# Модель данных

## Словарь проекта

| Термин курса | Термин моей темы |
|---|---|
| Ticket | Заявка на пересдачу (RetakeRequest) |
| Site | Дисциплина (Discipline) |
| User | Пользователь (студент / преподаватель / зав. кафедрой) |

## Сущности

### RetakeRequest — заявка на пересдачу (главный объект)

| Поле | Тип | Ключ | Описание |
|---|---|---|---|
| id | int | PK | машинный идентификатор |
| number | int |  | человеческий номер заявки |
| title | string |  | название дисциплины (5–80 символов) |
| description | string |  | причина пересдачи (10–500 символов) |
| status | string |  | New / InProgress / Closed / Cancelled |
| disciplineId | int | FK → Discipline.id | ссылка на справочник |
| createdByUserId | int | FK → User.id | кто создал (студент) |
| assigneeUserId | int, null | FK → User.id | исполнитель (преподаватель), может отсутствовать |
| retakeDate | date, null |  | дата пересдачи |
| retakeTime | time, null |  | время пересдачи |
| audience | string, null |  | аудитория |

### Discipline — дисциплина (справочник)

| Поле | Тип | Ключ |
|---|---|---|
| id | int | PK |
| name | string |  |

### User — пользователь

| Поле | Тип | Ключ |
|---|---|---|
| id | int | PK |
| login | string |  |
| fullName | string |  |
| role | string | Student / Teacher / HeadOfDepartment |
| passwordHash | string |  |

## ER-диаграмма (связи словами)

```
User (1) ────< создаёт >──── (N) RetakeRequest
User (1) ────< назначен исполнителем >──── (N) RetakeRequest
Discipline (1) ────< относится >──── (N) RetakeRequest
```

- один справочник (Discipline) — много заявок;
- один пользователь создаёт много заявок;
- один пользователь (преподаватель) может быть назначен на много заявок;
- у новой заявки исполнитель может отсутствовать (`assigneeUserId = null`).

## Проверка 3НФ

Название дисциплины хранится в сущности Discipline один раз.
В RetakeRequest хранится только внешний ключ disciplineId.
При переименовании дисциплины меняется одна строка в справочнике,
поэтому текст названия не дублируется в заявках.
ФИО пользователя тоже не хранится в заявке — только createdByUserId и assigneeUserId.