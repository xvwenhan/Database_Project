<script setup lang="ts">

import { Swiper, SwiperSlide } from 'swiper/vue';
import { Navigation, Pagination } from 'swiper/modules';
import 'swiper/css';
import { Mousewheel } from 'swiper/modules';
import Navbar from '../components/Navbar.vue';
import { ref, computed,onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
//import { METHODS } from 'http';
import axiosInstance from '../components/axios';
import { log } from 'console';

const mySwiper=ref(null)

const categories = [
  { id: 1, name: '服装' },
  { id: 2, name: '首饰' },
  { id: 3, name: '家具' },
  { id: 4, name: '工艺品' },
  { id: 5, name: '小物件' },
];




const subCategoryNames = [
  { id: 1, parent:"服装",name: '汉族传统服饰' },
  { id: 2, parent:"服装", name: '少数民族服饰' },
  { id: 3, parent:"服装",name: '地方特色服饰' },
  { id: 4, parent:"首饰",name: '银饰' },
  { id: 5, parent:"首饰", name: '玉饰' },
  { id: 6, parent:"首饰", name: '宝石首饰' },
  { id: 7, parent:"首饰", name: '民族特使首饰' },
  { id: 8, parent:"家具", name: '床榻类' },
  { id: 9, parent:"家具", name: '桌案类' },
  { id: 10, parent:"家具", name: '椅凳类' },
  { id: 11, parent:"家具", name: '柜架类' },
  { id: 12, parent:"家具", name: '屏风类' },
  { id: 13, parent:"工艺品", name: '陶瓷' },
  { id: 14, parent:"工艺品", name: '漆器' },
  { id: 15, parent:"工艺品", name: '刺绣' },
  { id: 16, parent:"工艺品", name: '景泰蓝' },
  { id: 17, parent:"小物件", name: '文房四宝' },
  { id: 18, parent:"小物件", name: '剪纸艺术' },
  { id: 19, parent:"小物件", name: '竹编' },
];


const currentType = ref('服装')

const subCategoryNamesCopy = null;
//这里是分类描述
var image = "";
var description = "";

// const products = ref([
//   { id: 1, name: '景泰蓝花瓶', category: 4, price: 199, image: '/src/assets/example1.png' },
//   { id: 2, name: '景德镇双耳陶瓷花瓶', category: 4, price: 59, image: '/src/assets/example1.png' },
//   { id: 3, name: '云锦披肩', category: 1, price: 99, image: '/src/assets/example1.png' },
//   { id: 4, name: '刺绣手帕', category: 1, price: 20, image: '/src/assets/example1.png' },
//   { id: 5, name: '银手镯', category: 2, price: 150, image: '/src/assets/example1.png' },
//   { id: 6, name: '木雕摆件', category: 3, price: 300, image: '/src/assets/example1.png' },
//   { id: 7, name: '竹编篮子', category: 5, price: 50, image: '/src/assets/example1.png' },
//   { id: 8, name: '陶瓷茶具', category: 4, price: 80, image: '/src/assets/example1.png' },
//   { id: 9, name: '剪纸艺术', category: 5, price: 30, image: '/src/assets/example1.png' },
//   { id: 10, name: '草编帽子', category: 1, price: 40, image: '/src/assets/example1.png' },
//   { id: 11, name: '铜镜', category: 2, price: 200, image: '/src/assets/example1.png' },
//   { id: 12, name: '漆器盘子', category: 3, price: 180, image: '/src/assets/example1.png' },
//   { id: 13, name: '琉璃摆件', category: 4, price: 220, image: '/src/assets/example1.png' },
//   { id: 14, name: '陶瓷杯', category: 5, price: 70, image: '/src/assets/example1.png' },
//   { id: 15, name: '木雕书签', category: 3, price: 25, image: '/src/assets/example1.png' },
// ]);
const products = ref([])
const defaultImage = '/src/assets/example1.png'


const route = useRoute();
const router = useRouter();
const selectedCategory = computed(() => route.params.category as string);

const selectedCategoryName = computed(() => {
  const category = categories.find(c => c.id.toString() === selectedCategory.value);
  return category ? category.name : '未知分类';
});

// const filterSubCategoryNames = computed(()=>{
//     return subCategoryNames.filter(subCategory => subCategory.parent === currentType.value);
// })



const typeChange = (id,name) =>{
  console.log(swiperInstance,'swiperInstance')
  swiperInstance.slideTo(1)
  currentType.value = name
  getCategory(name);
  getCommodity(name)
  
  
};


const categoryDate = ref({categorY_PIC:'',categorY_DESCRIPTION:''})
//请求分类商品的介绍
const getCategory = async (category) => {
    
    try {
  
    const response = await axiosInstance.get('/Classification/GetCategoryByName', {
      params: {
        categoryName: category,
      },
    });
    categoryDate.value = response.data
    console.log(description);
  } catch (error) {
    console.error('请求错误：', error);
    return [];
  }
};



//获取对应分类下的商品信息
const getCommodity = async (category) =>{
 
  try {
    
    const response = await axiosInstance.get('/Classification/getProductsByTag', {
      params: {
        tag: category,
      },
    });
    products.value = response.data
    console.log(description);
  } catch (error) {
    console.error('请求错误：', error);
    return [];
  }
}

//获取当前分类商品的数量
const getProductCount = () =>{
 return products.value.length;
};

const goToProductDetail = (productId: string) => {
  // console.log('Selected Store ID:', productId);
  localStorage.setItem('productIdOfDetail', productId);  // 存储 productId
  console.log('跳转至 /productdetail');
  router.push('/productdetail');  // 跳转到商品详情页
};


let swiperInstance = null
const onSwiper = (swiper) => {
    swiperInstance = swiper
  }
onMounted(()=>{
  getCategory(currentType.value);
  getCommodity(currentType.value)
  console.log(1);
  
})
const activeSlideIndex=ref(0);
function onSlideChange(swiper:any) {
  activeSlideIndex.value = swiper.activeIndex;
  console.log(`activeSlideIndex.value is ${activeSlideIndex.value}`);
}
</script>

<template>
  
  <swiper 
      :direction="'vertical'"
      :slidesPerView="1"
      :mousewheel="true"
      :speed="1000"
      class="mySwiper"
      ref="mySwiper"
      @slideChange="onSlideChange"
      :options="swiperOptions"
      @swiper="onSwiper" 
      :modules="[Mousewheel,Navigation, Pagination]"
  >
    <swiper-slide>
      <div class="page1">
  <Navbar />
  
          <div class="container"> 
  <div class="card green" style="top: 200px;" @click="typeChange(1,'首饰')">
  
    <img src="/src/assets/mmy/工艺品.png" alt="首饰">
    <p class="vertical-text">首饰</p>
    
  </div>
  <div class="card blue" style="top: 250px;"@click="typeChange(1,'家具')">
    <img src="/src/assets/mmy/家具.png" alt="家具">
    <p>家具</p>
  </div>
  <div class="card beige" style="top: 150px;"@click="typeChange(1,'服装')">
    <img src="/src/assets/mmy/服装.png" alt="服装">
    <p>服装</p>
  </div>
  <div class="card purple" style="top: 230px;"@click="typeChange(1,'工艺品')">
    <img src="/src/assets/mmy/首饰.png" alt="工艺品">
    <p>工艺品</p>
  </div>
  <div class="card red" style="top: 180px;"  @click="typeChange(1,'小物件')">
    <img src="/src/assets/mmy/小物件.png" alt="小物件">
    <p>小物件</p>
  </div>
</div>

      </div>
      </swiper-slide>

    <swiper-slide>
      
  <div class="merchandise-container">
    
    <!-- <aside class="sidebar" > -->
     
      <ul v-show="false">
        <li v-for="category in categories" :key="category.id"
            :class="{ 'category-block': true, 'active': selectedCategory === category.id.toString() }"
            @click="() => router.push(`/merchandise/${category.id.toString()}`)">
          {{ category.name }}
        </li>
      </ul>
  
      <ul>
        <!-- <li>全部商品</li> -->
        <li v-for="subCategoryName in filterSubCategoryNames" :key="subCategoryName.id">
          {{ subCategoryName.name }}
        </li>
      </ul>
    <!-- </aside> -->
    
    <main class="main-content">
     <!-- 对商品分类描述界面 -->
     <div class="image-with-description">
    <img :src="'data:image/jpeg;base64,'+categoryDate.categorY_PIC" alt="">
    <p>{{categoryDate.categorY_DESCRIPTION}}</p>
    
  </div>
      <div class="product-display">
        <div v-for="product in products" :key="product.id" class="product-item" @click="goToProductDetail(product.productId)">
          <img :src="product.images && product.images.length>0 ?product.images[0].imageUrl: defaultImage" :alt="product.name" class="product-image" />
          <h2>{{ product.productName }}</h2>
          <p>价格: ¥{{ product.productPrice }}</p>
         
          
        </div>
      </div>

      <!-- <div class="pagination"> 
        <span> 共 {{ getProductCount() }} 件</span>
      </div> -->
    </main>
  </div>
</swiper-slide>


</swiper>

</template>


<style scoped>
 
 .end-of-list {
  width: 100%;  /* 确保容器占满整个宽度 */
  text-align: center;
  margin: 20px 0;
  font-size: 24px;
  color: #a61b29;
  display: flex;
  justify-content: center; /* 水平居中 */
  align-items: center; /* 垂直居中 */
}
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
  display: grid;
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

  writing-mode: vertical-rl; /* 竖排文字，从右到左 */
  text-align: center;
  line-height: 1.5;
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
  height: 100%;
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









.main-content {
  flex: 1;
  padding: 20px;
  /* overflow-y: auto; */
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
  /* margin-top: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px; */
  margin-top: 20px; 
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  position: absolute;
  bottom: 0;
  left: 50%;
  background-color: white;
  transform: translateX(-50%);
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

.image-with-description {
  display: flex;
  align-items: center;
  position: relative;
  z-index: 1;
  background-color: white; 
  padding: 20px;
  margin-right: 80px;
  margin-bottom: 20px;
  border-radius: 15px; /* 增加圆角 */
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* 添加轻微阴影 */
  transition: transform 0.3s ease, box-shadow 0.3s ease; /* 添加交互效果 */
}




.image-with-description img {
  width: 200px;
  height: auto;
  margin-right: 20px;
  border-radius: 10px; /* 为图片添加圆角 */
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* 为图片添加轻微阴影 */
}

.image-with-description .description {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
  font-size: 16px; 
  color: #333; 
  line-height: 1.5; 
}


</style>
