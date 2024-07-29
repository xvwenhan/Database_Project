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
            <div class="customerAddress">{{ customer.address }}&nbsp&nbsp({{ customer.name }}&nbsp收)&nbsp&nbsp{{ customer.phone }}</div>
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
                    <div class="dialog-line">
                        <div class="dialog-text">&nbsp手&nbsp机&nbsp号&nbsp </div>
                        <el-input 
                        v-model="customer.phone" 
                        placeholder="请输入收件人手机号" 
                        clearable 
                        style="width: 350px;
                        height:40px;
                        font-size:17px;"
                        ></el-input>
                    </div>
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
                <div class="text2">{{ product.store }}&nbsp&nbsp></div>
            </div>
            <div class="productArea">
                <img src="@/assets/mmy/product.jpg" class="productImage">
                <div class="orderDetail">
                    <div class="text-p">{{ product.name }}&nbsp&nbsp￥{{ product.price }}</div>
                    <div class="text-p1">订单编号:&nbsp&nbsp{{ order.id }}</div>
                    <div class="text-p1">创建时间:&nbsp&nbsp{{ order.createTime }}</div>
                </div>
            </div>
        </div>
        <div class="price">
            <div class="text3">￥价格明细</div>
            <div class="text-price">商品原价：&nbsp&nbsp{{ (product.price).toFixed(2) }}元</div>
            <div class="text-price">活动折扣：&nbsp&nbsp{{ product.discount==1?'本商品未参与活动': '* '+product.discount*100+'%'}}</div>
            <div class="text-price">折扣价格：&nbsp&nbsp{{ discountPrice }}元</div>
            <!-- 注意vue中的整除 -->
            <div class="text-price">积分抵扣：当前积分{{ customer.credits }},可抵扣{{ creditPrice }}元</div>
            <div class="text-price" v-show="creditPrice!=='0'">
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
                ></el-dialog>

        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { ElDialog, ElButton ,ElMessage} from 'element-plus';
import { regionData, codeToText } from 'element-china-area-data';
const product={name:'苏绣成品荷花手工刺绣精品',
                price:85,
                description:'有一点点瑕疵，不影响美观,等等等等等等等等等等等等等等等等',
                store:'苏绣世家',
                discount:0.7,
                fromwhere:'江苏省某某市',
                score:4.7};
const customer=ref({name:'mmy',
                address:'上海市嘉定区曹安路1111号',
                phone:123456789,
                credits:4300});
const order=ref({id:'000001123334',createTime:'2024-06-24 12:12:01'});
const dialogVisible=ref(false);
const payVisible=ref(false);
const address1=ref('');
const address2=ref('');
const isUseCredits=ref('no');
//用了toFixed(2)就是string类型的了
const discountPrice=(product.price*product.discount).toFixed(2);
const creditPrice=Math.floor(customer.value.credits/1000).toFixed(0);
const finalPrice=ref(discountPrice);
// 省市区信息
const options = ref(regionData);
const selectedOptions = ref(['110000', '110100', '110101']);

const openDialog=()=>{
    dialogVisible.value = true;
}
const handleSave = () => {
  if(!address1.value||!address2.value){
      ElMessage.error('请保证地址不为空');
  }else {
    dialogVisible.value = false;
    customer.value.address=address1.value+address2.value;
    console.log(customer.value.address);
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
    const discount = parseFloat(discountPrice);
    const credits = parseFloat(creditPrice);

    if (isUseCredits.value === 'yes') {
        finalPrice.value = (discount - credits).toFixed(2); // 计算最终价格并保留两位小数
    } else {
        finalPrice.value = discount.toFixed(2); // 如果不使用积分，最终价格就是折扣价格
    }}
const openPay=()=>{
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