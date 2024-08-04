<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, computed, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const categories = [
  { id: 1, name: '服装' },
  { id: 2, name: '首饰' },
  { id: 3, name: '家具' },
  { id: 4, name: '工艺品' },
  { id: 5, name: '小物件' },
];

const products = ref([
  {
    id: 1,
    name: '景泰蓝花瓶',
    category: 4,
    price: 199,
    image: '/src/assets/example1.png',
  },
  {
    id: 2,
    name: '景德镇双耳陶瓷花瓶',
    category: 4,
    price: 59,
    image: '/src/assets/example2.png',
  },
  {
    id: 3,
    name: '云锦披肩',
    category: 1,
    price: 99,
    image: '/src/assets/example3.png',
  },
]);

const cart = ref([]);
const favorites = ref([]);
const showFavoritesDropdown = ref(false);

const route = useRoute();
const router = useRouter();
const selectedCategory = computed(() => route.params.category as string);

const selectedCategoryName = computed(() => {
  const category = categories.find(c => c.id.toString() === selectedCategory.value);
  return category ? category.name : '未知分类';
});

const filteredProducts = computed(() => {
  return products.value.filter(product => product.category.toString() === selectedCategory.value);
});

const addToCart = (product) => {
  cart.value.push(product);
  alert(`${product.name} 已加入购物车`);
};

const addToFavorites = (product) => {
  if (!favorites.value.includes(product)) {
    favorites.value.push(product);
    alert(`${product.name} 已加入收藏夹`);
  }
};

const removeFromFavorites = (product) => {
  const index = favorites.value.indexOf(product);
  if (index !== -1) {
    favorites.value.splice(index, 1);
    alert(`${product.name} 已从收藏夹移除`);
  }
};

const searchQuery = ref('');

const performSearch = () => {
  console.log('Searching for:', searchQuery.value);
};

const toggleFavoritesDropdown = () => {
  showFavoritesDropdown.value = !showFavoritesDropdown.value;
};
</script>

<template>
  <Navbar />
  <header class="header">
    <div class="search-bar">
      <input v-model="searchQuery" placeholder="搜索商品..." />
      <button @click="performSearch">搜索</button>
    </div>
    <div class="user-actions">
      <div class="favorites" @click="toggleFavoritesDropdown">
        <i class="fa fa-star"></i> 收藏夹
      </div>
      <div class="cart" @click="() => router.push('/cart')">
        <i class="fa fa-shopping-cart"></i> 购物车
      </div>
    </div>
  </header>
  <div v-if="showFavoritesDropdown" class="dropdown-menu">
    <ul>
      <li v-for="product in favorites" :key="product.id">
        {{ product.name }}
        <button @click="removeFromFavorites(product)">
          <i class="fa fa-trash"></i>
        </button>
      </li>
    </ul>
  </div>
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
        <div v-for="product in filteredProducts" :key="product.id" class="product-item">
          <img :src="product.image" alt="product.name" class="product-image" />
          <h2>{{ product.name }}</h2>
          <p>价格: ¥{{ product.price }}</p>
          <button @click="addToCart(product)">加入购物车</button>
          <button @click="addToFavorites(product)">收藏</button>
        </div>
      </div>
    </main>
  </div>
</template>


<style scoped>
@import '@fortawesome/fontawesome-free/css/all.css';

html, body {
  height: 100%;
  margin: 0;
  overflow: hidden;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px;
  background-color: #a8a8a8;
}

.logo {
  font-size: 24px;
  color: white;
}

.search-bar {
  display: flex;
  align-items: center;
}

.search-bar input {
  padding: 5px;
  border-radius: 5px;
  border: 1px solid #ccc;
}

.search-bar button {
  padding: 5px 10px;
  margin-left: 5px;
  background-color: #fff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.user-actions {
  display: flex;
  align-items: center;
}

.user-actions div {
  margin-left: 10px;
  cursor: pointer;
  color: white;
}

.dropdown-menu {
  position: absolute;
  top: 60px; /* Adjust according to your header height */
  right: 10px; /* Adjust according to your layout */
  background-color: white;
  border: 1px solid #ccc;
  border-radius: 5px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.dropdown-menu ul {
  list-style: none;
  padding: 10px;
  margin: 0;
}

.dropdown-menu li {
  padding: 5px 10px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.dropdown-menu li:hover {
  background-color: #f0f0f0;
}

.merchandise-container {
  display: flex;
  height: calc(100vh - 60px); /* Subtracting header height */
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

.product-item {
  width: calc(33.33% - 20px); /* Three items per row with gap */
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 10px;
  background-color: #fff;
}

.product-image {
  width: 50%;
  height: 200px;
  object-fit: cover;
  border-radius: 10px;
}
</style>

