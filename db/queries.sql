-- Фильтр по статусу и сортировка: сначала самые новые заявки New
SELECT number, title, status, created_at
FROM retake_requests
WHERE status = 'New'
ORDER BY created_at DESC;

-- JOIN показывает название дисциплины, а не только discipline_id
SELECT r.number, r.title, d.name AS discipline_name, r.status
FROM retake_requests AS r
JOIN disciplines AS d ON d.id = r.discipline_id
ORDER BY r.created_at DESC;

-- Seed содержит все четыре статуса, поэтому результат показывает четыре группы
SELECT status, COUNT(*) AS count
FROM retake_requests
GROUP BY status
ORDER BY status;

-- Проверка CHECK: раскомментируйте для проверки отказа, затем откатите транзакцию
-- BEGIN;
-- INSERT INTO retake_requests (title, description, discipline_id, status, created_by_user_id)
-- VALUES ('Проверка статуса', 'Проверка запрета неизвестного статуса', 1, 'Done', 1);
-- ROLLBACK;
