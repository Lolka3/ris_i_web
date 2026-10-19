import { ref } from 'vue'

export const items = ref([
  {
    id: 1,
    number: 101,
    title: 'Математический анализ',
    disciplineId: 2,
    status: 'New',
  },
  {
    id: 2,
    number: 102,
    title: 'Физика',
    disciplineId: 1,
    status: 'InProgress',
  },
])