<script setup>
import { ref } from 'vue'

const login = ref({ username: '', password: '' })

function onLogin() {
  console.log('Вход (заглушка):', login.value)
}
</script>

<template>
  <div>
    <h1>Вход</h1>
    <form @submit.prevent="onLogin">
      <div class="form-group">
        <label for="username">Логин</label>
        <input id="username" name="username" v-model="login.username" />
      </div>

      <div class="form-group">
        <label for="password">Пароль</label>
        <input
          id="password"
          name="password"
          type="password"
          v-model="login.password"
        />
      </div>

      <button type="submit" class="submit-btn">Войти</button>
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
</style>