<template>
  <header class="app-header">
    <div class="header-content">
      <router-link v-if="currentUser" to="/">Список</router-link>
      <router-link v-if="currentUser?.role === 'Student'" to="/new">Создать</router-link>
      <router-link v-if="!currentUser" to="/login">Вход</router-link>
      <span v-if="currentUser" class="user-name">{{ currentUser.fullName }}</span>
      <button v-if="currentUser" type="button" @click="onLogout">Выйти</button>
    </div>
    <p v-if="logoutError" class="logout-error" role="alert">{{ logoutError }}</p>
  </header>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { ApiError, logout } from '../api/tickets'
import { currentUser } from '../auth'

const router = useRouter()
const logoutError = ref('')

async function onLogout() {
  logoutError.value = ''
  try {
    await logout()
    currentUser.value = null
    await router.replace('/login')
  } catch (error) {
    logoutError.value = error instanceof ApiError ? error.message : 'Не удалось выйти'
  }
}
</script>

<style scoped>
.app-header {
  background: #fff;
  border-bottom: 1px solid #e0e0e0;
  box-shadow: 0 1px 3px rgba(0,0,0,0.05);
}
.header-content {
  max-width: 800px;
  margin: 0 auto;
  padding: 16px 24px;
  display: flex;
  gap: 12px;
}
.header-content a {
  padding: 8px 16px;
  border: 1px solid #ccc;
  background: #f8f9fa;
  color: #333;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 500;
  text-decoration: none;
  transition: all 0.2s ease;
}
.header-content a:hover {
  background: #e2e6ea;
  border-color: #bbb;
}
.header-content a.router-link-active {
  background: #4a90e2;
  border-color: #4a90e2;
  color: #fff;
}
.user-name {
  align-self: center;
  margin-left: auto;
  font-size: 14px;
}
.header-content button {
  padding: 8px 16px;
  border: 1px solid #ccc;
  background: #f8f9fa;
  color: #333;
  border-radius: 6px;
  cursor: pointer;
}
.logout-error {
  max-width: 752px;
  margin: 0 auto;
  padding: 0 24px 12px;
  color: #c0392b;
  font-size: 13px;
}
</style>