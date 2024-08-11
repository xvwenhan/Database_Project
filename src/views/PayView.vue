<template>
    <div class="Pcontainer">
        <div class="procedure">
            <div class="procedure1">
                <img src="@/assets/mmy/number1.svg" class="number"/>
                <div class="text">1.确认订单信息</div>
            </div>
            <div class="line">————————————</div>
            <div class="procedure2">
                <img src="@/assets/mmy/number2.svg" class="number"/>
                <div class="text">2.支付</div>
            </div>
        </div>
        <div class="address">
            <img class="addressImage" src="@/assets/mmy/location.svg">
            <div class="text1">寄送至&nbsp&nbsp&nbsp&nbsp</div>
            <div class="customerAddress">{{ customer.address }}&nbsp&nbsp({{ customer.name }}&nbsp收)&nbsp&nbsp</div>
            <el-button
                @click="openDialog"
                class="changeAddress"
                style="display: flex;
                align-items: center;
                justify-content: center;
                font-size: 20px;
                border-radius: 15px;
                border: 2px solid rgba(0,0,0,0.4);
                cursor: pointer;
                width: auto;
                height:80%;
                padding-left:20px;
                padding-right:20px;
                right:5%;
                bottom:10%;
                position: absolute" 
            >修改地址</el-button>
            <!-- 弹窗写法参考 -->
            <!-- 圆角设置 -->
            <el-dialog 
                v-model="dialogVisible"
                width="35%"
                :style="{borderRadius:'15px'}"
                >
                <div class="dialog-content">
                    
                    <div class="dialog-line">
                        <div class="dialog-text">&nbsp收&nbsp件&nbsp人&nbsp </div>
                        <el-input 
                        v-model="customer.name" 
                        placeholder="请输入收件人姓名" 
                        clearable 
                        style="width: 350px;
                        height:40px;
                        font-size:17px;"
                        ></el-input>
                    </div>
                    <!-- <div class="dialog-line">
                        <div class="dialog-text">&nbsp手&nbsp机&nbsp号&nbsp </div>
                        <el-input 
                        v-model="customer.phone" 
                        placeholder="请输入收件人手机号" 
                        clearable 
                        style="width: 350px;
                        height:40px;
                        font-size:17px;"
                        ></el-input>
                    </div> -->
                    <div class="dialog-line">
                        <div class="dialog-text">所在地区 </div>
                        <el-cascader :options='options' 
                        v-model='selectedOptions'
                        placeholder="省/市/区（县）"
                        class="custom-cascader"
                        style="width: 350px;"
                         @change='addressChange' 
                         ></el-cascader>
                     </div>
                    <div class="dialog-line">
                        <div class="dialog-text">详细地址 </div>
                        <el-input 
                        v-model="address2" 
                        placeholder="请输入详细地址（具体到小区、门牌号等）" 
                        clearable 
                        style="width: 350px;
                        height:40px;
                        font-size:17px;"
                        ></el-input>
                    </div>
                </div>
                <span slot="footer" class="dialog-footer">
                <el-button 
                style="border: 2px solid rgba(0,0,0,0.4);
                width: 20%;
                height:40px;
                font-size:20px;">取 消</el-button>
                <el-button 
                 @click="handleSave"
                 style="border: 2px solid rgba(0,0,0,0.4);
                 background-color:rgba(0,0,0,0.4);
                 color:white;
                 width: 20%;
                 height:40px;
                 font-size:20px;">确 定</el-button>
                </span>
                </el-dialog>
               
        </div>
        <div class="orderInfo">
            <div class="storeArea">
                <img src="@/assets/mmy/store.svg" class="storeImage">
                <div class="text2">{{ product.storeName }}&nbsp&nbsp></div>
            </div>
            <div class="productArea">
                <img :src="`data:image/png;base64,${product.picture}`" class="productImage">
                <div class="orderDetail">
                    <div class="text-p">{{ product.name }}&nbsp&nbsp￥{{ product.price }}</div>
                    <div class="text-p1">订单编号:&nbsp&nbsp{{ order.id }}</div>
                    <div class="text-p1">创建时间:&nbsp&nbsp{{ order.createTime }}</div>
                </div>
            </div>
        </div>
        <div class="price">
            <div class="text3">￥价格明细</div>
            <div class="text-price">商品原价：&nbsp&nbsp{{ (product.price) }}元</div>
            <div class="text-price">活动折扣：&nbsp&nbsp{{ product.discountPrice===product.price?'本商品未参与活动': '* '+product.discount*100+'%'}}</div>
            <div class="text-price">折扣价格：&nbsp&nbsp{{ product.discountPrice }}元</div>
            <!-- 注意vue中的整除 -->
            <div class="text-price">积分抵扣：当前积分{{ customer.credits }},可抵扣{{ creditPrice }}元</div>
            <div class="text-price" v-show="creditPrice!==0">
                <div class="useCredit">
                    <div>是否使用积分</div>
                    <el-radio-group  v-model="isUseCredits" style="margin-left: 10px;margin-bottom:0px" @change="priceCalculate">
                    <el-radio label="yes">是</el-radio>
                    <el-radio label="no">否</el-radio>
                    </el-radio-group>
                </div>
            </div>
            <div class="text-price">价格合计：{{ finalPrice }}元</div>
        </div>
        <div class="pay">
            <el-button
                @click="openPay"
                class="payMoney"
                style="display: flex;
                align-items: flex-end;
                justify-content: center;
                font-size: 22px;
                border-radius: 15px;
                border: 2px solid rgba(0,0,0,0.4);
                background-color: rgba(0,0,0,0.4);
                color:white;
                cursor: pointer;
                width: auto;
                height:80%;
                right:5%;
                bottom:10%;
                position: absolute" 
            >支付￥{{ finalPrice }}</el-button>
            <el-dialog 
                v-model="payVisible"
                width="20%"
                :style="{borderRadius:'15px'}"
                >
                <div v-show="isPaySuccess===true">
                    <p class="text_pay_isSuccess">支付成功</p>
                    <p class="text_pay">获得积分：&nbsp&nbsp{{ bonusCredits }}</p>
                    <p class="text_pay">积分余额：&nbsp&nbsp{{ finalCredits }}</p>
                </div>
                <div v-show="isPaySuccess===false">
                    <p class="text_pay_isSuccess">支付失败</p>
                    <p class="text_pay">钱包余额不足，请及时充值</p>
                </div>

            </el-dialog>

        </div>
    </div>
</template>

<script setup lang="ts">
import { ref ,computed,onMounted,watch} from 'vue';
import { ElDialog, ElButton ,ElMessage} from 'element-plus';
import { regionData, codeToText } from 'element-china-area-data';
import axiosInstance from '../components/axios';
import { useRouter } from 'vue-router';
//接收路由参数
import { useRoute } from 'vue-router';

const route = useRoute();
const userId = computed(() => route.query.userId as string);
const productId = computed(() => route.query.productId as string);
const product = computed(() => {
  const productStr = route.query.product as string | undefined;
  return productStr ? JSON.parse(productStr) : null;
});
const order=ref({id:'',createTime:''});
//收货人相关信息
const customer=ref({name:'',
                address:'',
                credits:300});
                const dialogVisible=ref(false);
const payVisible=ref(false);
const address1=ref('');
const address2=ref('');
const isUseCredits=ref('no');
//用了toFixed(2)就是string类型的了
const creditPrice=ref(0);
const finalPrice=ref(product.value.discountPrice);
// 省市区信息
const options = ref(regionData);
const selectedOptions = ref(['110000', '110100', '110101']);
//后端回复信息
const message=ref('');
//支付后获得积分以及剩余积分
const bonusCredits=ref(0);
const finalCredits=ref(0);
//是否支付成功
const isPaySuccess=ref(false);
////订单支付结束后（不管支付成功没成功）都跳转回原来的页面
const router=useRouter();
const routerPath=localStorage.getItem('routerPath');
watch(payVisible, (newValue, oldValue) => {
    if (newValue === false && oldValue === true) {
        // 当 `isPaySuccess` 变为 `false` 时执行操作
        console.log('支付失败，状态变为 false');
        // 跳转到指定页面
        const path = routerPath ? routerPath : '/home'; 
        router.push(path);
    }
});
onMounted(async () => {
    console.log(`product is ${JSON.stringify(product.value)}`);
    console.log(`productId is ${productId.value}`);
    console.log(`userId is ${userId.value}`);
    console.log(`creditPrice is ${creditPrice}`);

    const formData = new FormData();
    formData.append('BuyerId',userId.value);
    formData.append('ProductId',productId.value);
    try {
        const response = await axiosInstance.post('Payment/AddOrders', formData,{
            headers: { 'Content-Type': 'multipart/form-data' }
       });
        order.value.id=response.data.orderId;
        order.value.createTime=response.data.createTime;
        customer.value.name=response.data.username;
        console.log(`customer.value.name is ${customer.value.name}`)
        if(customer.value.name==null){
            customer.value.name='未知收货人';
        }
        customer.value.address=response.data.address;
        if(customer.value.address==null){
            customer.value.address='未知地';
        }
        customer.value.credits=535;
        creditPrice.value=parseInt(Math.floor(customer.value.credits/100).toFixed(0));
        // customer.value.credits=response.data.credits;
    } catch (error) {
        console.log('订单生成失败');
     }
})

const openDialog=()=>{
    dialogVisible.value = true;
}
const handleSave = () => {
  if(!address1.value||!address2.value){
      ElMessage.error('请保证地址不为空');
  }else {
    dialogVisible.value = false;
    customer.value.address=address1.value+address2.value;
    console.log(`customer.value.address is ${customer.value.address}`);
  }
};
// 将地址编号转换为汉字
const addressChange=()=> {
  const arr=selectedOptions.value;
  address1.value=codeToText[arr[0]]+codeToText[arr[1]]+codeToText[arr[2]];
  console.log(address1);
}
//根据积分计算最终价格
const priceCalculate=()=>{
    // 确保 discountPrice 和 creditPrice 是数字进行计算
    const discount = parseFloat(product.value.discountPrice);
    const credits = creditPrice.value;

    if (isUseCredits.value === 'yes') {
        finalPrice.value = parseInt((discount - credits).toFixed(2)); // 计算最终价格并保留两位小数
    } else {
        finalPrice.value = parseInt(discount.toFixed(2)); // 如果不使用积分，最终价格就是折扣价格
    }
    if(finalPrice.value<0){
        finalPrice.value=0;
    }
}

//确认订单信息并完成支付
const openPay=async()=>{
    const formData = new FormData();
    formData.append('orderId',order.value.id);
    formData.append('order_address',customer.value.address);
    formData.append('username',customer.value.name);
    formData.append('actual_pay', finalPrice.value);//使用积分后的价格
    formData.append('total_pay',product.value.discountPrice);//打折后的价格
    formData.forEach((value, key) => {
        console.log(`${key}: ${value}`);
    });
    try {
    const response = await axiosInstance.put('Payment/ConfirmOrders', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    });
    bonusCredits.value = response.data.bonusCredits;
    finalCredits.value = response.data.credits;
    isPaySuccess.value=true;
    } catch (error) {
        isPaySuccess.value=false;
        if (error.response) {
        // 请求已发出，服务器返回了状态码
            console.error('响应错误状态码:', error.response.status);
            console.error('响应错误数据:', error.response.data);
            console.error('响应错误头:', error.response.headers);
        } else if (error.request) {
            // 请求已发出，但没有响应
            console.error('请求错误:', error.request);
        } else {
            // 其他错误
            console.error('错误信息:', error.message);
        }
        }
    message.value='';
    payVisible.value=true;
}
</script>

<style scoped>
.Pcontainer {
  background-color: rgba(204, 204, 204,0.4);
  display: flex;
  align-items: center;/*这才是水平对齐 */
  flex-direction: column;
  height: 100vh;
  overflow-x: hidden; /*隐藏水平滚动条 */
  overflow: auto;
  /* position:relative; */
}
.procedure{
  margin-top:20px;
  display: flex;
  align-items: center;/*这才是水平对齐 */
  flex-direction: row;
  align-items: right;
}
.procedure1,.procedure2{
    display: flex;
    flex-direction: column;
    align-items: center;/*这才是水平对齐 */
}
.number{
  width: 40px; /* 调整图标大小 */
  height: 40px; /* 调整图标大小 */
  margin-right: 8px; /* 图标与文本之间的间距 */
}
.text{
  font-size:15px;
}
.line{
    padding-bottom:15px ;
    padding-right:20px;
}
.address,.orderInfo,.price{
    background-color: #FFFFFF;
    width: 70%; 
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); /* 添加阴影效果（可选） */
    box-sizing: border-box; /* 使内边距和边框包含在宽度和高度内 */
    border-radius: 15px; 
    position:relative;
    padding:5px;
    margin-top:15px;
}
.address{
    display: flex;
    flex-direction: row;
    padding-left:20px;
    /* height:40px; */
    height:55px;
}
.addressImage{
    width:35px;
    height:35px;
}
.text1,.customerAddress{
    font-size:20px;
    padding-top:5px;
}
.changeAddress:hover{
    color: black; /* 鼠标悬停时文字颜色保持不变 */
    background-color: white; /* 鼠标悬停时背景颜色保持不变 */
    border: 2px solid black; /* 鼠标悬停时边框颜色保持不变 */
}
.dialog-content {
  display: flex;
  flex-direction: column; /* 使内容竖直排列 */
  gap: 20px; 
}
.dialog-line{
    display: flex;
    justify-content: center; 
    flex-direction: row;
}
.dialog-text{
    padding-right:10px;
    padding-top:5px ;
    font-size:20px;
}
.dialog-footer {
  display: flex;
  justify-content: center; 
  padding-top:25px;
}
.orderInfo{
    display: flex;
    flex-direction: column;
    height:250px;
    padding-left:20px;
}
.storeArea{
    display: flex;
    flex-direction: row;
    padding-top:10px;
}
.storeImage{
    width:30px;
    height:30px;
    padding-right:5px;
}
.productArea{
    margin-top:20px;
    height:150px;
    display: flex;
    flex-direction: row;
}
.productImage{
    height:100%;
    border-radius: 50%;
    margin-left:30px;
}
.orderDetail{
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    padding-left:50px;
}
.text2{
    font-size:20px;
}
.text-p{
    font-size:23px;
}
.text-p1{
    padding-top:15px;
    font-size:20px;
    color:rgba(0,0,0,0.6);
}
.price{
    display: flex;
    flex-direction: column;
    padding-left:40px;
    padding-right:40px;
    height:35%;
    align-items: center; 
    /* justify-content: center; */
    position:relative;
}
.text-price{
    font-size:20px;
}
.useCredit{
    display: flex;
    flex-direction: row;
}
.text3{
    font-size:22px;
    margin-top: 10px;
    width:100%;
    border-bottom:1.5px solid black ;
    /* padding-bottom: 10px; */
}
.text_pay_isSuccess{
    font-size:22px;
    margin-bottom: 10px;
}
.text_pay{
    margin-bottom: 7px;
    font-size:20px;
}

.pay{
    width: 70%; 
    position:relative;
    padding:5px;
    margin-top:15px;
    /* display: flex;
    align-items: center; 
    justify-content: center; */
    height:60px;
}
.payMoney:hover{
    transform: scale(1.02); /* Scale up by 20% */
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);
}

</style>