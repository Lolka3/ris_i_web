import { ref } from 'vue'

export const nextId = ref(3)

export const items = ref([
  {
    id: 1,
    number: 'TMP-1',
    title: 'Математический анализ',
    description: 'Пропустил экзамен по болезни, есть справка',
    disciplineId: 2,
    status: 'New',
  },
  {
    id: 2,
    number: 'TMP-2',
    title: 'Физика',
    description: 'Нужна пересдача после комиссии',
    disciplineId: 1,
    status: 'InProgress',
  },
])

export function addItem({ title, disciplineId, description }) {
  const id = nextId.value
  items.value.push({
    id,
    number: `TMP-${id}`,
    title,
    disciplineId,
    description,
    status: 'New',
  })
  nextId.value += 1
  return id
}