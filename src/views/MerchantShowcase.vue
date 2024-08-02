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

// 示例数据
import logo1 from '@/assets/wy/logo1.png';
import logo2 from '@/assets/wy/logo2.png';
import logo3 from '@/assets/wy/logo3.png';

import product1 from '@/assets/wy/product1.png';
import product2 from '@/assets/wy/product2.png';
import product3 from '@/assets/wy/product3.png';
import product4 from '@/assets/wy/product4.png';
import product5 from '@/assets/wy/product5.png';
import product6 from '@/assets/wy/product6.png';
import product7 from '@/assets/wy/product7.png';
import product8 from '@/assets/wy/product8.png';
import product9 from '@/assets/wy/product9.png';
import product10 from '@/assets/wy/product10.png';
import product11 from '@/assets/wy/product11.png';
import product12 from '@/assets/wy/product12.png';

const stores = ref([
  {
    id: 1,
    name: '景德镇官方旗舰店',
    logo: logo1,
    rating: 99.46,
    products: [
      { id: 1, image: product1, price: 758.00 },
      { id: 2, image: product2, price: 22.00 },
      { id: 3, image: product3, price: 188.00 },
      { id: 4, image: product4, price: 628.00 },
    ],
  },
  {
    id: 2,
    name: '一抹微蓝甄选直播间',
    logo: logo2,
    rating: 99.46,
    products: [
      { id: 1, image: product5, price: 399.00 },
      { id: 2, image: product6, price: 666.00 },
      { id: 3, image: product7, price: 500.00 },
      { id: 4, image: product8, price: 500.00 },
    ],
  },
  {
    id: 3,
    name: '蜀门家居旗舰店',
    logo: logo3,
    rating: 99.46,
    products: [
      { id: 1, image: product9, price: 21.00 },
      { id: 2, image: product10, price: 40.50 },
      { id: 3, image: product11, price: 40.50 },
      { id: 4, image: product12, price: 40.50 },
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
