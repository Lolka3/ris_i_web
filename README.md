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

## Запуск Backend и PostgreSQL (ЛР10)

```bash
cd backend
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.11
createdb course_retake_requests_lab10
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=course_retake_requests_lab10;Username=postgres;Password=ВАШ_ЛОКАЛЬНЫЙ_ПАРОЛЬ"
dotnet ef database update
psql -d course_retake_requests_lab10 -f ..\db\seed.sql
dotnet run --launch-profile http
```

- Конфигурация `ConnectionStrings:Default` хранится в локальном User Secrets, не в Git.
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- Заявки и номера генерируются и сохраняются в PostgreSQL, поэтому остаются после перезапуска API.
- В учебной версии создателем новых заявок служит пользователь с `id = 1` из `db/seed.sql`; полноценной аутентификации пока нет.
- /health проверяет только живость процесса, а не создание заявки.
- Коллекцию Postman импортировать из `docs/api-collection/retake-requests.postman_collection.json`; базовый URL — `http://localhost:5000`.
- Для PATCH передавайте `X-User-Id`: при назначении он должен совпадать с `assigneeUserId`, а статус менять может только назначенный пользователь. Это учебная идентификация, не полноценная аутентификация.
- Миграции находятся в `backend/Migrations/` и применяются к пустой базе `course_retake_requests_lab10`, чтобы не конфликтовать с таблицами ЛР9; не запускайте поверх этой же БД `db/schema.sql`.
- Отчёты: [ЛР9](docs/reports/lab-09.md), [ЛР10](docs/reports/lab-10.md).

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

Схема ЛР9 в `db/` остаётся самостоятельным SQL-развёртыванием. Backend
использует EF Core миграции из `backend/Migrations/`; применяйте их к новой
учебной базе, затем загрузите `db/seed.sql`.
