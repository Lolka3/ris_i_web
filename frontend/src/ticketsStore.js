import { ref } from 'vue'

export const items = ref([])

export function setItems(list) {
  items.value = list
}

export function upsertItem(created) {
  if (items.value.some((x) => x.id === created.id)) return
  items.value = [...items.value, created]
}