<script setup>
import { onMounted, ref } from 'vue'
import TicketTable from '../components/TicketTable.vue'
import { getTickets, ApiError } from '../api/tickets'
import { items, setItems } from '../ticketsStore'

const screen = ref('loading')
const errorText = ref('')

function messageFor(err) {
  if (err instanceof ApiError) {
    if (err.kind === 'network') return 'Нет связи с сервером'
    if (err.status === 400) return 'Проверьте поля запроса'
    if (err.status === 404) return 'Список заявок не найден'
    if (err.status >= 500) return 'Сервер не смог обработать запрос'
    return err.message
  }
  return 'Неизвестная ошибка'
}

async function load() {
  screen.value = 'loading'
  errorText.value = ''
  try {
    const list = await getTickets()
    setItems(list)
    screen.value = list.length === 0 ? 'empty' : 'data'
  } catch (err) {
    errorText.value = messageFor(err)
    screen.value = 'error'
  }
}

onMounted(load)
</script>

<template>
  <div>
    <h1>Заявки на пересдачу</h1>

    <div v-if="screen === 'loading'" class="state">Загрузка…</div>

    <div v-else-if="screen === 'empty'" class="state">
      Записей пока нет
    </div>

    <div v-else-if="screen === 'error'" class="state state-error">
      {{ errorText }}
      <button class="retry" @click="load">Повторить</button>
    </div>

    <TicketTable v-else :items="items" />
  </div>
</template>

<style scoped>
.state {
  padding: 24px;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  color: #555;
}
.state-error {
  color: #c0392b;
  border-left: 4px solid #c0392b;
}
.retry {
  margin-left: 12px;
  padding: 6px 12px;
  border: 1px solid #c0392b;
  background: #fff;
  color: #c0392b;
  border-radius: 6px;
  cursor: pointer;
}
.retry:hover {
  background: #fdecea;
}
</style>