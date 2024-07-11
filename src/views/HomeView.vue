<<<<<<< HEAD
<script setup lang="ts">
import { useRouter } from 'vue-router';
import Navbar from '../components/Navbar.vue';

const router = useRouter();

const blocks = [
  { id: 1, title: '服装', bgImage: 'url(src/assets/Kind1.jpg)', gridArea: '1 / 1 / 2 / 3', category: '1' },
  { id: 2, title: '首饰', bgImage: 'url(src/assets/Kind2.png)', gridArea: '1 / 3 / 2 / 4', category: '2' },
  { id: 3, title: '家具', bgImage: 'url(src/assets/Kind3.png)', gridArea: '2 / 1 / 3 / 2', category: '3' },
  { id: 4, title: '工艺品', bgImage: 'url(src/assets/Kind4.png)', gridArea: '2 / 2 / 3 / 4', category: '4' },
  { id: 5, title: '小物件', bgImage: 'url(src/assets/Kind5.png)', gridArea: '3 / 1 / 4 / 3', category: '5' },
  { id: 6, bgImage: 'url(src/assets/Kind5.png)', gridArea: '3 / 3 / 4 / 3', class: 'disabled' },
];

const navigateToCategory = (category: string) => {
  console.log('Navigating to category:', category); // 调试输出
  router.push(`/merchandise/${category}`);
};
</script>

<template>
  <Navbar />
  <div class="container">
    <div class="sidebar">
      <h2>商品分类</h2>
    </div>
    <div class="block-container">
      <div
        v-for="block in blocks"
        :key="block.id"
        :style="{ backgroundImage: block.bgImage, gridArea: block.gridArea }"
        :class="['block', block.class]"
        @click="!block.class && navigateToCategory(block.category)"
      >
        {{ block.title }}
      </div>
    </div>
  </div>
</template>

<style scoped>
/* html, body {
  height: 100%;
  margin: 0;
  overflow: hidden;
} */

.container {
  display: flex;
  height: 100vh;
}

.background {
  position: fixed;
  top: 60px;
  width: 100%;
  height: calc(100vh - 60px);
  background-image: url('src/assets/Background.png');
  background-size: cover;
  background-position: center;
  z-index: -1;
}

.sidebar {
  width: 200px;
  padding: 20px;
  background-color: #f5f5f5;
  border-right: 1px solid #ccc;
}

.sidebar h2 {
  writing-mode: vertical-rl;
  text-align: center;
  font-family: 'Arial', sans-serif;
  font-size: 24px;
}

.sidebar ul {
  list-style: none;
  padding: 0;
  width: 100%;
}

.block-container {
  flex: 4;
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  grid-template-rows: repeat(3, 150px);
  grid-gap: 10px;
  padding: 20px;
}

.block {
  background-size: cover;
  background-position: center;
  border: 1px solid #ccc;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 18px;
  text-align: center;
  animation: fadeIn 1s ease-in-out;
  cursor: url('src/assets/cursor.png') 16 16, auto;
}

.block:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.block:active {
  transform: translateY(0);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}

.block:nth-child(odd) {
  animation: moveUp 1s forwards;
}

.disabled {
  pointer-events: none;
  transform: none !important;
  box-shadow: none !important;
}

.block.disabled:hover {
  transform: none !important;
  box-shadow: none !important;
}

.block:nth-child(even) {
  animation: moveDown 1s forwards;
}

@keyframes moveRight {
  from {
    transform: translateX(100%);
  }
  to {
    transform: translateX(0);
  }
}

@keyframes moveLeft {
  from {
    transform: translateX(-100%);
  }
  to {
    transform: translateX(0);
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateX(50px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}
</style>
=======
<template>
  <div class="container">
  <img
    src="@/assets/mmy/arrow_left.svg"
    class="arrow-button1"
    @click="setAd((currentAdIndex - 1 + numAds) % numAds)"
  />
  <img
    src="@/assets/mmy/arrow_right.svg"
    class="arrow-button2"
    @click="setAd((currentAdIndex + 1) % numAds)"
  />
  <img
    v-show="currentAdIndex!==0"
    src= "@/assets/mmy/knot.svg"
    class="knot1"
    @click="setAd(0)"
  />
  <img
    v-show="currentAdIndex===0"
    src= "@/assets/mmy/active_knot.svg"
    class="knot1"
    @click="setAd(0)"
  />
  <img
    v-show="currentAdIndex!==1"
    src="@/assets/mmy/knot.svg"
    class="knot2"
    @click="setAd(1)"
  />
  <img
    v-show="currentAdIndex===1"
    src="@/assets/mmy/active_knot.svg"
    class="knot2"
    @click="setAd(1)"
  />
  <img
    v-show="currentAdIndex!==2"
    src="@/assets/mmy/knot.svg"
    class="knot3"
    @click="setAd(2)"
  />
  <img
    v-show="currentAdIndex===2"
    src="@/assets/mmy/active_knot.svg"
    class="knot3"
    @click="setAd(2)"
  />
  </div>
  
  <div class="ad">
    <img
      v-show="currentAdIndex === 0"
      src="@/assets/mmy/ad1.png"
      class="ad_discount"
    />
    <img
      v-show="currentAdIndex === 1"
      src="@/assets/mmy/ad2.png"
      class="ad_discount"
    />
    <img
      v-show="currentAdIndex === 2"
      src="@/assets/mmy/ad3.png"
      class="ad_discount"
    />
    
  </div>

</template>

<script setup lang="ts">
import { ref , onMounted} from 'vue';

const currentAdIndex = ref(0); 
const numAds = 3; 
// const adImages = [
//   '@/assets/mmy/ad_discount.png',
//   '@/assets/mmy/ad2.png',
//   '@/assets/mmy/ad3.png',
// ];
// 定义定时器函数
const startTimer = () => {
  setInterval(() => {
    setAd((currentAdIndex.value + 1) % numAds)
  }, 2500); 
};
// 开始定时器在组件挂载后
onMounted(() => {
  startTimer();
});
function setAd(index:number){
  currentAdIndex.value=index;
}
// function adSrc(index: number): string {
//   return adImages[index] || '';
// }
</script>

<style>
.container {
background-color: rgb(36, 63, 51); /* 设置背景颜色为 RGB(36,63,51) */
height: 100vh; /* 使用视口高度填充整个屏幕 */
display: flex;
justify-content: space-between;
align-items: center;
padding: 20px; /* 添加一些内边距 */
}

.arrow-button1,
.arrow-button2,
.knot1,
.knot2,
.knot3 {
cursor: pointer;
}
.arrow-button1 {
width: 50px; /* 根据需要调整大小 */
height: 50px; /* 根据需要调整大小 */
position: fixed; /* 使用固定定位 */
top: 50%; /* 距离顶部50%处 */
left: 70px; /* 距离左侧50px处 */
}
.arrow-button2 {
width: 50px; 
height: 50px; 
position: fixed;
top: 50%; 
right: 70px; 
}
.arrow-button2:hover,
.arrow-button1:hover{
  box-shadow: 0 0 5px rgba(229, 215, 215, 0.5); /* 悬浮时阴影效果 */
  transform: scale(1.1); /* 悬浮时放大 */
}
.knot1,
.knot2,
.knot3{
  width: 30px; 
  height: 30px; 
  position: fixed;
  top: 95%;
}
.knot1{
right: 53%; 
}
.knot2{
right: 50%; 
}
.knot3 {
right: 47%; 
}

.ad_discount {
width: 130vh; 
/* height: 90vh;  */
position: fixed;
top: 19%; 
right: 15%; 
}
</style>
>>>>>>> de6673f0c30131bc05013ccc70b762f45ee86a0e
