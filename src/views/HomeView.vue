<template>
    <div v-if="!isTestPage">
        <Navbar class="narbar"/>
        <div  class="LRcontainer">
        <img
            src="@/assets/mmy/arrow_left.svg"
            class="arrow-button1"
            @click="setAd((currentAdIndex - 1 + numAds) % numAds)"
            @mouseenter="stopTimer"
            @mouseleave="startTimer"
        />
        <img
            src="@/assets/mmy/arrow_right.svg"
            class="arrow-button2"
            @click="setAd((currentAdIndex + 1) % numAds)"
            @mouseenter="stopTimer"
            @mouseleave="startTimer"
        />
        <div class="ad">
            <img
            v-show="currentAdIndex === 0"
            src="@/assets/mmy/ad1.png"
            />
            <!-- class="ad_discount" -->
            <img
            v-show="currentAdIndex === 1"
            src="@/assets/mmy/ad2.png"
            />
            <img
            v-show="currentAdIndex === 2"
            src="@/assets/mmy/ad3.png"
            />
        <div class="knotArea" @mouseenter="stopTimer" @mouseleave="startTimer">
        <img
          v-show="currentAdIndex !== 0"
          src= "@/assets/mmy/knot.svg"
          class="knot1"
          @click="setAd(0)"
        />
        <img
            v-show="currentAdIndex === 0"
            src= "@/assets/mmy/active_knot.svg"
            class="knot1"
            @click="setAd(0)"
        />
        <img
            v-show="currentAdIndex !== 1"
            src="@/assets/mmy/knot.svg"
            class="knot2"
            @click="setAd(1)"
        />
        <img
            v-show="currentAdIndex === 1"
            src="@/assets/mmy/active_knot.svg"
            class="knot2"
            @click="setAd(1)"
        />
        <img
            v-show="currentAdIndex !== 2"
            src="@/assets/mmy/knot.svg"
            class="knot3"
            @click="setAd(2)"
        />
        <img
            v-show="currentAdIndex === 2"
            src="@/assets/mmy/active_knot.svg"
            class="knot3"
            @click="setAd(2)"
        />
        </div>
        </div>  
        </div>
    </div>
    
    <HomeCategory v-else />
  </template>
  
  <script setup lang="ts">
  import Navbar from '../components/Navbar.vue';
  import HomeCategory from './HomeCategory.vue';
  import { ref, onMounted, onBeforeUnmount } from 'vue';
  
  const currentAdIndex = ref(0);
  const numAds = 3;
  const isTestPage = ref(false);
  let adTimer: number | null = null;  // 用于存储计时器 ID，这样写是为了知道数据类型

  ////////////////////////// 每2.5秒调用setAd函数
  const startTimer = () => {
  if (adTimer === null) {
    adTimer = setInterval(() => {
      setAd((currentAdIndex.value + 1) % numAds);
    }, 2500);
  }
};
const stopTimer = () => {
  if (adTimer !== null) {
    clearInterval(adTimer);
    adTimer = null;
  }
};
  /////////////////////////// 用于处理鼠标滚动事件
  const handleScroll = (event: WheelEvent) => {
    if (event.deltaY > 0) {
      isTestPage.value = true;
    } else if (event.deltaY < 0) {
      isTestPage.value = false;
    }
  };
  ////////////////////////// 声明周期钩子
  //////////////////////////onMounted 和 onBeforeUnmount 确保在组件的整个生命周期内正确设置和移除事件监听器
  onMounted(() => {
    startTimer();//内部调用计时器
    window.addEventListener('wheel', handleScroll);//添加滚动事件监听器
  });
  onBeforeUnmount(() => {
    window.removeEventListener('wheel', handleScroll);
  });
  
  function setAd(index: number) {
    currentAdIndex.value = index;
  }
  </script>
  
  <style scoped>
  .narbar {
  position: absolute;
  background-color: rgba(0, 0, 0, 0.3);
  color: #FFFFFF;
  z-index: 3;
}

.LRcontainer {
  background-color: rgba(36, 63, 51);
  height: 100vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  /* padding: 20px; */
  position: relative; /* 添加这个以便子元素可以使用绝对定位 */
}

.ad {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
  height: 100%;
  overflow: hidden;
  position: relative; /* 确保 .knotArea 绝对定位基于 .ad */
}

.ad img {
  width: 100%;
  height: auto;
  object-fit: cover; /* 保证图片自适应 */
  z-index: 1;
}

.arrow-button1,
.arrow-button2,
.knot1,
.knot2,
.knot3 {
  cursor: pointer;
  z-index: 2;
}

.arrow-button1 {
  width: 50px;
  height: 50px;
  position: fixed;
  top: 50%;
  left: 70px;
}

.arrow-button2 {
  width: 50px;
  height: 50px;
  position: fixed;
  top: 50%;
  right: 70px;
}

.arrow-button2:hover,
.arrow-button1:hover {
  box-shadow: 0 0 5px rgba(229, 215, 215, 0.5);
  transform: scale(1.1);
}

.knotArea {
  display: flex;
  justify-content: center;
  gap: 15px;
  position: absolute;
  bottom: 20px; /* 你可以调整这个值来改变位置 */
  width: 10%;
}
.ad_discount {
  width: 130vh;
  display: block;
  margin: auto;
}

  </style>
  