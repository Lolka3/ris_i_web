<script setup>
defineProps({
  items: { type: Array, required: true },
})

const statusLabels = {
  New: 'Новая',
  InProgress: 'Назначена дата',
  Closed: 'Принята',
  Cancelled: 'Отменена',
}
</script>

<template>
  <table>
    <thead>
      <tr>
        <th>Номер</th>
        <th>Тема</th>
        <th>Дисциплина (ID)</th>
        <th>Статус</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="item in items" :key="item.id">
        <td>{{ item.number }}</td>
        <td>{{ item.title }}</td>
        <td>{{ item.disciplineId }}</td>
        <td>{{ statusLabels[item.status] || item.status }}</td>
      </tr>
    </tbody>
  </table>
</template>

<style scoped>
table {
  width: 100%;
  border-collapse: collapse;
  background: #fff;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  border-radius: 8px;
  overflow: hidden;
}
th, td {
  padding: 14px 16px;
  text-align: left;
  border-bottom: 1px solid #eee;
}
th {
  background: #f8f9fa;
  font-weight: 600;
}
tr:last-child td { border-bottom: none; }
tr:hover td { background: #fafbfc; }
</style>