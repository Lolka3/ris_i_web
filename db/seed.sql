-- Демонстрационные пользователи. Значения password_hash не являются паролями или пригодными для аутентификации хешами
INSERT INTO users (id, login, full_name, role, password_hash) VALUES
    (1, 'student.demo', 'Иванов Иван Иванович', 'Student', 'seed-placeholder-not-a-real-hash'),
    (2, 'teacher.demo', 'Петров Петр Петрович', 'Teacher', 'seed-placeholder-not-a-real-hash');

INSERT INTO disciplines (id, name) VALUES
    (1, 'Математический анализ'),
    (2, 'Теория вероятностей');

INSERT INTO retake_requests (
    id, number, title, description, discipline_id, status, created_at,
    created_by_user_id, assignee_user_id, retake_date, retake_time, audience
) VALUES
    (1, 1001, 'Математический анализ', 'Пропустил экзамен по болезни', 1, 'New',
        '2026-11-10 09:00:00+03', 1, NULL, NULL, NULL, NULL),
    (2, 1002, 'Теория вероятностей', 'Не набрал проходной балл на экзамене', 2, 'New',
        '2026-11-11 10:30:00+03', 1, NULL, NULL, NULL, NULL),
    (3, 1003, 'Математический анализ', 'Нужна повторная сдача экзамена', 1, 'InProgress',
        '2026-11-12 11:15:00+03', 1, 2, '2026-11-25', '14:30', 'А-312'),
    (4, 1004, 'Теория вероятностей', 'Экзамен пропущен по уважительной причине', 2, 'InProgress',
        '2026-11-13 12:00:00+03', 1, 2, '2026-11-26', '10:00', 'Б-204'),
    (5, 1005, 'Математический анализ', 'Пересдача успешно проведена', 1, 'Closed',
        '2026-11-14 08:45:00+03', 1, 2, '2026-11-20', '09:00', 'А-105'),
    (6, 1006, 'Теория вероятностей', 'Заявка отменена студентом', 2, 'Cancelled',
        '2026-11-15 16:20:00+03', 1, 2, NULL, NULL, NULL);

-- Синхронизируем identity-счётчики после явного задания учебных id
SELECT setval(pg_get_serial_sequence('users', 'id'), (SELECT MAX(id) FROM users), true);
SELECT setval(pg_get_serial_sequence('disciplines', 'id'), (SELECT MAX(id) FROM disciplines), true);
SELECT setval(pg_get_serial_sequence('retake_requests', 'id'), (SELECT MAX(id) FROM retake_requests), true);
