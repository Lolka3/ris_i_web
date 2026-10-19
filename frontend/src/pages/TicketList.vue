<script setup>
import TicketTable from '../components/TicketTable.vue'
import { items } from '../ticketsStore'
</script>

<template>
  <div>
    <h1>Заявки на пересдачу</h1>
    <TicketTable :items="items" />
  </div>
</template>