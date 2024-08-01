<!-- 买家用户的市集页面 -->
<template>
    <Navbar />
    <div class="carousel-container">
      <button class="carousel-button left" @click="prevSlide">◀</button>
      <div class="carousel-slide" :style="slideStyle">
        <img v-for="(image, index) in images" :key="index" :src="image" class="carousel-image" @click="goToMerchantShowcase" />
      </div>
      <button class="carousel-button right" @click="nextSlide">▶</button>
    </div>
</template>
  
  <script setup lang="ts">
  import Navbar from '../components/Navbar.vue';
  import { ref, computed } from 'vue';
  
  import image1 from '@/assets/wy/1.png';
  import image2 from '@/assets/wy/2.png';
  import image3 from '@/assets/wy/3.png';
  import router from '@/router';
  
  const images = ref([image1, image2, image3]);
  
  const currentIndex = ref(0);
  
  const prevSlide = () => {
    currentIndex.value = (currentIndex.value - 1 + images.value.length) % images.value.length;
  };
  
  const nextSlide = () => {
    currentIndex.value = (currentIndex.value + 1) % images.value.length;
  };

  const goToMerchantShowcase = () => {
  router.push('/merchantshowcase');
};
  
  const slideStyle = computed(() => ({
    transform: `translateX(-${currentIndex.value * 100}%)`
  }));
  </script>
  
  
<style scoped>
  .carousel-container {
    position: relative;
    width: 100%; /* 占满父级元素宽度 */
    overflow: hidden;
    max-width: 1420px; /* 可选：限制最大宽度 */
    margin: 0 auto; /* 可选：居中对齐 */
  }
  
  .carousel-slide {
    display: flex;
    transition: transform 0.5s ease;
    width: 100%; /* 确保子元素占满容器 */
  }
  
  .carousel-image {
    width: 100%; /* 图片宽度占满父级 */
    height: auto; /* 保持图片比例 */
    flex-shrink: 0; /* 防止图片缩小 */
    cursor: pointer; /* 鼠标悬停时显示为可点击 */
  }
  
  .carousel-button {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    background-color: rgba(0, 0, 0, 0.5);
    color: white;
    border: none;
    padding: 10px;
    cursor: pointer;
    z-index: 10;
  }
  
  .carousel-button.left {
    left: 10px; /* 调整位置 */
  }
  
  .carousel-button.right {
    right: 10px; /* 调整位置 */
  }
</style>
  