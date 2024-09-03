<!-- 卖家用户的订单中心页面 -->
<script setup lang="ts">
import { ref ,computed} from 'vue';
import { ElMenu,ElButton, ElInput,ElRate,ElMessage} from 'element-plus';
import 'element-plus/dist/index.css';
import Navbar from '../components/Navbar.vue';
import axiosInstance from '../components/axios';
import router from '@/router';
import Loading from '../views/templates/4.vue';

const userId =localStorage.getItem('userId');
console.log('id',userId);
const option=ref(1);
const currentRow_cancel=ref(null);
const isLoading = ref(false);
const currentRow_return=ref(null);
const currentRow_star=ref(null);
interface Order {
  id: string;
  product: string;
  productId: string;
  time: string;
  store: string;
  status: string;
  price:number;
  totalPay: number;
  actualPay: number;
  pic: string;
  dialogVisible: boolean;
  dialogVisible_order: boolean;
  isStar:boolean,
  star: number | null;
  starVisible: boolean;
}

const filteredOrders = computed(() => {
      if (option.value==1) {
        return myOrders.value;
      }
      const statusMapping: { [key: number]: string } = {
        2: '待付款',
        3: '已付款', 
        4: '运输中',
        5: '已送到',
        6: '待退货',
        7: '已退货',
      };
      return myOrders.value.filter(order => order.status === statusMapping[option.value]);
    });

// const orders=ref([
//        {
//           id: 1,
//           number:'11111',
//           time:'2024-8-17',
//           product:[{
//           id:11,
//           image: 'src/assets/czw/order1.png',
//           productName: '星',
//           price: 23,
//           },
//           {
//             id:12,
          
//           image: 'src/assets/czw/order2.png',
//           productName: '穹',
         
//           price: 23,
//           }],
//           orderStatus: '已发货',
//           state:1,
//           shopName: '匹诺康尼',
//           price:46,
//           star:null,
//           starVisible:false,
//           dialogVisible:false,
//         },
//         {
//           id: 2,
//           number:'22222',
//           time:'2024-8-23',
//           product:[{
//           id:21,
//           image: 'src/assets/czw/order3.png',
//           productName: '三月七',
         
//           price: 30,
//           }],
//           orderStatus: '未发货',
//           state:1,
//           shopName: '星穹列车',
//           price: 30,
//           star:null,
//           starVisible:false,
//           dialogVisible:false,
//         },
//         {
//           id: 3,
//           number:'3333',
//           time:'2024-9-30',
//           product:[{
//           id:31,
//           image: 'src/assets/czw/order4.png',
//           productName: '丹恒',
         
//           price: 10,
        
//           }],
//           orderStatus: '待评价',
//           state:1,
//           shopName: '星核猎手',
//           price: 10,
//           star:null,
//           starVisible:false,
//           dialogVisible:false,
//         },
//         {
//           id: 4,
//           number:'22222',
//           time:'2024-8-23',
//           product:[{
//           id:21,
//           image: 'src/assets/czw/order3.png',
//           productName: '三月七',
          
//           price: 30,
          
//           }],
//           orderStatus: '交易成功',
//           state:1,
//           shopName: '星穹列车',
//           price: 30,
//           star:4,
//           starVisible:false,
//           dialogVisible:false,
//         },
//         {
//           id: 5,
//           number:'3333',
//           time:'2024-9-30',
//           product:[{
//           id:31,
//           image: 'src/assets/czw/order4.png',
//           productName: '丹恒',
         
//           price: 10,
//           }],
//           orderStatus: '交易成功',
//           state:1,
//           shopName: '星核猎手',
//           price: 10,
//           star:null,
//           starVisible:false,
//           dialogVisible:false,
//         },
//         {
//           id: 6,
//           number:'22222',
//           time:'2024-8-23',
//           product:[{
//           id:21,
//           image: 'src/assets/czw/order3.png',
//           productName: '三月七',
//           price: 30,
//           }],
//           orderStatus: '待评价',
//           state:1,
//           shopName: '星穹列车',
//           price: 30,
//           star:null,
//           starVisible:false,
//           dialogVisible:false,
//         },
//         {
//           id: 7,
//           number:'3333',
//           time:'2024-9-30',
//           product:[{
//           id:31,
//           image: 'src/assets/czw/order4.png',
//           productName: '丹恒',
//           price: 10,
//           }],
//           orderStatus: '未发货',
//           state:1,
//           shopName: '星核猎手',
//           price: 10,
//           star:null,
//           starVisible:false,
//           dialogVisible:false,
//         },
// ]);
const myOrders = ref<Order[]>([]);
  const getMyPost = async () => {
  axiosInstance.get('/Payment/GetAllOrders', {
        params: {
       buyerId: userId
     }
      })
    .then(response => {
        const data = response.data;
        console.log('Raw response data: ', response.data);
        if(data==null){
          isLoading.value=false;
        }
        else if (data && Array.isArray(data)) {
          myOrders.value = data.map((order: any) => ({
            id: order.orderId || '',
            product: order.productName || '',
            productId:order.productId,
            time: order.createTime || '',
            store: order.storeName || '',
            status: order.orderStatus || '',
            price:order.price||0,
            totalPay: order.totalPay || 0,
            actualPay: order.actualPay || 0,
            pic: order.picture || '',
            star: null,
            dialogVisible: false,
            dialogVisible_order:false,
            starVisible: false,
            isStar:false,

            }));
            console.log("成功获得订单");
            console.log(myOrders.value);
            myOrders.value.forEach(order => {
            isStarred(order);
            isLoading.value=false;
          });

        } else {
            console.error('Invalid data format.');
        }
    }).catch(error => {
        console.error(error);
    });
};
getMyPost();
const isStarred = async (order: Order) => {
  axiosInstance.get('/Shopping/CheckOrderRemark', {
        params: {
        orderId: order.id
     }
      })
    .then(response => {
      const isStarred = response.data; // 直接获取布尔结果
      order.isStar = isStarred;
    }).catch(error => {
        console.error(error);
    });
};
const returnProduct=ref('');
const returnProduct_else=ref('');
const comment=ref('');
// const admit = (order) => {
//   order.dialogVisible = false;
//   resetForm(order);
// }
const admit = async (order) => {
  if(returnProduct.value=='其他'){
    returnProduct.value=returnProduct_else.value;
  }
    axiosInstance.post('/Payment/AddReturn', {
      orderId: order.id,
      returnReason: returnProduct.value
    }, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
   })
  .then(response => {
    resetForm(order);
    console.log('submitting return:成功');
    ElMessage({
        message: '申请退货成功',
        type: 'success',
        });
    }).catch(error => {
      console.error('Error submitting return:', error);
    });
};
const admitOrder = async (order) => {
  if(returnProduct.value=='其他'){
    returnProduct.value=returnProduct_else.value;
  }
    axiosInstance.delete('/Payment/DeleteOrders', {
      params: {
        orderId: order.id
      }
   })
  .then(response => {
    resetForm(order);
    console.log('取消订单:成功');
    ElMessage({
        message: '成功取消订单',
        type: 'success',
        });
    }).catch(error => {
      console.error('Error submitting return:', error);
    });
};
// function confirmStar(order)
// {
//   row.starVisible = false;
//   row.orderStatus = '交易成功';
// }
const confirmStar = async (order) => {
    axiosInstance.put('/Shopping/OrderRemark', {
      orderId: order.id,
      remark: comment.value,
      score: order.star,
    }, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
})
  .then(response => {
    console.log('Rating submitted:', response.data);
    order.starVisible=false;
    comment.value='';
    ElMessage({
        message: '评价成功',
        type: 'success',
        });
    }).catch(error => {
      console.error('Error submitting rating:', error);
    });
};
function startRating(order) {
  order.starVisible = true;
  currentRow_star.value=order;
};
function returnOrder(order) {
  order.dialogVisible = true;
  currentRow_return.value=order;
};
function cancelOrder(order) {
  order.dialogVisible_order = true;
  currentRow_cancel.value=order;
};
const resetForm = (order) => {
  order.dialogVisible = false;
  order.dialogVisible_order=false;
  returnProduct.value = '';
  returnProduct_else.value=''
}
function resetStar(order)
{
  order.star=null;
  order.starVisible=false;
}
// function deleteRow(order) {
//   this.orders = this.orders.filter(order => order !== row);
//   this.filteredOrders = this.filteredOrders.filter(order => order !== row);
// }
const product = ref({
      name: '',
      picture: '',
      price: 0,//原价格
      storeName: '',
      discountPrice: 0,//折后价格
      finalPrice:0,//最终支付的价格，考虑到会有积分使用
});
function gotoDetail(order:Order){
  product.value.name=order.product;
  product.value.picture=order.pic;
  product.value.price=order.price;
  product.value.storeName=order.store;
  product.value.discountPrice=order.totalPay;
  product.value.finalPrice=order.actualPay;
  const productStr = JSON.stringify(product.value);//序列化对象
  localStorage.setItem('productIdOfDetail',order.productId);
  localStorage.setItem('routerPath','/ordercentre');
  if(order.status=='待付款'){
    router.push({path:'/pay',query:{product: productStr,isPaid:'false'}});
  }
  else{
    router.push({path:'/pay',query:{product: productStr,isPaid:'true'}});
  }
}
const  colors=['#99A9BF', '#F7BA2A', '#FF9900'] ;// 等同于 { 2: '#99A9BF', 4: { value: '#F7BA2A', excluded: true }, 5: '#FF9900' }
const activeIndex = ref('1');
const menuItems = ref([
  { index: 1, title: '全部' },
  { index: 2, title: '待付款' },
  { index: 3, title: '已付款' },
  { index: 4, title: '运输中' },
  { index: 5, title: '已送达' },
  { index: 6, title: '待退货' },
  { index: 7, title: '已退货' },
]);
const menuChange = (index) => {
  activeIndex.value = index.toString();
  option.value = parseInt(index);
};
</script>

<template>
    <Navbar />
    <el-container>
      <el-aside width="18vh" style="background-color:  #82111f; ">
  <div class="big-title" style="display: flex; justify-content: center; align-items: center; width: 100%;">
    <img class="image" src="@/assets/czw/aside.svg" alt="Original Image"  />
    <span>订单</span>
    <img class="flipped-image" src="@/assets/czw/aside.svg" alt="Flipped Image" />
  </div>
  <div style="width: 100%;">
    <el-menu
      :default-active="activeIndex"
      mode="vertical"
      background-color="#ffffff"
      text-color="black"
      active-text-color="#d42517"
      @select="menuChange"
      style="width: 100%;"
    >
      <el-menu-item 
        v-for="item in menuItems" 
        :key="item.index" 
        :index="item.index.toString()"
        
      >
        <span class="el-icon-menu">{{ item.title }}</span>
      </el-menu-item>
    </el-menu>
  </div>
</el-aside>
  <el-container>
    <el-header style="text-align: left">
    <div style="line-height: 6vh;">
      <span v-if="option === 1" style="font-size: 2vh; color: #333;">全部</span>
      <span v-if="option === 2" style="font-size: 2vh; color: #333;">待付款</span>
      <span v-if="option === 3" style="font-size: 2vh; color: #333;">已付款</span>
      <span v-if="option === 4" style="font-size: 2vh; color: #333;">运输中</span>
      <span v-if="option === 5" style="font-size: 2vh; color: #333;">已送达</span>
      <span v-if="option === 6" style="font-size: 2vh; color: #333;">待退货</span>
      <span v-if="option === 7" style="font-size: 2vh; color: #333;">已退货</span>
    </div>
  </el-header>
  <Loading v-show="isLoading" />

  <el-table v-show="!isLoading" :data="filteredOrders" style="width: 100%">
    <el-table-column label="商品信息">
    <template v-slot="scope">
      <div class="order-header">
        <p>{{ scope.row.time }} 订单号: {{ scope.row.id }}</p>
      </div>
      <div style="display: flex;">
        <img :src="`data:image/png;base64,${scope.row.pic}`" style="max-width: 100px; max-height: 100px; margin-bottom: 5px;" />
        <div style="margin-left: 10px;">
          <p>{{ scope.row.productName }}</p>
          <p>价格:{{ scope.row.actualPay }}</p>
        </div>
      </div>
    </template>
  </el-table-column>
    <el-table-column prop="store" label="店铺" width="160"></el-table-column>
    <el-table-column prop="province" label="商品操作" width="160">
      <template  v-slot="scope">
      <div>
      <el-button type="text" v-if="scope.row.status === '已送达'" @click="returnOrder(scope.row)">退货</el-button>
      </div>
      <div>
      <el-button type="text" v-if="scope.row.status === '已付款'" @click="cancelOrder(scope.row)">取消订单</el-button>
      </div>
    </template>
    </el-table-column>
    <el-table-column prop="actualPay" label="实付款" width="160"></el-table-column>
    <el-table-column prop="status" label="交易状态" width="160"></el-table-column>
    <el-table-column prop="zip" label="操作" width="160">
    <template v-slot="scope">
      <el-button type="text" v-if="scope.row.isStar === false" @click="startRating(scope.row)">去评价</el-button>
    </template>
  </el-table-column>
  <el-table-column prop="o" label="订单详情" width="160">
    <template v-slot="scope">
    <el-button type="text" v-if="scope.row.status === '待付款'" @click="gotoDetail(scope.row)">去付款</el-button>
    <el-button type="text" v-else @click="gotoDetail(scope.row)">点击详情</el-button>
    </template>
  </el-table-column>
  </el-table>
  </el-container>
  <div v-if="currentRow_star!=null">
  <el-dialog
      customClass="custom-dialog"
        :draggable="true"
        :style="{ zIndex: 9999 }"
       :model-append-to-body="true"
        :modal="false"
        v-model="currentRow_star.starVisible"
        title="评价"
        width="460px"
        @close="resetStar(currentRow_star)"
         :z-index="2000"
      >
        <el-rate v-if="currentRow_star.starVisible" v-model="currentRow_star.star" :colors="colors"></el-rate>
        <el-input v-if="currentRow_star.starVisible" v-model="comment" placeholder="请输入评价" class="oo"></el-input>
        <el-button type="primary" @click="confirmStar(currentRow_star)">确定</el-button>
      </el-dialog>
    </div>
    <div v-if="currentRow_cancel!=null">
    <el-dialog
      customClass="custom-dialog"
      :draggable="true"
      :modalAppendToBody="false" 
      :modal="false"
       v-model="currentRow_cancel.dialogVisible_order" title="取消订单原因" width="460px" @close="resetForm(currentRow_cancel)">
     <el-select v-model="returnProduct" placeholder="请选择原因">
        <el-option label="买错了" value="买错了"></el-option>
        <el-option label="不想要了" value="不想要了"></el-option>
        <el-option label="其他" value="其他"></el-option>
      </el-select>
      <div style="margin-top: 10px;"></div>
      <el-input v-if="returnProduct === '其他' "v-model="returnProduct_else" placeholder="请输入原因"></el-input>
      <el-divider></el-divider>
      <div slot="footer" class="dialog-footer">
    <el-button type="primary" @click="admitOrder(currentRow_cancel)">确 定</el-button>
    </div>
      </el-dialog>
    </div>
    <div v-if="currentRow_return!=null">
    <el-dialog
      customClass="custom-dialog"
      :draggable="true"
      :modalAppendToBody="false" 
      :modal="false"
       v-model="currentRow_return.dialogVisible" title="退货原因" width="460px" @close="resetForm(currentRow_return)">
     <el-select v-model="returnProduct" placeholder="请选择原因">
        <el-option label="商品与图片不符" value="商品与图片不符"></el-option>
        <el-option label="商品破损" value="商品破损"></el-option>
        <el-option label="商品假冒" value="商品假冒"></el-option>
        <el-option label="不想要了" value="不想要了"></el-option>
        <el-option label="其他" value="其他"></el-option>
      </el-select>
      <div style="margin-top: 10px;"></div>
      <el-input v-if="returnProduct === '其他' "v-model="returnProduct_else" placeholder="请输入原因"></el-input>
      <el-divider></el-divider>
      <div slot="footer" class="dialog-footer">
    <el-button type="primary" @click="admit(currentRow_return)">确 定</el-button>
    </div>
      </el-dialog>
    </div>
</el-container>
</template>


<style scoped>
.oo{
  margin-bottom: 3vh;
}
.custom-dialog {
  position: absolute;
    top: 100%; /* 让评价组件在菜单项的下方 */
    left: 0;
    z-index: 1000; /* 提高层级以确保其显示在前面 */
}
.image {
    width: 30px; /* 根据需求调整大小 */
    height: auto;
    /* transform: rotate(-90deg);*/
  }
  
  .flipped-image {
    width: 30px; /* 确保与第一张图一致 */
    height: auto;
    transform: scaleX(-1); /* 使用CSS进行垂直翻转 */

  }
  .big-title{
    font-size:4vh;  /* 将字体大小设置为32像素 */
    font-weight: bold; /* 可选：让文字加粗 */
    text-align: center; /* 可选：使文字水平居中 */
    margin: 20px 0; /* 可选：设置上下的外边距 */
    font-family: "华文中宋", sans-serif;
    color: #ffffff; /* 设置文字颜色为白色 */
  }
  .el-menu {
    position: relative;
    overflow: hidden;
  }
  
  .el-menu-item {
    position: relative;
    transition: background-color 0.3s ease;
  }
  
  .el-menu-item::before {
    content: '';
    position: absolute;
    left: 0;
    top: 0;
    width: 0;
    height: 100%;
    background-color: #bdaead;
    z-index: 0;
    transition: width 0.3s ease;
  }
  
  .el-menu-item:hover::before {
    width: 100%;
  }
  
  .el-menu-item > span {
    position: relative;
    z-index: 1; /* 确保文本始终在滑块上方 */
    transition: color 0.3s ease;
  }
  
  .el-menu-item:hover {
    color: #ffffff;
  }
  .el-icon-menu{
    font-size:2vh;
    font-weight: bold; /* 可选：让文字加粗 */  
    font-family: "华文中宋", sans-serif;
  }
.el-header {
  background-color: #ffffff;
  color: #333;
  line-height: 18vh;
}
.order_img {
  max-width: 37vh;
  max-height: 37vh;
}
.order-divider {
  border-bottom: 1px solid #839fe0; /* 添加底部边框样式 */
  width: 100%;
}

</style>
