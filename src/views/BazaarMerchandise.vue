<template>
  <Navbar />
  <div class="market-page">
    <div class="left-panel">
      <h2 class="market-title" @click="goBackToMarketList">市集 > {{ marketTheme }}</h2>
      <div class="left-panel-bottom">
        <img src="@/assets/wy/market1.png" alt="市场图片1" class="market-image" style="position: absolute;top: 50px;"/>
        <p class="market-middle-text" >市集·{{ marketTheme }}</p>
        <img src="@/assets/wy/market2.png" alt="市场图片2" class="market-image" style="position: absolute;bottom: 50px;"/>
      </div>
    </div>
    
    <div class="right-panel">
      <div class="market-header">
        <img :src="'data:image/png;base64,' + marketPoster" alt="市场海报" class="market-poster" />
        <p class="market-detail">
          <!-- {{ marketDetail }} -->
          天青釉是汉族传统制瓷工艺中的珍品，瓷器釉色清明，也叫雨过天青，是一种幽淡隽永的高温兰色釉，我国古代陶书描写的青如天，明如镜，正是这种釉色特点的形容。

        </p>
      </div>
      <div class="product-display">
        <div v-for="product in products" :key="product.id" class="product-item" @click="goToProductDetail(product.id)">
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
    </div>
  </div>
</template>

<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, onMounted } from 'vue';
import axiosInstance from '../components/axios';
import { useRouter } from 'vue-router';

const router = useRouter();
const products = ref([]);
const marketTheme = localStorage.getItem('selectedMarkettheme');
const marketPoster = localStorage.getItem('selectedMarketposterImg');
const marketDetail = localStorage.getItem('selectedMarketdetail');

const fetchProducts = async (marketId) => {
  try {
    const response = await axiosInstance.post('/Market/GetProductsByMarket', {
      marketId: marketId
    });

    const data = response.data;
    products.value = data.map(item => ({
      id: item.productId,
      name: item.productName,
      tag: item.productTag,
      price: (item.productPrice * item.discount).toFixed(2),
      originalPrice: item.productPrice.toFixed(2),
      discount: (item.discount * 10).toFixed(1),
      description: item.description,
      image: 'data:image/png;base64,' + item.productPic
    }));
  } catch (error) {
    console.error('Error fetching products:', error);
  }
};

const goToProductDetail = (productId: string) => {
  localStorage.setItem('productIdOfDetail', productId);
  router.push('/productdetail');
};
const goBackToMarketList = () => {
  router.push('/bazaar');
};
onMounted(() => {
  const marketId = localStorage.getItem('selectedMarketId');
  
  fetchProducts(marketId);
});
</script>

<style scoped>
.market-page {
  display: flex;
  background-color: #f7f4ed;
  height: 100%;
  overflow: hidden;
}

.left-panel {
  width: 14%;
  /* height: 100%; */
  /* background-color: #e0f7ff; */
  /* padding: 20px; */
  text-align: center;
  /* display: flex;  */
}
.left-panel-bottom {
  background-color: #bdaead;
  margin-left: 30px;
  width: 80%;
  height: 600px;
  display: flex;
  flex-direction: column;
  justify-content: center; /* 使文字垂直居中 */
  align-items: center; /* 使内容水平居中 */
  position: relative;
}

.market-title {
  font-size: 20px;
  color: #333;
  margin-bottom: 20px;
  margin-top: 20px;
  cursor: pointer;
}

.market-image {
  width: 40%;
  /* position: absolute; */
  /* bottom: 50px; 固定距离底部边框50px */
}

.market-middle-text {
  font-size: 35px;
  color: #333;
  writing-mode: vertical-rl;
  text-orientation: upright;
  text-align: center;
  margin: 0; /* 移除外边距，确保居中 */
}

.right-panel {
  width: 86%;
  padding: 30px;
  overflow-y: auto; /* 允许垂直滚动 */
}


.market-header {
  display: flex;
  background-color: #bdaead;
  align-items: center;
  margin-bottom: 20px;

}

.market-poster {
  /* width: 500px; */
  height: 300px;
  padding: 15px;
  /* margin-right: 20px; */
  /* border-radius: 10px; */
}

.market-detail {
  font-size: 18px;
  color: #555;
}

.product-display {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}

.product-item {
  width: calc(33.33% - 20px);
  padding: 20px;
  border: 1px solid #e7e7e7;
  border-radius: 10px;
  background-color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  cursor: pointer;
  transition: border-color 0.3s ease;
}

.product-item:hover {
  border-color: #3498db;
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
