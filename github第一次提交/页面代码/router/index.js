import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/businesshomepage',
    name: 'BusinessHomePage',
    component: () => import('../views/BusinessHomePage.vue')
  },
  {
    path: '/businessordermanage',
    name: 'BusinessOrderManage',
    component: () => import('../views/BusinessOrderManage.vue')
  },
  {
    path: '/businesscommodity',
    name: 'BusinessCommodity',
    component: () => import('../views/BusinessCommodity.vue')
  },
  {
    path: '/businessmarket',
    name: 'BusinessMarket',
    component: () => import('../views/BusinessMarket.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
