<!-- 外部调用 -->
<!-- 
const functionName = () => {
  router.push({ path: '/productdetail', query: { id: '01' } });
} 
其中'01'可变,需填入对应商品的id
-->
<template>
  <Navbar class="narbar"/>
  <div v-show="isLoading" class="loading">
    <div class="loading-text">加载中...</div>
  </div>
  <div v-show="!isLoading" class="PDcontainer">
    <div class="storeContent">
        <img :src="`data:image/png;base64,${product.storeAvatar}`" class="Avatar" />
        <div class="storeName">&nbsp&nbsp{{ product.storeName }}</div>
        <div class="storeScore">评分：{{  product.score}}</div>
        <!-- 店铺收藏按钮，与商品收藏差不多 -->
        <el-button v-show="role==='买家'"
              @click="starStore"
              class="starStore-button"
              style="display: flex;
              align-items: center;
              justify-content: center;
              font-size: 21px;
              border-radius: 15px;
              border: 2px solid rgba(0,0,0,0.4);
              cursor: pointer;
              width: auto;
              height:65%;
              position: absolute;
              right:20%;" 

            >
            <img v-show="product.isStoreStared" 
            src="@/assets/mmy/stared.svg"
            alt="star"
            class="icon"/>
            <img v-show="!product.isStoreStared" 
            src="@/assets/mmy/star.svg"
            alt="star"
            class="icon"/>
            {{ product.isStoreStared ? '已收藏' : '&nbsp收&nbsp&nbsp藏&nbsp' }}
            </el-button>
        <!-- 进入店铺按钮 -->
        <el-button
              class="enterStore-button"
              @click="enterStore"
              style="display: flex;
              align-items: center;
              justify-content: center;
              font-size: 21px;
              border-radius: 15px;
              border: 2px solid rgba(0,0,0,0.4);
              cursor: pointer;
              width: auto;
              height:65%;
              position: absolute;
              right:5%;" 
            >
            <img  
            src="@/assets/mmy/store.svg"
            class="icon"/>
            进店逛逛 
            </el-button>
      </div>
    <div class="productContent">
      <div class="image">
        <img
          :src="`data:image/png;base64,${product.picture}`"
          class="image_inside"
        />
        <div class="overlay">细节描述：{{ product.description }}</div>
      </div>
      <div class="text">
        <div v-show="product.discountPrice===product.price" class="price">￥{{ product.price }}</div>
        <div v-show="product.discountPrice!==product.price" class="doubleprice">
          <div class="discountPrice">￥{{ product.price }}</div>
          <div class="price">￥{{ product.discountPrice }}</div>
        </div>
        <!-- <div v-show="product.discount!==1" class="dis_price">
          <div class="dis">-{{ product.discount*100 }}%</div>
          <div class="original_and_dis_price">
            <div class="original_price">￥{{ product.price }}</div>
            <div class="final_price">￥{{ (product.price*product.discount) .toFixed(2)}}&nbsp&nbsp&nbsp</div>
          </div>
        </div> -->

        <div class="name">{{ product.name }}</div>
        <div class="store">来自 {{ product.storeName }} | 100%非遗正品保证</div>
        <div class="fromwhere">
          <div class="from1">发货地</div>
          <div class="from2">{{ product.fromWhere }}</div>
        </div>
        <div class="baozhang">
          <div class="baozhang1">保&nbsp&nbsp&nbsp&nbsp障</div>
          <div class="baozhang2">假一赔十&nbsp&nbsp&nbsp&nbsp七天无理由退货 </div>
        </div>
        <div class="star_and_buy">
          <!-- 收藏 -->
          <el-button v-show="role==='买家'"
          @click="starProduct"
          class="starProduct-button"
          style="display: flex;
          align-items: center;
          justify-content: center;
          font-size: 21px;
          border-radius: 5px;
          cursor: pointer;
          width: auto;
          height:50px;
          right:30%;
          bottom:0%;
          position: absolute" 
        >
        <img v-show="product.isProductStared" 
        src="@/assets/mmy/stared.svg"
        alt="star"
        class="icon"/>
        <img v-show="!product.isProductStared" 
        src="@/assets/mmy/star.svg"
        alt="star"
        class="icon"/>
        {{ product.isProductStared ? '已收藏' : '&nbsp收&nbsp&nbsp藏&nbsp' }}
        </el-button>
        <!-- 购买 -->
        <el-button type="success" 
        v-show="role==='买家'"
        @click="enterPay"
        style="background-color: #257b5e; 
        letter-spacing: 5px; 
        font-size: 22px;
        width: 25%; 
        height:50px;
        right: 5%;
        bottom:0%;
        position: absolute" 
        > 购买</el-button>
        </div>
      </div>
    </div> 
    <div class="productAndCommentContent">
        <!-- 侧边导航栏 -->
        <div class="sidebar">
          <div class="nav-item" @click="showProducts">商品预览</div>
          <div class="nav-item" @click="showComments">店铺评论</div>
        </div>
        <!-- 内容区域 -->
        <div class="main-content">
            <div v-if="activeSection === 'preview'" class="preview-section">
                <div 
                  class="displayProductItem"
                  v-for="product in displayProducts" 
                  :key="product.productId" 
                  @click="handleProductClick(product.productId)"
                  >
                    <img :src="product.productPic" alt="图片加载失败" class="product-image" />
                    <div class="product-text">
                      <h2>{{ product.productName }}</h2>
                      <p>价格: ¥{{ product.productPrice }}</p>
                    </div> 
                </div>
            </div>

            <div v-if="activeSection === 'comments'" class="comments-section">
              评论预览
          </div>
          <!-- 内容区域内的跳转按钮 -->
          <!-- <el-button 
           @click="handleButtonClick"
           class="enter-button"
           style="display: flex;
              align-items: center;
              justify-content: center;
              font-size: 23px;
              border-radius: 5px;
              cursor: pointer;
              width: 15%;
              height:50px;
              position: absolute;
              right:5%;">{{ activeSection=='comments'?'查看全部评价':'查看全部商品' }}</el-button> -->
      </div>
    </div>
  </div>
</template>
  
<script setup >
    import Navbar from '../components/Navbar.vue';
    import { ref,onMounted,reactive} from 'vue';
    import 'element-plus/dist/index.css';
    import { ElButton ,ElMessage} from 'element-plus';
    import { useRouter } from 'vue-router';
    import axiosInstance from '../components/axios';
    
    //页面是否正在加载
    const isLoading=ref(true);
    //从浏览器中获取数据
    // const productId = '555555';
    const productId = localStorage.getItem('productIdOfDetail');
    const userId =localStorage.getItem('userId');
    const role=localStorage.getItem('role');
    // 使用 useRoute 来访问路由参数
    const router=useRouter();

    const product = ref({
      name: '',
      picture: '',
      price: 0,
      description: '',
      storeName: '',
      storeId: '',
      discountPrice: 0,
      fromWhere: '',
      score: 0,
      isProductStared: false,/////////注意1，补一条所属店铺是否被收藏
      isStoreStared:false,
      storeAvatar:''
    }) ;
    //处理商品和评论预览
    const activeSection = ref('preview');
    const displayProducts = reactive([]);           
    const message=ref('');
  
    onMounted(async () => {
      // showProducts();
      console.log(`当前登录用户id为${userId}`);
      try {
        // const response = await axiosInstance.get('/Account/send_verification_code',
        // {params: { email: registerEmail.value }});
        const response = await axiosInstance.get(`/Shopping/GetProductDetails/`,{
          params:{
            userId:userId,
            productId:productId
          }
        });
        isLoading.value=false;
        product.value=response.data;
        console.log('页面加载完成');
        ////////////////////////////////折扣测试
        // product.value.discountPrice=3;
        // console.log(product.value);
        } catch (error) {
          ElMessage.error('页面加载失败！');
        }
    })
    const enterStore=()=>{
      localStorage.setItem('storeIdOfDetail',product.value.storeId);
      router.push('/shopdetail');
    }
    const starProduct = async() => {
      if(product.value.isProductStared===false){
        console.log('进入收藏');
        try {
          const response = await axiosInstance.post('/Favourite/BookmarkProduct', null, {
              params: {
                userId: userId,
                productId: productId
              }
            });

            console.log(response.data);
            if (response.status === 200) {
              product.value.isProductStared = !product.value.isProductStared;
            } else {
              console.error('Unexpected response status:', response.status);
            }
      } catch (error) {
        console.error('starProduct failed:', error);
      }
    }else{
        console.log('进入取消收藏');
        try {
          const response = await axiosInstance.delete('/Favourite/UnbookmarkProduct', {
            params: {
              userId: userId,
              productId: productId
            }
          });
          console.log(response.data);
          if (response.status === 200) {
            product.value.isProductStared = !product.value.isProductStared;
          } else {
            console.error('Unexpected response status:', response.status);
          }
        } catch (error) {
          console.error('unStarProduct failed:', error);
        }
      }
    };
    const starStore = async() => {
      if(product.value.isStoreStared===false){
        try {
          const response = await axiosInstance.post('Favourite/BookmarkStore',null,{
            params: {
                userId: userId,
                'storeId':product.value.storeId
              }});
            if (response.status === 200) {
              product.value.isStoreStared = !product.value.isStoreStared; 
            } else {
              console.error('Unexpected response status:', response.status);
            }
        } catch (error) {
          console.error('starStore failed:', error);
        }
      }else{
        try {
          const response = await axiosInstance.delete('/Favourite/UnbookmarkStore', {
            params: {
              userId: userId,
              storeId: product.value.storeId
            }
          });
          if (response.status === 200) {
            product.value.isStoreStared = !product.value.isStoreStared;
          } else {
            console.error('Unexpected response status:', response.status);
          }
        } catch (error) {
          console.error('unStarStore failed:', error);
        }
      }
    };
    const showComments = () => {
      activeSection.value = 'comments';
    };

    const showProducts = async () => {
      activeSection.value='preview';
      console.log(`product.value.storeId${product.value.storeId}`);
      try {
      const response = await axiosInstance.get('/StoreViewProduct/GetProductsByStoreIdAndViewType', {
        params: {
          storeId: product.value.storeId,
          ViewType: 1
        }
      });
      if (displayProducts.length > 0) {
        displayProducts.splice(0, displayProducts.length);
      }
      //splice和slice不同
      // 只取返回数据的前三个商品
      const limitedProducts = response.data.slice(0, 3);
      limitedProducts.forEach(product => {
        product.productPic = `data:image/png;base64,${product.productPic}`;
        displayProducts.push(product);
      });
      console.log(`displayProducts is ${JSON.stringify(displayProducts, null, 2)}`)
    } catch (error) {
      if (error.response) {
        message.value = error.response.data;
      } else {
        message.value = '获取全部信息失败';
      }
    }
    console.log(message.value);
};

    const handleButtonClick = () => {
      // Handle button click
      console.log('按钮点击');
    };
    const handleProductClick = (productId) => {
      console.log('正在被点击');
      console.log(`productId is ${productId}`);
      localStorage.setItem('productIdOfDetail',productId);
      router.replace('/productdetail').catch(err => {});
    };
    const enterPay=()=>{
      const productStr = JSON.stringify(product.value);//序列化对象
      router.push({path:'/pay',query:{product: productStr,
                                      isPaid:'false'
      }});
    }

</script>

 <style scoped>
.el-button:active{
box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2); /* 点击时的阴影效果 */
transform: scale(0.95); /* 点击时缩小效果 */
}

/* 容器样式 */
.PDcontainer {
  background-color: #CCCCCC;
  display: flex;
  align-items: center;/*这才是水平对齐 */
  flex-direction: column;
  /* 哇趣！！！！！！min-height！！！I love you!!*/
  min-height: 100vh;
  overflow-x: hidden; /*隐藏水平滚动条 */
  padding-bottom:15px;
  /* overflow: auto; */
}
.productContent,.storeContent{
  background-color: #FFFFFF;
  width: 70%; /*百分比都是相对于父容器说的*/ 
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); /* 添加阴影效果（可选） */
  box-sizing: border-box; /* 使内边距和边框包含在宽度和高度内 */
  border-radius: 15px; 
  margin-top:15px;
}

.storeContent{
  /* height:80px; */
  padding: 10px 30px 10px 10px;
  position: relative;
  display: flex;
  flex-direction: row; 
  justify-items: center;
}
.productContent {
  min-height:450px;
  padding: 40px; 
  display: flex;
  position: relative;
  overflow: hidden;
  flex-direction: row; 
}
.productAndCommentContent {
  width: 70%; /*百分比都是相对于父容器说的*/ 
  margin-top:15px;
  display: flex;
  flex-direction: row;
  position: relative;
}

.image {
  width: 48%;
  height: 100%;
  border-radius: 15px;
  position: relative; /* 确保覆盖层绝对定位在此容器内 */
}
.image_inside,.overlay {
  width: 100%;
  height: 100%;
  border-radius: 15px;
}
.overlay{
  position: absolute;/*absolute 定位相对于最近的 position 属性不为 static 的父元素*/
  top: 0;/*将 .overlay 的上边缘对齐到 .image 的上边缘 */
  left: 0;
  display: flex;
  align-items: center;
  justify-content: center;

  background: rgba(0, 0, 0, 0.3); /* 黑色半透明背景 */
  color: white;
  opacity: 0; /* 默认隐藏 */
  transition: opacity 0.3s; /* 平滑过渡 */
  padding: 80px;
  box-sizing: border-box; /* 确保内边距不会影响元素宽度 */
  text-align: center;
  z-index: 2; 
}
/* 当鼠标悬停在 .image 元素上时，.overlay 元素的样式变化 */
.image:hover .overlay {
  opacity: 1; /* 鼠标悬停时显示覆盖层 */
}
.text{
  display: flex;
  flex-direction: column; 
  align-items: flex-start;
  /* justify-content: center;  */
  width: 49%;
  height: auto;
  padding:30px;
  position: relative; /* 使得按钮能定位在 text 内部 */
  box-sizing: border-box;
}

.name, .price,.store{
  margin-top: 5px;;
  text-align: left; /*文字左对齐*/ 
  width:100%;
}
.price,.dis_price{
  font-size: 30px;
  color:rgb(139, 49, 31);
}
.price{
  /* border:2px solid #232020; */
  width:40%;
  /* background-color: #CCCCCC; */
}
.dis_price{
  display: flex;
  align-items: center;
  justify-content: left;
  flex-direction: row; 
}
.dis{
  background-color: rgba(27, 25, 25,0.5);
  padding:5px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-top-left-radius:5px;
  border-bottom-left-radius:5px;

}
.dis,.original_and_dis_price{
  width:50%;
  height:100%;
}
.original_price{
  font-size: 20px;
  text-decoration: line-through;
  color:rgb(0, 0, 0);
  background-color: #CCCCCC;
  border-top-right-radius:5px;
}
.final_price{
  font-size:25px;
  background-color: rgba(0, 0, 0,0.3);
  overflow: hidden;
  padding:3px;
  box-sizing: border-box;
  text-align: left;
  border-bottom-right-radius:5px;
}
.name{
  font-size: 25px;
}
.store{
  border-radius:5px;
  border: 1px solid #ddd; /* 浅灰色边界 */
  padding: 5px; /* 为内容添加一些内边距，以便边界不会直接贴近内容 */
  box-sizing: border-box; /* 确保内边距和边界不会影响元素的宽度 */
  background-color: #CCCCCC;
  font-size: 20px;
  transition: transform 0.3s ease, box-shadow 0.3s ease; /* 平滑过渡效果 */
  will-change: transform;/*告诉浏览器将要发生变化 */
}

.store:hover {
  transform: scale(1.005); /* 放大 5% */
  cursor: pointer;
}
.fromwhere,.baozhang{
  display: flex;
  align-items: center;
  justify-content: left;
  flex-direction: row; 
  margin-top: 5px;
}
.from1,.baozhang1{
  font-size: 20px;
  color:#CCCCCC;
}
.from2,.baozhang2{
  font-size: 20px;
  color:#000000;
  padding-left:20px ;
}
.starProduct-button:hover ,.starStore-button:hover,.enterStore-button:hover,.enter-button:hover{
  color: black; /* 鼠标悬停时文字颜色保持不变 */
  background-color: white; /* 鼠标悬停时背景颜色保持不变 */
  border: 2px solid black; /* 鼠标悬停时边框颜色保持不变 */
}
.starProduct-button .icon ,.starStore-button .icon,.enterStore-button .icon{
  width: 20px; /* 调整图标大小 */
  height: 20px; /* 调整图标大小 */
  margin-right: 8px; /* 图标与文本之间的间距 */
}
.storeName{
  font-size: 20px;
  /* padding-top:5px; */
}
.storeScore{
  font-size:20px;
  color:rgba(0, 0, 0,0.3);
  padding-left:20px;
  /* padding-top:5px; */
}


/* 侧边栏内部 */
.sidebar {
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); /* 添加阴影效果（可选） */
  box-sizing: border-box; /* 使内边距和边框包含在宽度和高度内 */
  border-radius: 15px; 
  width: 20%;
  background-color: #FFFFFF;
  color: #000000;
  margin-right: 10px;
  padding: 10px;
  border-radius:15px ;
}

.nav-item {
  padding: 10px;
  cursor: pointer;
  border-bottom: 1px solid rgba(0,0,0,0.4);
}

.nav-item:hover {
  border-bottom: 2px solid rgba(0,0,0,1);
}

/* 主内容区域 */
.main-content {
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); /* 添加阴影效果（可选） */
  box-sizing: border-box; /* 使内边距和边框包含在宽度和高度内 */
  border-radius: 15px; 
  background-color: #FFFFFF;
  color: #000000;
  border-radius:15px ;
  width: 100%;
  padding: 10px;
}
.loading-text{
  font-size:22px;
}
.Avatar{
  width: 30px; 
  height: 30px; 
  border-radius: 10px;
  object-fit: cover;
}
.doubleprice{
  position: relative;
  display: flex;
  flex-direction: row; 
  justify-items: center;
}
.discountPrice{
  text-decoration: line-through;
  font-size:15px;
  color:rgba(0,0,0,0.4);
}
.preview-section{
  padding-left:20px;
  padding-right:20px;
  display: flex;
  flex-wrap: wrap;
  /* gap: 20px; */
  justify-content: space-between;
  width:100%;
}
.displayProductItem{
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3); /* 添加阴影效果（可选） */
  box-sizing: border-box; /* 使内边距和边框包含在宽度和高度内 */
  border-radius: 15px; 
  transition: transform 0.3s, box-shadow 0.3s;
  display: grid;
  width: 200px;
  height: 200px;
  margin-top:10px;
  margin-bottom:10px;
  padding:20px;
}
.displayProductItem:hover{
  transform: translateY(-2px);
}
.product-image{
  border-radius: 15px; 
  width:100%;
  height:auto;
  max-height:90px;
}
.product-item h2 {
  font-size: 18px;
  margin: 0 0 10px;
}

.product-item p {
  font-size: 16px;
  margin: 0 0 10px;
}

.product-text{
  align-self: end;
}
</style> 
  