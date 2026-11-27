<script setup>
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ApiError, login } from '../api/tickets'
import { currentUser } from '../auth'

const route = useRoute()
const router = useRouter()
const credentials = ref({ login: '', password: '' })
const errorText = ref('')
const isSubmitting = ref(false)

async function onLogin() {
  if (isSubmitting.value) return

  errorText.value = ''
  isSubmitting.value = true
  try {
    currentUser.value = await login(credentials.value)
    const redirect = route.query.redirect
    const target = typeof redirect === 'string' && redirect.startsWith('/') && !redirect.startsWith('//')
      ? redirect
      : '/'
    await router.replace(target)
  } catch (error) {
    errorText.value = error instanceof ApiError
      ? error.status === 401
        ? 'Неверный логин или пароль'
        : error.message
      : 'Не удалось выполнить вход'
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <div>
    <h1>Вход</h1>
    <form @submit.prevent="onLogin">
      <div class="form-group">
        <label for="login">Логин</label>
        <input id="login" name="login" autocomplete="username" v-model="credentials.login" required />
      </div>

      <div class="form-group">
        <label for="password">Пароль</label>
        <input
          id="password"
          name="password"
          type="password"
          autocomplete="current-password"
          v-model="credentials.password"
          required
        />
      </div>

      <p v-if="errorText" class="error" role="alert">{{ errorText }}</p>
      <button type="submit" class="submit-btn" :disabled="isSubmitting">
        {{ isSubmitting ? 'Входим…' : 'Войти' }}
      </button>
    </form>
  </div>
</template>

<style scoped>
form {
  display: flex;
  flex-direction: column;
  gap: 16px;
  max-width: 450px;
  background: #fff;
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
input {
  font: inherit;
  padding: 10px 12px;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 14px;
}
input:focus {
  outline: none;
  border-color: #4a90e2;
  box-shadow: 0 0 0 3px rgba(74,144,226,0.15);
}
.submit-btn {
  background: #4a90e2;
  color: #fff;
  border: none;
  cursor: pointer;
  font-weight: 600;
  padding: 12px;
  border-radius: 6px;
  font-size: 15px;
}
.submit-btn:hover { background: #357abd; }
.submit-btn:disabled { opacity: 0.7; cursor: wait; }
.error { margin: 0; color: #c0392b; }
</style>