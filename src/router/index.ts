import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import Navbar from '../components/Navbar.vue'
import Merchabdise from '../views/MerchandiseView.vue'
import Bazaar from '../views/BazaarView.vue'
import Forum from '../views/ForumView.vue'
import Cart from '../views/CartView.vue'
import Ordercentre from '../views/OrdercentreView.vue'
import Personalcentre from '../views/PersonalcentreView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Navbar',
      component: Navbar
    },
    {
      path: '/home',
      name: 'home',
      component: HomeView
    },
    {
      path: '/merchandise',
      name: 'Merchabdise',
      component: Merchabdise
    },
    {
      path: '/bazaar',
      name: 'Bazaar',
      component: Bazaar
    },
    {
      path: '/forum',
      name: 'Forum',
      component: Forum
    },
    {
      path: '/cart',
      name: 'Cart',
      component: Cart
    },
    {
      path: '/ordercentre',
      name: 'Ordercentre',
      component: Ordercentre
    },
    {
      path: '/personalcentre',
      name: 'Personalcentre',
      component: Personalcentre
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    }
  ]
})

export default router
