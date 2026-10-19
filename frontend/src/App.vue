<script setup>
import AppHeader from './components/AppHeader.vue'
</script>

<template>
  <div class="app-wrapper">
    <AppHeader />
    <main class="app-main">
      <router-view />
    </main>
  </div>
</template>

<style>
body, html {
  margin: 0;
  padding: 0;
  background-color: #f4f6f8;
  min-height: 100vh;
}
</style>

<style scoped>
.app-wrapper {
  font-family: system-ui, -apple-system, 'Segoe UI', Roboto, sans-serif;
  color: #333;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}
.app-main {
  flex: 1;
  width: 100%;
  max-width: 800px;
  margin: 0 auto;
  padding: 32px 24px;
  box-sizing: border-box;
}
</style>