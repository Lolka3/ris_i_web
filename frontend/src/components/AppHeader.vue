<template>
  <header class="app-header">
    <div class="header-content">
      <router-link to="/">Список</router-link>
      <router-link to="/new">Создать</router-link>
      <router-link to="/login">Вход</router-link>
    </div>
  </header>
</template>

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
</style>