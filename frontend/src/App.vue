<script setup>
import { ref } from 'vue'

const screen = ref('list')

const items = ref([
  {
    id: 1,
    number: 'A-101',
    title: 'Математический анализ',
    disciplineId: 2,
    status: 'New',
  },
  {
    id: 2,
    number: 'A-102',
    title: 'Физика',
    disciplineId: 1,
    status: 'InProgress',
  },
])

const form = ref({
  title: '',
  disciplineId: '',
  description: '',
})

function onCreate() {
  console.log('Форма создания (заглушка):', form.value)
}

const login = ref({ username: '', password: '' })
function onLogin() {
  console.log('Вход (заглушка):', login.value)
}

const statusLabels = {
  New: 'Новая',
  InProgress: 'Назначена дата',
  Closed: 'Принята',
  Cancelled: 'Отменена',
}
</script>

<template>
  <!-- Обертка на весь экран -->
  <div class="app-wrapper">
    
    <!-- Шапка с кнопками -->
    <header class="app-header">
      <div class="header-content">
        <button type="button" @click="screen = 'list'">Список</button>
        <button type="button" @click="screen = 'new'">Создать</button>
        <button type="button" @click="screen = 'login'">Вход</button>
      </div>
    </header>

    <!-- Центральная рабочая область -->
    <main class="app-main">
      <!-- ЭКРАН 1: СПИСОК -->
      <div v-if="screen === 'list'">
        <h1>Заявки на пересдачу</h1>
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
      </div>

      <!-- ЭКРАН 2: СОЗДАНИЕ -->
      <div v-else-if="screen === 'new'">
        <h1>Новая заявка</h1>
        <form @submit.prevent="onCreate">
          <div class="form-group">
            <label for="title">Тема (5–80 символов)</label>
            <input id="title" name="title" v-model="form.title" autofocus placeholder="Например, Математический анализ" />
          </div>

          <div class="form-group">
            <label for="disciplineId">Номер дисциплины (&gt; 0)</label>
            <input id="disciplineId" name="disciplineId" type="number" min="1" v-model="form.disciplineId" placeholder="Например, 2" />
          </div>

          <div class="form-group">
            <label for="description">Описание (10–500 символов)</label>
            <textarea id="description" name="description" v-model="form.description" placeholder="Пропустил экзамен по болезни, есть справка"></textarea>
          </div>

          <button type="submit" class="submit-btn">Создать</button>
        </form>
      </div>

      <!-- ЭКРАН 3: ВХОД -->
      <div v-else>
        <h1>Вход</h1>
        <form @submit.prevent="onLogin">
          <div class="form-group">
            <label for="username">Логин</label>
            <input id="username" name="username" v-model="login.username" />
          </div>

          <div class="form-group">
            <label for="password">Пароль</label>
            <input id="password" name="password" type="password" v-model="login.password" />
          </div>

          <button type="submit" class="submit-btn">Войти</button>
        </form>
      </div>
    </main>
  </div>
</template>

<!-- ГЛОБАЛЬНЫЕ СТИЛИ (чтобы убрать черный фон браузера и Vite) -->
<style>
body, html {
  margin: 0;
  padding: 0;
  background-color: #f4f6f8 !important; /* Принудительно светло-серый фон */
  min-height: 100vh;
}
</style>

<!-- ЛОКАЛЬНЫЕ СТИЛИ (как требует методичка) -->
<style scoped>
.app-wrapper {
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  color: #333333;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* --- ШАПКА --- */
.app-header {
  background: #ffffff;
  border-bottom: 1px solid #e0e0e0;
  box-shadow: 0 1px 3px rgba(0,0,0,0.05);
  width: 100%;
}

.header-content {
  max-width: 800px;
  margin: 0 auto;
  padding: 16px 24px;
  display: flex;
  gap: 12px;
}

header button {
  padding: 8px 16px;
  cursor: pointer;
  border: 1px solid #ccc;
  background-color: #f8f9fa;
  color: #333;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 500;
  transition: all 0.2s ease;
}

header button:hover {
  background-color: #e2e6ea;
  border-color: #bbb;
}

/* --- РАБОЧАЯ ОБЛАСТЬ --- */
.app-main {
  flex: 1; /* Растягивает рабочую область на оставшуюся высоту */
  width: 100%;
  max-width: 800px;
  margin: 0 auto;
  padding: 32px 24px;
  box-sizing: border-box;
}

h1 {
  font-size: 24px;
  margin-top: 0;
  margin-bottom: 20px;
  color: #1a1a1a;
}

/* --- ТАБЛИЦА --- */
table {
  width: 100%;
  border-collapse: collapse;
  background-color: #ffffff;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  border-radius: 8px;
  overflow: hidden;
}

th, td {
  padding: 14px 16px;
  text-align: left;
  border-bottom: 1px solid #eeeeee;
}

th {
  background-color: #f8f9fa;
  color: #333333;
  font-weight: 600;
}

tr:last-child td {
  border-bottom: none;
}

tr:hover td {
  background-color: #fafbfc;
}

/* --- ФОРМЫ --- */
form {
  display: flex;
  flex-direction: column;
  gap: 16px;
  max-width: 450px;
  background: #ffffff;
  padding: 24px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

label {
  font-weight: 500;
  color: #444;
  font-size: 14px;
}

input,
textarea {
  font: inherit;
  padding: 10px 12px;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 14px;
  color: #333;
  background-color: #fff;
  transition: border-color 0.2s, box-shadow 0.2s;
}

input:focus,
textarea:focus {
  outline: none;
  border-color: #4a90e2;
  box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.15);
}

textarea {
  min-height: 100px;
  resize: vertical;
}

/* --- КНОПКА --- */
.submit-btn {
  background-color: #4a90e2;
  color: white;
  border: none;
  cursor: pointer;
  font-weight: 600;
  padding: 12px;
  border-radius: 6px;
  font-size: 15px;
  margin-top: 8px;
  transition: background-color 0.2s;
}

.submit-btn:hover {
  background-color: #357abd;
}
</style>