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