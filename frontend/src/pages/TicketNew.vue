<script setup>
import { ref } from 'vue'

const form = ref({
  title: '',
  disciplineId: '',
  description: '',
})

function onCreate() {
  console.log('Форма создания (заглушка):', form.value)
}
</script>

<template>
  <div>
    <h1>Новая заявка</h1>
    <form @submit.prevent="onCreate">
      <div class="form-group">
        <label for="title">Тема (5–80 символов)</label>
        <input
          id="title"
          name="title"
          v-model="form.title"
          autofocus
          placeholder="Например, Математический анализ"
        />
      </div>

      <div class="form-group">
        <label for="disciplineId">Номер дисциплины (&gt; 0)</label>
        <input
          id="disciplineId"
          name="disciplineId"
          type="number"
          min="1"
          v-model="form.disciplineId"
          placeholder="Например, 2"
        />
      </div>

      <div class="form-group">
        <label for="description">Описание (10–500 символов)</label>
        <textarea
          id="description"
          name="description"
          v-model="form.description"
          placeholder="Пропустил экзамен по болезни, есть справка"
        ></textarea>
      </div>

      <button type="submit" class="submit-btn">Создать</button>
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
input, textarea {
  font: inherit;
  padding: 10px 12px;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 14px;
  background: #fff;
}
input:focus, textarea:focus {
  outline: none;
  border-color: #4a90e2;
  box-shadow: 0 0 0 3px rgba(74,144,226,0.15);
}
textarea {
  min-height: 100px;
  resize: vertical;
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