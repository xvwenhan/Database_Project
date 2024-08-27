<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { METHODS } from 'http';
import axiosInstance from '../components/axios';

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


const goToPage = (pageNumber) => {
  if (pageNumber >= 1 && pageNumber <= totalPages.value) {
    currentPage.value = pageNumber;
  }
};

const typeChange = (id) =>{
  router.push(`/merchandise/${id.toString()}`);
  //使界面下滑 xx px
  window.scrollBy(0,750);
}
</script>

<template>
    <swiper-slide>
      <div class="page1">
  <Navbar />
  
          <div class="container">
  <div class="card green" style="top: 200px;" @click="typeChange(1)">
  
    <img src="/src/assets/mmy/工艺品.png" alt="首饰">
    <p>首饰</p>
    
  </div>
  <div class="card blue" style="top: 250px;"@click="typeChange(2)">
    <img src="/src/assets/mmy/家具.png" alt="家具">
    <p>家具</p>
  </div>
  <div class="card beige" style="top: 150px;"@click="typeChange(3)">
    <img src="/src/assets/mmy/服装.png" alt="服装">
    <p>服装</p>
  </div>
  <div class="card purple" style="top: 230px;"@click="typeChange(4)">
    <img src="/src/assets/mmy/首饰.png" alt="工艺品">
    <p>工艺品</p>
  </div>
  <div class="card red" style="top: 180px;"  @click="typeChange(5)">
    <img src="/src/assets/mmy/小物件.png" alt="小物件">
    <p>小物件</p>
  </div>
</div>

      </div>
      </swiper-slide>


  <div class="merchandise-container">
    
    <aside class="sidebar" v-show="false">
     
      <ul>
        <li v-for="category in categories" :key="category.id"
            :class="{ 'category-block': true, 'active': selectedCategory === category.id.toString() }"
            @click="() => router.push(`/merchandise/${category.id.toString()}`)">
          {{ category.name }}
        </li>
      </ul>
  
   
      

    
    </aside>
    
    <main class="main-content">
     
      <div class="product-display">
        <div v-for="product in filteredProducts" :key="product.id" class="product-item">
          <img :src="product.image" :alt="product.name" class="product-image" />
          <h2>{{ product.name }}</h2>
          <p>价格: ¥{{ product.price }}</p>
      
           
        
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
 

.container {
  display: flex;
  justify-content: center; /* 水平居中对齐 */
 
  /* align-items: center; */
  height: 100vh; /* 使容器高度为视口高度 */
  z-index: 9999;
}

.card {
  width: 150px;
  height: 250px;
  margin: 0px;
  background-color: #fff;

  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  text-align: center;
  position: relative;
  z-index: 9999;
}


.card:hover {
    transform: translateY(-10px); 
    box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);
}

.card img {
  width: 80px; 
  height: 80px;
  margin-top: 20px;
}

.card p {
  font-size: 18px;
  color: #333;
  margin-top: 10px;
}



.card:hover .icon {
    animation: shake 0.5s; /* 应用晃动动画 */
}

/* 图标晃动效果 */
@keyframes shake {
    0% { transform: rotate(0deg); }
    25% { transform: rotate(10deg); }
    50% { transform: rotate(0deg); }
    75% { transform: rotate(-10deg); }
    100% { transform: rotate(0deg); }
}

.green {  background-image: url('@/assets/categories/b1.png');    background-size: cover; }
.blue { background-image: url('@/assets/categories/b4.png');    background-size: cover; }
.beige { background-image: url('@/assets/categories/b7.png');    background-size: cover; }
.purple { background-image: url('@/assets/categories/b9.png');    background-size: cover; }
.red { background-image: url('@/assets/categories/b10.png');    background-size: cover; }


.page1 {
  background-color: #f0f0f0;
  background-image: url('@/assets/categories/背景图.jpg');
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  height: 100vh;
}




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







.merchandise-container {
  display: flex;
  height: 100vh;
  background-image: url('@/assets/categories/背景图.jpg');
}

.sidebar {
  flex: 0 0 200px;
  background-color: #003366; /* 深蓝色背景 */
  color: #ffffff; /* 白色文本 */
  padding: 20px;
  box-shadow: 2px 0 10px rgba(0,0,0,0.2); /* 更明显的阴影 */
  display: flex;
  flex-direction: column;
}

.sidebar h2 {
  font-size: 24px;
  margin-bottom: 20px;
  color: #e0e0e0; /* 较浅的颜色，提升对比度 */
}

.sidebar ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.sidebar .category-block {
  padding: 12px;
  margin-bottom: 12px;
  background-color: #004080; /* 稍浅的深蓝色背景 */
  border-radius: 8px; /* 更大的圆角 */
  cursor: pointer;
  transition: background-color 0.3s, transform 0.3s; /* 增加平滑的过渡效果 */
  color: #ffffff; /* 白色文本 */
}

.sidebar .category-block.active {
  background-color: #00509e; /* 更亮的蓝色用于高亮显示 */
}

.sidebar .category-block:hover {
  background-color: #003d66; /* 悬停时的背景颜色 */
  transform: scale(1.02); /* 添加缩放效果 */
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

.animate__animated.animate__slideInUp{
--animate-duration: 1.2s;
animation-delay: 0.5s;
}
.animate__animated.animate__slideInDown{
--animate-duration: 1.2s;
animation-delay: 0.5s;
}

.swiper {
width: 100%;
height: 100%;
}

</style>
