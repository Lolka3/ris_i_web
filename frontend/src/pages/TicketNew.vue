<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { addItem } from '../ticketsStore'

const router = useRouter()

const form = ref({
  title: '',
  disciplineId: 0,
  description: '',
})

const errors = ref({
  title: '',
  disciplineId: '',
  description: '',
})

function validate() {
  errors.value = { title: '', disciplineId: '', description: '' }

  const title = form.value.title.trim()
  if (title.length < 5 || title.length > 80) {
    errors.value.title = 'Тема: от 5 до 80 символов'
  }

  const disciplineId = form.value.disciplineId
  if (!Number.isInteger(disciplineId) || disciplineId <= 0) {
    errors.value.disciplineId = 'Дисциплина: целое число больше нуля'
  }

  const description = form.value.description.trim()
  if (description.length < 10 || description.length > 500) {
    errors.value.description = 'Описание: от 10 до 500 символов'
  }

  return !errors.value.title && !errors.value.disciplineId && !errors.value.description
}

function onCreate() {
  if (!validate()) {
    return
  }

  addItem({
    title: form.value.title.trim(),
    disciplineId: form.value.disciplineId,
    description: form.value.description.trim(),
  })

  form.value = { title: '', disciplineId: 0, description: '' }
  errors.value = { title: '', disciplineId: '', description: '' }

  router.push('/')
}
</script>

<template>
  <div>
    <h1>Новая заявка</h1>

    <form @submit.prevent="onCreate" novalidate>
      <div class="form-group">
        <label for="title">Тема (5–80 символов)</label>
        <input
          id="title"
          name="title"
          v-model="form.title"
          autofocus
          placeholder="Например, Математический анализ"
        />
        <p v-if="errors.title" class="error">{{ errors.title }}</p>
      </div>

      <div class="form-group">
        <label for="disciplineId">Номер дисциплины (&gt; 0)</label>
        <input
          id="disciplineId"
          name="disciplineId"
          type="number"
          min="1"
          v-model.number="form.disciplineId"
          placeholder="Например, 2"
        />
        <p v-if="errors.disciplineId" class="error">{{ errors.disciplineId }}</p>
      </div>

      <div class="form-group">
        <label for="description">Описание (10–500 символов)</label>
        <textarea
          id="description"
          name="description"
          v-model="form.description"
          placeholder="Пропустил экзамен по болезни, есть справка"
        ></textarea>
        <p v-if="errors.description" class="error">{{ errors.description }}</p>
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
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
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
  background: #fff;
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
.error {
  color: #c0392b;
  font-size: 13px;
  margin: 0;
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
.submit-btn:hover {
  background: #357abd;
}
</style>