# Информационная система учета заявок на пересдачу
**Студент:** Архипов Даниил Сергеевич

**Группа:** ТРИС-2-23

**Тема:** Заявки на пересдачу (№32)

**Статус:** На утверждении

**Стек:** Vue 3 + ASP.NET Core + PostgreSQL

## Документация
- [Заявка на тему](docs/project-proposal.md)
- [Требования ЛР1](docs/lab-01-requirements.md)
- [Архитектура](docs/architecture.md)
- [Модель данных](docs/data-model.md)
- [API-контракт](docs/api-contract.md)
- [Матрица требований](docs/requirements-matrix.md)

## Запуск клиента (ЛР3)

Нужен Node.js LTS.

```bash
cd frontend
npm install
npm run dev
```

Открыть адрес, который напишет терминал (обычно http://localhost:5173).

## Запуск Backend (ЛР8)

```bash
cd backend
dotnet restore
dotnet run --launch-profile http
```

- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- Заявки пока хранятся в памяти и пропадают после перезапуска сервера.
- /health проверяет только живость процесса, а не создание заявки.
- Коллекцию Postman импортировать из `docs/api-collection/retake-requests.postman_collection.json`; базовый URL — `http://localhost:5000`.
- Для PATCH передавайте `X-User-Id`: при назначении он должен совпадать с `assigneeUserId`, а статус менять может только назначенный пользователь. Это учебная идентификация, не полноценная аутентификация.
- Отчёт по работе: [docs/reports/lab-08.md](docs/reports/lab-08.md).

## PostgreSQL (ЛР9)

SQL-схема, демонстрационные данные и запросы находятся в `db/`. Создайте
отдельную учебную базу и выполните файлы через `psql` (или те же шаги в
pgAdmin):

```bash
createdb course_retake_requests
psql -d course_retake_requests -f db/schema.sql
psql -d course_retake_requests -f db/seed.sql
psql -d course_retake_requests -f db/queries.sql
```

Повторный запуск `schema.sql` удаляет и создаёт таблицы заново, поэтому
используйте только учебную БД. Значения `password_hash` в seed — не настоящие
пароли или хеши; пароль PostgreSQL в репозиторий не записывается.

В ЛР9 PostgreSQL не подключается к backend: API из ЛР8 продолжает хранить
заявки в памяти. Результаты и ограничения описаны в
[отчёте ЛР9](docs/reports/lab-09.md).
