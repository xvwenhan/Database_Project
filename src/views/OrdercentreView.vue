<!-- 卖家用户的订单中心页面 -->
<script setup lang="ts">
import { ref ,computed} from 'vue';
import { ElMenu,ElButton, ElInput,ElRate} from 'element-plus';
import 'element-plus/dist/index.css';
import Navbar from '../components/Navbar.vue';
import axiosInstance from '../components/axios';
import router from '@/router';
const userId =localStorage.getItem('userId');
console.log('id',userId);

const option=ref(1);

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
  isStar:boolean,
  star: number | null;
  starVisible: boolean;
}
function menuChange(index) {
 option.value=index;
};
const filteredOrders = computed(() => {
      if (option.value==1) {
        return myOrders.value;
      }
      const statusMapping: { [key: number]: string } = {
        2: '待付款',
        3: '待发货',
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
        if (data && Array.isArray(data)) {
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
            starVisible: false,
            isStar:false,

            }));
            console.log("成功获得订单");
            console.log(myOrders.value);
            myOrders.value.forEach(order => {
            isStarred(order);
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
    }).catch(error => {
      console.error('Error submitting rating:', error);
    });
};
function startRating(order) {
  order.starVisible = true;
};
const resetForm = (order) => {
  order.dialogVisible = false;
  returnProduct.value = '';
  returnProduct_else.value=''
}
function resetStar(order)
{
  order.star=null;
  order.starVisible=false;
}
function deleteRow(order) {
  this.orders = this.orders.filter(order => order !== row);
  this.filteredOrders = this.filteredOrders.filter(order => order !== row);
}
const product = ref({
      name: '',
      picture: '',
      price: 0,//原价格
      storeName: '',
      discountPrice: 0,//折后价格
      finalPrice:0,//最终支付的价格，考虑到会有积分使用
    }) ;        
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

</script>

<template>
    <Navbar />
    <el-container>
    <el-aside width="200px" style="background-color: rgb(238, 241, 246)">
      <h2>订单中心</h2>
    <el-menu :default-openeds="['1', '7']">
      <el-menu-item index="1" @click="menuChange(1)">
        <i class="el-icon-menu" ></i>
        <span slot="title">全部</span>
      </el-menu-item>
      <el-menu-item index="2" @click="menuChange(2)">
        <i class="el-icon-menu"></i>
        <span slot="title">待付款</span>
      </el-menu-item>
      <el-menu-item index="3" @click="menuChange(3)">
        <i class="el-icon-menu"></i>
        <span slot="title">待发货</span>
      </el-menu-item>
      <el-menu-item index="4" @click="menuChange(4)">
        <i class="el-icon-menu"></i>
        <span slot="title">运输中</span>
      </el-menu-item>
      <el-menu-item index="5" @click="menuChange(5)">
        <i class="el-icon-menu"></i>
        <span slot="title">已送达</span>
      </el-menu-item>
      <el-menu-item index="6" @click="menuChange(6)">
        <i class="el-icon-menu"></i>
        <span slot="title">待退货</span>
      </el-menu-item>
      <el-menu-item index="7" @click="menuChange(7)">
        <i class="el-icon-menu"></i>
        <span slot="title">已退货</span>
      </el-menu-item>
    </el-menu>
  </el-aside>
  <el-container>
    <el-header style="text-align: left">
    <div style="line-height: 6vh;">
      <span v-if="option === 1" style="font-size: 2vh; color: #333;">全部</span>
      <span v-if="option === 2" style="font-size: 2vh; color: #333;">待付款</span>
      <span v-if="option === 3" style="font-size: 2vh; color: #333;">待发货</span>
      <span v-if="option === 4" style="font-size: 2vh; color: #333;">运输中</span>
      <span v-if="option === 5" style="font-size: 2vh; color: #333;">已送达</span>
      <span v-if="option === 6" style="font-size: 2vh; color: #333;">待退货</span>
      <span v-if="option === 7" style="font-size: 2vh; color: #333;">已退货</span>
    </div>
  </el-header>
  <el-table :data="filteredOrders" style="width: 100%">
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
      <div class="order-divider"></div>
    </template>
  </el-table-column>
    <el-table-column prop="store" label="店铺" width="160"></el-table-column>
    <el-table-column prop="province" label="商品操作" width="160">
      <template  v-slot="scope">
      <div>
      <el-button type="text" @click="scope.row.dialogVisible = true">退货</el-button>
      </div>
      <el-dialog
      :draggable="true"
      :modalAppendToBody="false" 
      :modal="false"
       v-model="scope.row.dialogVisible" title="退货原因" width="460px" @close="resetForm(scope.row)">
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
    <el-button type="primary" @click="admit(scope.row)">确 定</el-button>
    </div>
      </el-dialog>
    </template>
    </el-table-column>
    <el-table-column prop="actualPay" label="实付款" width="160"></el-table-column>
    <el-table-column prop="status" label="交易状态" width="160"></el-table-column>
    <el-table-column prop="zip" label="操作" width="160">
    <template v-slot="scope">
      <el-button v-if="scope.row.status === '已送达' &&scope.row.isStar === true"
        @click.native.prevent="deleteRow(scope.row)"
        type="text"
        size="small">
        移除
      </el-button>
      <el-button type="text" v-if="scope.row.isStar === false" @click="startRating(scope.row)">去评价</el-button>
      <el-dialog
        :draggable="true"
        :modalAppendToBody="false"
        :modal="false"
        v-model="scope.row.starVisible"
        title="评价"
        width="460px"
        @close="resetStar(scope.row)"
      >
        <el-rate v-if="scope.row.starVisible" v-model="scope.row.star" :colors="colors"></el-rate>
        <el-input v-if="scope.row.starVisible" v-model="comment" placeholder="请输入评价"></el-input>
        <el-button type="primary" @click="confirmStar(scope.row)">确定</el-button>
      </el-dialog>
    </template>
  </el-table-column>
  <el-table-column prop="o" label="订单详情" width="160">
    <template v-slot="scope">
    <el-button type="text"  @click="gotoDetail(scope.row)">点击详情</el-button>
    </template>
  </el-table-column>
  </el-table>
  </el-container>
</el-container>

</template>


<style scoped>
.el-header {
  background-color: #B3C0D1;
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
