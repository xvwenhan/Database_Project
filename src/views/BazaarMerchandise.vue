<!-- 市集的商品页面 -->
<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref } from 'vue';

const products = ref([
  {
    id: 1,
    name: '名字',
    Tag:"tag",
    price: 32,
    originalPrice: 93,
    discount: 3.3,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  {
    id: 2,
    name: '名字',
    Tag:"tag",
    price: 59,
    originalPrice: 269,
    discount: 2.2,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  {
    id: 3,
    name: '名字',
    Tag:"tag",
    price: 59,
    originalPrice: 269,
    discount: 2.2,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  {
    id: 4,
    name: '名字',
    Tag:"tag",
    price: 59,
    originalPrice: 269,
    discount: 2.2,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  {
    id: 5,
    name: '名字',
    Tag:"tag",
    price: 59,
    originalPrice: 269,
    discount: 2.2,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  {
    id: 6,
    name: '名字',
    Tag:"tag",
    price: 59,
    originalPrice: 269,
    discount: 2.2,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  {
    id: 7,
    name: '名字',
    Tag:"tag",
    price: 59,
    originalPrice: 269,
    discount: 2.2,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  {
    id: 8,
    name: '名字',
    Tag:"tag",
    price: 59,
    originalPrice: 269,
    discount: 2.2,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  {
    id: 9,
    name: '名字',
    Tag:"tag",
    price: 59,
    originalPrice: 269,
    discount: 2.2,
    description: '详细描述',
    image: '/src/assets/wy/product1.png'
  },
  // 其他商品数据...
]);
</script>

<template>
    <Navbar />
    <div class="product-display">
        <div v-for="product in products" :key="product.id" class="product-item">
        <img :src="product.image" :alt="product.name" class="product-image" />
        <div class="product-info">
            <p class="product-price">
            <span class="special-price">特卖价</span> ¥{{ product.price }}
            <span class="original-price">¥{{ product.originalPrice }}</span>
            <span class="discount">{{ product.discount }}折</span>
            </p>
            <h2 class="product-name">【tag】{{ product.name }} {{ product.description }}</h2>
        </div>
        </div>
    </div>
</template>

<style scoped>
.product-display {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  padding: 20px;
}

.product-item {
  width: calc(25% - 20px); /* 每行显示四个商品，减去间隙 */
  padding: 20px;
  border: 1px solid #e7e7e7;
  border-radius: 10px;
  background-color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.product-image {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 10px;
  margin-bottom: 10px;
}

.product-info {
  text-align: center;
}

.product-name {
  font-size: 16px;
  color: #333;
  margin-bottom: 10px;
}

.product-price {
  font-size: 18px;
  color: #e60012;
  margin-bottom: 10px;
}

.special-price {
  background-color: #ff4275;
  color: #fff;
  padding: 2px 5px;
  border-radius: 5px;
}

.original-price {
  text-decoration: line-through;
  margin-left: 5px;
  color: #999;
}

.discount {
  margin-left: 5px;
  color: #e60012;
}
</style>
