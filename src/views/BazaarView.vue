<template>
  <Navbar />
  <div class="carousel-container">
    <button class="carousel-button left" @click="prevSlide">
      <el-icon :size="20"><ArrowLeftBold /></el-icon>
    </button>
    <div class="carousel-slide" :style="slideStyle">
      <div v-for="(market, index) in markets" :key="index" class="carousel-item">
        <h1>{{ market.title }}</h1>
        <div class="image-container">
          <img :src="market.image" class="carousel-image" @click="goToMerchantShowcase" />
          <div class="date-overlay">{{ market.date }}</div>
        </div>
      </div>
    </div>
    <button class="carousel-button right" @click="nextSlide">
      <el-icon :size="20"><ArrowRightBold /></el-icon>
    </button>
  </div>
</template>

<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, computed } from 'vue';
import router from '@/router';
import { ArrowLeftBold, ArrowRightBold } from '@element-plus/icons-vue';

const markets = ref([
  { title: 'Market 1', date: '2024-08-01', image: '/src/assets/wy/1.png' },
  { title: 'Market 2', date: '2024-08-02', image: '/src/assets/wy/2.png' },
  { title: 'Market 3', date: '2024-08-03', image: '/src/assets/wy/3.png' }
]);

const currentIndex = ref(0);

const prevSlide = () => {
  currentIndex.value = (currentIndex.value - 1 + markets.value.length) % markets.value.length;
};

const nextSlide = () => {
  currentIndex.value = (currentIndex.value + 1) % markets.value.length;
};

const goToMerchantShowcase = () => {
  router.push('/bazaarmerchandise');
};

const slideStyle = computed(() => ({
  transform: `translateX(-${currentIndex.value * 100}%)`
}));
</script>

<style scoped>
.carousel-container {
  position: relative;
  width: 100%;
  overflow: hidden;
  max-width: 1420px;
  margin: 0 auto;
}

.carousel-slide {
  display: flex;
  transition: transform 0.5s ease;
}

.carousel-item {
  width: 100%;
  flex-shrink: 0;
  text-align: center;
  padding: 20px;
  box-sizing: border-box;
}

.image-container {
  position: relative;
  width: 100%;
  height: 650px; /* 固定高度 */
}

.carousel-image {
  width: 100%;
  height: 100%;
  object-fit: cover; /* 使图片适应容器 */
  cursor: pointer;
}

.date-overlay {
  position: absolute;
  bottom: 10px;
  right: 10px;
  background-color: rgba(0, 0, 0, 0.5);
  color: white;
  padding: 5px;
  font-size: 12px;
}

.carousel-button {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background-color: rgba(0, 0, 0, 0.5);
  color: white;
  border: none;
  padding: 20px;
  cursor: pointer;
  z-index: 10;
  border-radius: 50%;
}

.carousel-button.left {
  left: 10px;
}

.carousel-button.right {
  right: 10px;
}

.carousel-button:hover {
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
}
</style>
