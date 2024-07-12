<template>
    <div v-if="!isTestPage">
        <Navbar />
        <div  class="container">
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
  
  const startTimer = () => {
    setInterval(() => {
      setAd((currentAdIndex.value + 1) % numAds);
    }, 2500);
  };
  
  const handleScroll = (event: WheelEvent) => {
    if (event.deltaY > 0) {
      isTestPage.value = true;
    } else if (event.deltaY < 0) {
      isTestPage.value = false;
    }
  };
  
  onMounted(() => {
    startTimer();
    window.addEventListener('wheel', handleScroll);
  });
  
  onBeforeUnmount(() => {
    window.removeEventListener('wheel', handleScroll);
  });
  
  function setAd(index: number) {
    currentAdIndex.value = index;
  }
  </script>
  
  <style scoped>
  .container {
    background-color: rgb(36, 63, 51);
    height: 100vh;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 20px;
  }
  
  .arrow-button1,
  .arrow-button2,
  .knot1,
  .knot2,
  .knot3 {
    cursor: pointer;
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
  .knot1,
  .knot2,
  .knot3 {
    width: 30px;
    height: 30px;
    position: fixed;
    top: 95%;
  }
  .knot1 {
    right: 53%;
  }
  .knot2 {
    right: 50%;
  }
  .knot3 {
    right: 47%;
  }
  
  .ad_discount {
    width: 130vh;
    position: fixed;
    top: 19%;
    right: 15%;
  }
  </style>
  