<!-- 卖家用户的订单中心页面 -->
<script setup lang="ts">
import { ref } from 'vue';
import { ElMenu,ElButton, ElInput,ElRate} from 'element-plus';
import 'element-plus/dist/index.css';
import Navbar from '../components/Navbar.vue';
const option=ref(1);
function menuChange(index) {
  this.filteredOrders = [];
      for (let i = 0; i < this.orders.length; i++) {
        if (index === 1 ||
          (index === 2 && this.orders[i].orderStatus === '未发货') ||
          (index === 3 && this.orders[i].orderStatus === '已发货') ||
          (index === 4 && this.orders[i].orderStatus === '待评价')) {
          this.filteredOrders.push(this.orders[i]);
        }
      }
};
const orders=ref([
       {
          id: 1,
          number:'11111',
          time:'2024-8-17',
          product:[{
          id:11,
          image: 'src/assets/czw/order1.png',
          productName: '星',
          price: 23,
          },
          {
            id:12,
          
          image: 'src/assets/czw/order2.png',
          productName: '穹',
         
          price: 23,
          }],
          orderStatus: '已发货',
          state:1,
          shopName: '匹诺康尼',
          price:46,
          star:null,
          starVisible:false,
        },
        {
          id: 2,
          number:'22222',
          time:'2024-8-23',
          product:[{
          id:21,
          image: 'src/assets/czw/order3.png',
          productName: '三月七',
         
          price: 30,
          }],
          orderStatus: '未发货',
          state:1,
          shopName: '星穹列车',
          price: 30,
          star:null,
          starVisible:false,
        },
        {
          id: 3,
          number:'3333',
          time:'2024-9-30',
          product:[{
          id:31,
          image: 'src/assets/czw/order4.png',
          productName: '丹恒',
         
          price: 10,
        
          }],
          orderStatus: '待评价',
          state:1,
          shopName: '星核猎手',
          price: 10,
          star:null,
          starVisible:false,
        },
        {
          id: 4,
          number:'22222',
          time:'2024-8-23',
          product:[{
          id:21,
          image: 'src/assets/czw/order3.png',
          productName: '三月七',
          
          price: 30,
          
          }],
          orderStatus: '交易成功',
          state:1,
          shopName: '星穹列车',
          price: 30,
          star:4,
          starVisible:false,
        },
        {
          id: 5,
          number:'3333',
          time:'2024-9-30',
          product:[{
          id:31,
          image: 'src/assets/czw/order4.png',
          productName: '丹恒',
         
          price: 10,
          }],
          orderStatus: '交易成功',
          state:1,
          shopName: '星核猎手',
          price: 10,
          star:null,
          starVisible:false,
        },
        {
          id: 6,
          number:'22222',
          time:'2024-8-23',
          product:[{
          id:21,
          image: 'src/assets/czw/order3.png',
          productName: '三月七',
          price: 30,
          }],
          orderStatus: '待评价',
          state:1,
          shopName: '星穹列车',
          price: 30,
          star:null,
          starVisible:false,
        },
        {
          id: 7,
          number:'3333',
          time:'2024-9-30',
          product:[{
          id:31,
          image: 'src/assets/czw/order4.png',
          productName: '丹恒',
          price: 10,
          }],
          orderStatus: '未发货',
          state:1,
          shopName: '星核猎手',
          price: 10,
          star:null,
          starVisible:false,
        },
]);
const filteredOrders=ref([]);

const returnProduct=ref('');
const returnProduct_else=ref('');
const dialogVisible =ref(false);
function admit()
{
  dialogVisible.value=false;
}
function confirmStar(row)
{
  row.starVisible = false;
  row.orderStatus = '交易成功';
}
function startRating(row) {
  row.starVisible = true;
};
function resetForm()
{
  returnProduct.value = '';
  returnProduct_else.value = '';
}
function resetStar(row)
{
  row.star=null;
  row.starVisible=false;
}
function deleteRow(row) {
  this.orders = this.orders.filter(order => order !== row);
  this.filteredOrders = this.filteredOrders.filter(order => order !== row);
}
const  colors=['#99A9BF', '#F7BA2A', '#FF9900'] ;// 等同于 { 2: '#99A9BF', 4: { value: '#F7BA2A', excluded: true }, 5: '#FF9900' }

</script>

<template>
    <Navbar />
    <el-container>
    <el-aside width="200px" style="background-color: rgb(238, 241, 246)">
      <h2>订单中心</h2>
    <el-menu :default-openeds="['1', '4']">
      <el-menu-item index="1" @click="menuChange(1)">
        <i class="el-icon-menu" ></i>
        <span slot="title">全部</span>
      </el-menu-item>
      <el-menu-item index="2" @click="menuChange(2)">
        <i class="el-icon-menu"></i>
        <span slot="title">待发货</span>
      </el-menu-item>
      <el-menu-item index="3" @click="menuChange(3)">
        <i class="el-icon-menu"></i>
        <span slot="title">待收货</span>
      </el-menu-item>
      <el-menu-item index="4" @click="menuChange(4)">
        <i class="el-icon-menu"></i>
        <span slot="title">待评价</span>
      </el-menu-item>
    </el-menu>
  </el-aside>
  <el-container>
    <el-header style="text-align: left">
    <div style="line-height: 6vh;">
      <span v-if="option === 1" style="font-size: 2vh; color: #333;">全部</span>
      <span v-if="option === 2" style="font-size: 2vh; color: #333;">待发货</span>
      <span v-if="option === 3" style="font-size: 2vh; color: #333;">待收货</span>
      <span v-if="option === 4" style="font-size: 2vh; color: #333;">待评价</span>
    </div>
  </el-header>
  <el-table :data="filteredOrders" style="width: 100%">
    <el-table-column label="商品信息">
    <template v-slot="{ row }">
      <div class="order-header">
        <p>{{ row.time }} 订单号: {{ row.number }}</p>
      </div>
      <div v-for="item in row.product":key="item.id" style="display: flex;">
        <img :src="item.image" style="max-width: 100px; max-height: 100px; margin-bottom: 5px;" />
        <div style="margin-left: 10px;">
          <p>{{ item.productName }}</p>
          <p>价格:{{ item.price }}</p>
        </div>
      </div>
      <div class="order-divider"></div>
    </template>
  </el-table-column>
    <el-table-column prop="shopName" label="店铺" width="160"></el-table-column>
    <el-table-column prop="province" label="商品操作" width="160">
      <div>
      <el-button type="text" @click="dialogVisible = true">退货</el-button>
      </div>
      <el-dialog
      :draggable="true"
      :modalAppendToBody="false" 
      :modal="false"
       v-model="dialogVisible" title="退货原因" width="460px" @close="resetForm">
     <el-select v-model="returnProduct" placeholder="请选择原因">
        <el-option label="商品与图片不符" value="1"></el-option>
        <el-option label="商品破损" value="2"></el-option>
        <el-option label="商品假冒" value="3"></el-option>
        <el-option label="不想要了" value="4"></el-option>
        <el-option label="其他" value="5"></el-option>
      </el-select>
      <div style="margin-top: 10px;"></div>
      <el-input v-if="returnProduct === '5' "v-model="returnProduct_else" placeholder="请输入原因"></el-input>
      <el-divider></el-divider>
      <div slot="footer" class="dialog-footer">
    <el-button type="primary" @click="admit()">确 定</el-button>
    </div>
      </el-dialog>

    </el-table-column>
    <el-table-column prop="price" label="实付款" width="160"></el-table-column>
    <el-table-column prop="orderStatus" label="交易状态" width="160"></el-table-column>
    <el-table-column prop="zip" label="操作" width="160">
    <template v-slot="{row}">
      <el-button v-if="row.orderStatus === '交易成功'"
        @click.native.prevent="deleteRow(row)"
        type="text"
        size="small">
        移除
      </el-button>
      <el-button type="text" v-if="row.orderStatus === '待评价'" @click="startRating(row)">去评价</el-button>
      <el-dialog
        :draggable="true"
        :modalAppendToBody="false"
        :modal="false"
        v-model="row.starVisible"
        title="评价"
        width="460px"
        @close="resetStar(row)"
      >
        <el-rate v-if="row.starVisible" v-model="row.star" :colors="colors"></el-rate>
        <el-button type="primary" @click="confirmStar(row)">确定</el-button>
      </el-dialog>
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
