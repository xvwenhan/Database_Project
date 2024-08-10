<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, onMounted } from 'vue';
import axiosInstance from '../components/axios';

// 定义响应式数据变量
const products = ref([]);

// 获取商品数据的函数
const fetchProducts = async (marketId) => {
  try {
    const response = await axiosInstance.post('/Market/GetProductsByMarket', {
      marketId: marketId
    });

    // 处理返回的数据
    const data = response.data;
    products.value = data.map(item => ({
      id: item.productId,
      name: item.productName,
      tag: item.productTag,
      price: (item.productPrice * item.discount).toFixed(2), // 计算折后价
      originalPrice: item.productPrice.toFixed(2),
      discount: (item.discount * 10).toFixed(1), // 将折扣转化为折数
      description: item.description,
      image: 'data:image/png;base64,' + item.productPic // 将base64字符串转化为图片
    }));
  } catch (error) {
    console.error('Error fetching products:', error);
  }
};

// 组件挂载时调用获取商品数据的函数
onMounted(() => {
  const marketId = localStorage.getItem('selectedMarketId');
  if (marketId) {
    fetchProducts(marketId);
  }
});
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
        <h2 class="product-name">【{{ product.tag }}】{{ product.name }} {{ product.description }}</h2>
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
