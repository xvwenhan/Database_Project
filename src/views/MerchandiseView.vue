<!-- 买家用户的商品页面 -->
<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, computed, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const categories = [
  { id: 1, name: '服装' },
  { id: 2, name: '首饰' },
  { id: 3, name: '家具' },
  { id: 4, name: '工艺品' },
  { id: 5, name: '小物件' },
];

const route = useRoute();
const router = useRouter();
const selectedCategory = computed(() => {
  const categoryId = route.params.category as string;
  console.log('Route category:', categoryId); // 调试输出
  return categoryId;
});

const selectedCategoryName = computed(() => {
  const category = categories.find(c => c.id.toString() === selectedCategory.value);
  console.log('Selected category:', category); // 调试输出
  return category ? category.name : '未知分类';
});

// 监听路由变化并输出调试信息
watch(route, (newRoute) => {
  console.log('Route changed:', newRoute.params.category);
});
</script>

<template>
  <Navbar />
  <div class="merchandise-container">
    <aside class="sidebar">
      <h2>分类</h2>
      <ul>
        <li v-for="category in categories" :key="category.id"
            :class="{ 'category-block': true, 'active': selectedCategory === category.id.toString() }"
            @click="() => router.push(`/merchandise/${category.id.toString()}`)">
          {{ category.name }}
        </li>
      </ul>
    </aside>
    <main class="main-content">
      <h1>{{ selectedCategoryName }}</h1>
      <div class="product-display">
        <div v-if="selectedCategory === '1'" class="category-background clothing-background"></div>
        <div v-else-if="selectedCategory === '2'" class="category-background jewelry-background"></div>
        <div v-else-if="selectedCategory === '3'" class="category-background furniture-background"></div>
        <div v-else-if="selectedCategory === '4'" class="category-background handicrafts-background"></div>
        <div v-else-if="selectedCategory === '5'" class="category-background smallthings-background"></div>
      </div>
    </main>
  </div>
</template>

<style scoped>
html, body {
  height: 100%;
  margin: 0;
  overflow: hidden;
}

.merchandise-container {
  display: flex;
  height: 100vh;
}

.sidebar {
  width: 200px;
  padding: 20px;
  background-color: #f5f5f5;
  border-right: 1px solid #ccc;
  overflow: auto;
}

.sidebar h2 {
  margin-bottom: 10px;
}

.sidebar ul {
  list-style: none;
  padding: 0;
}

.sidebar ul li {
  margin: 10px 0;
  padding: 10px;
  background-color: #e0e0e0;
  border-radius: 5px;
  cursor: pointer;
  text-align: center;
}

.sidebar ul li:hover {
  background-color: #d0d0d0;
}

.sidebar ul li.active {
  background-color: #a0a0a0;
}

.main-content {
  flex: 1;
  padding: 20px;
  overflow: auto;
}

.product-display {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.category-background {
  width: 100%;
  height: 500px;
  margin-top: 20px;
  border-radius: 10px;
}

.clothing-background {
  background-color: #a8beb4;
}

.jewelry-background {
  background-color: #a59fa8;
}

.furniture-background {
  background-color: #afb0a5;
}

.handicrafts-background {
  background-color: #a9a59f;
}

.smallthings-background {
  background-color: #908587;
}
</style>
