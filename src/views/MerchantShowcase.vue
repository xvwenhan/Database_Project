<template>
  <Navbar />
  <div class="store-page">
    <div v-for="store in stores" :key="store.id" class="store-container">
      <div class="store-content">
        <div class="store-header">
          <img :src="store.logo" alt="Store Logo" class="store-logo" />
          <div class="store-info">
            <h2 class="store-name">{{ store.name }}</h2>
            <p class="store-rating">好评率: {{ store.rating }}%</p>
          </div>
        </div>
        <div class="store-products">
          <div v-for="product in store.products" :key="product.id" class="product-item">
            <img :src="product.image" alt="Product Image" class="product-image" />
            <p class="product-price">¥{{ product.price }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import Navbar from '../components/Navbar.vue';

const stores = ref([
  {
    id: 1,
    name: '景德镇官方旗舰店',
    logo: '/src/assets/wy/logo1.png',
    rating: 99.46,
    products: [
      { id: 1, image: '/src/assets/wy/product1.png', price: 758.00 },
      { id: 2, image: '/src/assets/wy/product2.png', price: 22.00 },
      { id: 3, image: '/src/assets/wy/product3.png', price: 188.00 },
      { id: 4, image: '/src/assets/wy/product4.png', price: 628.00 },
    ],
  },
  {
    id: 2,
    name: '一抹微蓝甄选直播间',
    logo: '/src/assets/wy/logo2.png',
    rating: 99.46,
    products: [
      { id: 1, image: '/src/assets/wy/product5.png', price: 399.00 },
      { id: 2, image: '/src/assets/wy/product6.png', price: 666.00 },
      { id: 3, image: '/src/assets/wy/product7.png', price: 500.00 },
      { id: 4, image: '/src/assets/wy/product8.png', price: 500.00 },
    ],
  },
  {
    id: 3,
    name: '蜀门家居旗舰店',
    logo: '/src/assets/wy/logo3.png',
    rating: 99.46,
    products: [
      { id: 1, image: '/src/assets/wy/product9.png', price: 21.00 },
      { id: 2, image: '/src/assets/wy/product10.png', price: 40.50 },
      { id: 3, image: '/src/assets/wy/product11.png', price: 40.50 },
      { id: 4, image: '/src/assets/wy/product12.png', price: 40.50 },
    ],
  },
]);
</script>

<style scoped>
.store-page {
  display: flex;
  flex-direction: column;
  gap: 20px;
  padding: 20px;
}

.store-container {
  border: 1px solid #e7e7e7;
  border-radius: 10px;
  padding: 20px;
}

.store-content {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.store-header {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 20px;
}

.store-logo {
  width: 50px;
  height: 50px;
}

.store-info {
  display: flex;
  flex-direction: column;
}

.store-name {
  font-size: 18px;
  margin: 0;
}

.store-rating {
  font-size: 14px;
  color: #888;
}

.store-products {
  display: flex;
  gap: 10px;
  margin-left: -200px; /* 调整为合适的负值，向左移动图片 */
}

.product-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 150px;
}

.product-image {
  width: 100%;
  border-radius: 5px;
}

.product-price {
  font-size: 16px;
  color: #333;
  margin-top: 5px;
}
</style>
