import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import Navbar from '../components/Navbar.vue'
import Merchabdise from '../views/MerchandiseView.vue'
import Bazaar from '../views/BazaarView.vue'
import Forum from '../views/ForumView.vue'
import Cart from '../views/CartView.vue'
import Ordercentre from '../views/OrdercentreView.vue'
import Personalcentre from '../views/PersonalcentreView.vue'
import Login from '../views/LoginView.vue'
import LoginAndRegister from '../views/LoginAndRegisterView.vue'
import ReleasePost from '../views/ReleasePost.vue'
import AdminHeaderSec from '../components/AdminHeaderSec.vue'
import PlatformInfo from '../views/PlatformInfo.vue';
import MerchantCertification from '../views/MerchantCertification.vue';
import MarketManagement from '../views/MarketManagement.vue';
import ReportManagement from '../views/ReportManagement.vue';
import AdminSidebarMenu from '../components/AdminSidebarMenu.vue'
import MerchantPage from '../views/MerchantPage.vue'
import MerchantShowcase from '../views/MerchantShowcase.vue'
import SearchProductShowcase from '../views/SearchProductShowcase.vue'
import BazaarMerchandise from '../views/BazaarMerchandise.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'HomeView',
      component: HomeView
    },
    {
      path: '/loginandregister',
      name: 'LoginAndRegister',
      component: LoginAndRegister
    },
    {
      path: '/home',
      name: 'home',
      component: HomeView
    },
    {
      path: '/merchandise/:category',
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
      path: '/login',
      name: 'Login',
      component: Login
    },
    {
      path: '/releasepost',
      name: 'ReleasePost',
      component: ReleasePost
    },
    {
      path: '/adminheadersec',
      name: 'AdminHeaderSec',
      component: AdminHeaderSec
    },
    { path: '/platform-info', 
      name: 'PlatformInfo', 
      component: PlatformInfo 
    },
    { path: '/merchant-certification', 
      name: 'MerchantCertification', 
      component: MerchantCertification 
    },
    { path: '/market-management', 
      name: 'MarketManagement', 
      component: MarketManagement 
    },
    { path: '/report-management', 
      name: 'ReportManagement', 
      component: ReportManagement 
    },
    { path: '/adminsidebarmenu', 
      name: 'AdminSidebarMenu', 
      component: AdminSidebarMenu 
    },
    { path: '/merchantpage',  
      name: 'MerchantPage', 
      component: MerchantPage 
    },
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
    },
    {
      path: '/messageview',
      name: 'MessageView',
      component: () => import('../views/MessageView.vue')
    },
    {
      path: '/viewpost',
      name: 'ViewPost',
      component: () => import('../views/ViewPost.vue')
    },
    { path: '/merchantshowcase', 
      name: 'MerchantShowcase', 
      component: MerchantShowcase 
    },
    { path: '/searchproductshowcase', 
      name: 'SearchProductShowcase', 
      component: SearchProductShowcase 
    },
    { path: '/bazaarmerchandise', 
      name: 'BazaarMerchandise', 
      component: BazaarMerchandise 
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    },
    {
      path: '/productdetail',
      name: 'ProductDetail',
      component: () => import('../views/ProductDetail.vue')
    },
    {
      path: '/pay',
      name: 'Pay',
      component: () => import('../views/PayView.vue')
    }
  ]
})
export default router
