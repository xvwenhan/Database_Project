<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const categories = [
  { id: 1, name: '服装' },
  { id: 2, name: '首饰' },
  { id: 3, name: '家具' },
  { id: 4, name: '工艺品' },
  { id: 5, name: '小物件' },
];

const products = ref([
  { id: 1, name: '景泰蓝花瓶', category: 4, price: 199, image: '/src/assets/example1.png' },
  { id: 2, name: '景德镇双耳陶瓷花瓶', category: 4, price: 59, image: '/src/assets/example1.png' },
  { id: 3, name: '云锦披肩', category: 1, price: 99, image: '/src/assets/example1.png' },
  { id: 4, name: '刺绣手帕', category: 1, price: 20, image: '/src/assets/example1.png' },
  { id: 5, name: '银手镯', category: 2, price: 150, image: '/src/assets/example1.png' },
  { id: 6, name: '木雕摆件', category: 3, price: 300, image: '/src/assets/example1.png' },
  { id: 7, name: '竹编篮子', category: 5, price: 50, image: '/src/assets/example1.png' },
  { id: 8, name: '陶瓷茶具', category: 4, price: 80, image: '/src/assets/example1.png' },
  { id: 9, name: '剪纸艺术', category: 5, price: 30, image: '/src/assets/example1.png' },
  { id: 10, name: '草编帽子', category: 1, price: 40, image: '/src/assets/example1.png' },
  { id: 11, name: '铜镜', category: 2, price: 200, image: '/src/assets/example1.png' },
  { id: 12, name: '漆器盘子', category: 3, price: 180, image: '/src/assets/example1.png' },
  { id: 13, name: '琉璃摆件', category: 4, price: 220, image: '/src/assets/example1.png' },
  { id: 14, name: '陶瓷杯', category: 5, price: 70, image: '/src/assets/example1.png' },
  { id: 15, name: '木雕书签', category: 3, price: 25, image: '/src/assets/example1.png' },
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

const itemsPerPage = 9; // 每页显示的商品数量
const currentPage = ref(1); // 当前页码

const filteredProducts = computed(() => {
  const filtered = products.value.filter(product => product.category.toString() === selectedCategory.value);
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = start + itemsPerPage;
  return filtered.slice(start, end);
});

const totalPages = computed(() => {
  const totalItems = products.value.filter(product => product.category.toString() === selectedCategory.value).length;
  return Math.ceil(totalItems / itemsPerPage);
});

const isProductFavorite = (product) => {
  return favorites.value.some(fav => fav.id === product.id);
};

const addToFavorites = (product) => {
  if (!isProductFavorite(product)) {
    favorites.value.push(product);
    localStorage.setItem('favorites', JSON.stringify(favorites.value));
    alert(`${product.name} 已加入收藏夹`);
  }
};

const removeFromFavorites = (product) => {
  const index = favorites.value.findIndex(p => p.id === product.id);
  if (index !== -1) {
    favorites.value.splice(index, 1);
    localStorage.setItem('favorites', JSON.stringify(favorites.value));
    alert(`${product.name} 已从收藏夹移除`);
  }
};

const handleFavoriteToggle = (product) => {
  if (isProductFavorite(product)) {
    removeFromFavorites(product);
  } else {
    addToFavorites(product);
  }
};

const goToPage = (pageNumber) => {
  if (pageNumber >= 1 && pageNumber <= totalPages.value) {
    currentPage.value = pageNumber;
  }
};
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
      <!-- 收藏夹按钮 -->
      <div class="favorites-section">
        <button @click="showFavoritesDropdown = !showFavoritesDropdown" class="favorites-button">
          收藏夹
          <i :class="showFavoritesDropdown ? 'fa fa-chevron-up' : 'fa fa-chevron-down'"></i>
        </button>
        <div v-if="showFavoritesDropdown" class="favorites-dropdown">
          <ul>
            <li v-for="product in favorites" :key="product.id">
              {{ product.name }}
              <button @click="removeFromFavorites(product)">
  &#x2716;
</button>
            </li>
          </ul>
        </div>
      </div>
    </aside>
    
    <main class="main-content">
      <h1>{{ selectedCategoryName }}</h1>
      <div class="product-display">
        <div v-for="product in filteredProducts" :key="product.id" class="product-item">
          <img :src="product.image" :alt="product.name" class="product-image" />
          <h2>{{ product.name }}</h2>
          <p>价格: ¥{{ product.price }}</p>
          <button @click="handleFavoriteToggle(product)">
            {{ isProductFavorite(product) ? '取消收藏夹' : '加入收藏夹' }}
          </button>
        </div>
      </div>

      <!-- 分页按钮 -->
      <div class="pagination">
        <button @click="goToPage(currentPage - 1)" :disabled="currentPage === 1">上一页</button>
        <span>第 {{ currentPage }} 页 / 共 {{ totalPages }} 页</span>
        <button @click="goToPage(currentPage + 1)" :disabled="currentPage === totalPages">下一页</button>
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

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px;
  background-color: #a8a8a8;
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

.dropdown-menu li button {
  background-color: transparent;
  border: none;
  cursor: pointer;
}

.merchandise-container {
  display: flex;
  height: 100vh;
}

.sidebar {
  flex: 0 0 200px;
  background-color: #f8f8f8;
  padding: 20px;
  box-shadow: 2px 0 5px rgba(0,0,0,0.1);
}

.sidebar h2 {
  font-size: 24px;
  margin-bottom: 10px;
}

.sidebar ul {
  list-style: none;
  padding: 0;
}

.sidebar .category-block {
  padding: 10px;
  margin-bottom: 10px;
  background-color: #fff;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.sidebar .category-block.active {
  background-color: #c8e1ff;
}

.sidebar .category-block:hover {
  background-color: #eaeaea;
}

.favorites-section {
  margin-top: 20px;
}

.favorites-button {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  padding: 10px;
  background-color: #fff;
  border: 1px solid #ccc;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.favorites-button:hover {
  background-color: #eaeaea;
}

.favorites-dropdown {
  margin-top: 10px;
  padding: 10px;
  background-color: #fff;
  border: 1px solid #ccc;
  border-radius: 5px;
  max-height: 200px;
  overflow-y: auto;
}

.favorites-dropdown ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.favorites-dropdown li {
  padding: 5px 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.favorites-dropdown li button {
  background-color: transparent;
  border: none;
  cursor: pointer;
}

.main-content {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
}

.main-content h1 {
  font-size: 24px;
  margin-bottom: 20px;
}

.product-display {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}

.product-item {
  flex: 0 0 30%;
  background-color: #fff;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 2px 5px rgba(0,0,0,0.1);
  transition: transform 0.3s, box-shadow 0.3s;
}

.product-item:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 10px rgba(0,0,0,0.15);
}

.product-image {
  width: 100%;
  height: auto;
  object-fit: cover;
  border-radius: 5px;
  margin-bottom: 10px;
}

.product-item h2 {
  font-size: 18px;
  margin: 0 0 10px;
}

.product-item p {
  font-size: 16px;
  margin: 0 0 10px;
}

.pagination {
  margin-top: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
}

.pagination button {
  padding: 5px 10px;
  background-color: #fff;
  border: 1px solid #ccc;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.pagination button:disabled {
  cursor: not-allowed;
  opacity: 0.5;
}

.pagination button:hover:not(:disabled) {
  background-color: #f0f0f0;
}
</style>
