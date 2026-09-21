# Матрица требований

| Требование ЛР1 | Поле или сущность | Запрос | Критерий |
|---|---|---|---|
| Создать объект (ФТ-1, ФТ-9) | RetakeRequest, title, disciplineId, description | POST /api/retake-requests | 1 |
| Назначить исполнителя (ФТ-3) | assigneeUserId | PATCH /api/retake-requests/{id}/assignee | 2 |
| Перевести в работу (ФТ-4, ФТ-12) | status, retakeDate, retakeTime, audience | PATCH /api/retake-requests/{id}/status | 3 |
| Закрыть заявку (ФТ-5) | status | PATCH /api/retake-requests/{id}/status | — |
| Отменить заявку (ФТ-6) | status | PATCH /api/retake-requests/{id}/status | — |
| Сводка для зав. кафедрой (ФТ-10) | агрегация по RetakeRequest | GET /api/retake-requests | — |

Все три критерия ЛР1 покрыты строками матрицы.