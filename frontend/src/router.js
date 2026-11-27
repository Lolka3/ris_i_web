import { createRouter, createWebHistory } from 'vue-router'
import TicketList from './pages/TicketList.vue'
import TicketNew from './pages/TicketNew.vue'
import Login from './pages/Login.vue'
import { getCurrentUser, ApiError } from './api/tickets'
import { currentUser } from './auth'

const routes = [
  { path: '/', component: TicketList, meta: { requiresAuth: true } },
  { path: '/new', component: TicketNew, meta: { requiresAuth: true } },
  { path: '/login', component: Login },
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.beforeEach(async (to) => {
  if (!to.meta.requiresAuth) return true

  try {
    currentUser.value = await getCurrentUser()
    return true
  } catch (error) {
    if (error instanceof ApiError && error.status === 401) {
      currentUser.value = null
      return { path: '/login', query: { redirect: to.fullPath } }
    }
    return true
  }
})