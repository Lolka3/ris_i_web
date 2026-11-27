# ЛР10. EF Core и PostgreSQL

## Что сделано

- Добавлены сущности `Discipline`, `User`, `RetakeRequest` и `AppDbContext`.
- EF Core отображает SQL-таблицы, ограничения, индексы и связи из ЛР9.
- Создана миграция `InitialCreate`; API использует PostgreSQL через Npgsql.
- GET, POST и оба PATCH работают с БД вместо списка в памяти.
- `id` и `number` генерируются PostgreSQL; создатель в учебном API — демо-пользователь `id = 1`.
- Строка подключения читается из `ConnectionStrings:Default` и задаётся локально через User Secrets.

## Развёртывание

1. Создать отдельную пустую учебную базу `course_retake_requests_lab10`, не используя базу ЛР9 с уже существующими таблицами.
2. В `backend` задать локальный User Secret `ConnectionStrings:Default`.
3. Выполнить `dotnet ef database update`.
4. Из `backend` загрузить начальные данные командой `psql -d course_retake_requests_lab10 -f ..\db\seed.sql`.
5. Запустить API через `dotnet run --launch-profile http`.

Не выполняйте `db/schema.sql` поверх базы, созданной миграцией: этот файл
предназначен для отдельного SQL-развёртывания ЛР9.

## Проверки

| Проверка | Результат |
|---|---|
| `dotnet restore backend\backend.csproj` | успешно |
| `dotnet build backend\backend.csproj --no-restore` | успешно, 0 предупреждений и ошибок |
| Генерация миграции и SQL-скрипта | успешно |
| `dotnet ef migrations has-pending-model-changes` | модель совпадает с миграцией |
| `git diff --check` | успешно |
| `dotnet ef database update` | миграция `InitialCreate` применена к PostgreSQL 17.12 |
| `db/seed.sql` и `GROUP BY status` | 2 пользователя, 2 дисциплины, 6 заявок; все 4 статуса присутствуют |
| GET и POST API | GET вернул 6 seed-заявок; POST создал заявку `id = 7`, `number = 1007` |
| PATCH assignee и status | исполнитель `2` и статус `InProgress` сохранены |
| PATCH с недопустимым `Done` | API вернул 400 |
| Перезапуск API и GET по id | `id = 7`, `number = 1007`, статус `InProgress` и исполнитель `2` сохранились; GET списка вернул 7 заявок |

Пароль подключения и дампы в репозиторий не добавляются.
