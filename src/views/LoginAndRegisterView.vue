
<!-- npm install animate.css --save -->
<!-- el-put中，v-model用来双向绑定数据，clearable提供清除按钮，show-password可以调节可见性 -->
<template>
  <div class="base">
    <div class="loginAndRegist">
      <!-- 登录部分 -->
      <div class="loginArea" >
        <transition
                    name="animate__animated animate__bounce"
                    enter-active-class="animate__fadeInUp"
                    leave-active-class="animate__zoomOut"
                    appear
            >
            <div class="title" v-show="isLogin">LOGIN </div>
        </transition>
        <transition
                    name="animate__animated animate__bounce"
                    enter-active-class="animate__fadeInUp"
                    leave-active-class="animate__zoomOut"
                    appear
            >
            <div class="loginForm" v-show="isLogin">
          <el-input 
          v-model="loginEmail" 
          placeholder="请输入邮箱" 
          type="email" 
          clearable 
          style="width: 215px;margin-left: 10px;height:40px;margin-bottom: 30px"
        ></el-input>
          <el-input 
          v-model="password" 
          placeholder="请输入密码" 
          show-password 
          clearable 
          style="width: 215px;margin-left: 10px;height:40px;margin-bottom: 15px"
        ></el-input>
        <div>
          <label class="forget-password" @click="handleForgetPassword">忘记密码?</label>
        </div>
          <el-button type="success" round style="background-color: #257b5e; letter-spacing: 5px; width: 250px; margin-left: 10px;margin-top:20px" @click="login">登录</el-button>
        </div>
         </transition>
      </div>
      <!-- 注册部分 -->
      <div class="registerArea">
        <transition
                    name="animate__animated animate__bounce"
                    enter-active-class="animate__fadeInUp"
                    leave-active-class="animate__zoomOut"
                    appear
            >
            <div class="registForm" v-show="!isLogin">
      <el-radio-group v-show="!isLookfor" v-model="userType" style="margin-left: 10px;margin-bottom:0px">
        <el-radio label="buyer">买家</el-radio>
        <el-radio label="seller">商家</el-radio>
      </el-radio-group>
      <el-input 
        v-model="registerEmail" 
        placeholder="请输入邮箱" 
        clearable 
        type="email"
        style="width: 250px;margin-left: 10px;margin-top: 20px"
      ></el-input>
      <el-input 
        v-model="newPassword" 
        placeholder="请输入新密码" 
        clearable 
        show-password
        style="width: 250px;margin-left: 10px;margin-top: 20px"
      ></el-input>
      <el-input 
        v-model="confirmPassword" 
        placeholder="请再次输入新密码" 
        clearable 
        show-password
        style="width: 250px;margin-left: 10px;margin-top: 20px"
      ></el-input>
    <div >
      <el-input 
      v-model="verificationCode" 
      placeholder="请输入验证码" 
      clearable 
      style="width: 120px;margin-left: 10px;margin-top: 20px;margin-bottom: 30px"></el-input>
      <el-button type="success" round 
      style="background-color: #257b5e; 
      /*letter-spacing调整间距 */
      letter-spacing: 5px; 
      width: 120px; 
      margin-left: 10px;
      margin-top: 20px;
      margin-bottom: 30px" 
      @click="getVerificationCode">获取验证码</el-button>
    </div>
      <el-button v-show="!isLookfor" type="success" round style="background-color: #257b5e; letter-spacing: 5px; width: 250px; margin-left: 10px;margin-bottom: 40px" @click="register" >
        注册
      </el-button>
      <el-button v-show="isLookfor" type="success" round style="background-color: #257b5e; letter-spacing: 5px; width: 250px; margin-left: 10px;margin-bottom: 40px" @click="changPsw" >
        修改密码
      </el-button>
      </div>
          </transition>
      
     </div>
     <!-- 动态页面 -->
    <div id="aaa" class="showInfo"
              :style="{
             borderTopRightRadius:styleObj.bordertoprightradius,
             borderBottomRightRadius:styleObj.borderbottomrightradius,
             borderTopLeftRadius:styleObj.bordertopleftradius,
             borderBottomLeftRadius:styleObj.borderbottomleftradius,
             right:styleObj.rightDis
            }">
            <!-- 登录时的遮挡页 -->
            <div v-show="isLogin" class="button-container">
            <el-button type="success" style="background-color:#257B5E; border: 1px solid #ffffff;" round @click="changeToRegist">点击注册</el-button>
            </div>
           <!-- 注册或修改密码时的遮挡页 -->
            <div v-show="!isLogin" class="button-container" >
                <el-button type="success"  style="background-color:#257B5E;border: 1px solid #ffffff;" round @click="changeToLogin">返回登录</el-button>
           </div>
        </div>
     </div>
    </div>
</template>

<script setup>
import 'animate.css';
import { ref ,reactive} from 'vue';
import 'element-plus/dist/index.css';
import { ElInput, ElButton ,ElMessage} from 'element-plus';
// 输入变量
const userType = ref('buyer');
const loginEmail = ref('');
const registerEmail = ref('');
const password = ref('');
const newPassword=ref('');
const confirmPassword = ref('');
const verificationCode = ref('');
//后端返回验证码
const realVerificationCode=ref('');
//控制变量
const isLogin=ref(true);
const isLookfor=ref(false);
// 设置遮挡页的四角和位置
const styleObj = ref({
  bordertoprightradius: '15px',
  borderbottomrightradius: '15px',
  bordertopleftradius: '0px',
  borderbottomleftradius: '0px',
  rightDis: '0px'
});
//预定义数组
const dataArr=reactive([
  {email:"123456@qq.com",psw:"123456"},
  {email:"654321@qq.com",psw:"654321"}
])
const validateEmail = (email) => {
  const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return re.test(email);
};
// 重置变量
const clearData=()=>{
  userType.value='buyer';
  loginEmail.value='';
  registerEmail.value='';
  password.value='';
  newPassword.value='';
  confirmPassword.value='';
  verificationCode.value='';
  realVerificationCode.value='';
}
const login = () => {
      const user = dataArr.find(us =>loginEmail.value===us.email);
      if(!loginEmail.value||!password.value){
        ElMessage.error('请保证不为空');
      } else if(!validateEmail(loginEmail.value)){
        ElMessage.error('邮箱格式不正确');
      } else if (!user) {
        ElMessage.error('邮箱不存在');
      } else if (user.psw !== password.value) {
        ElMessage.error('密码不正确');
      } else {
        ElMessage.success('登录成功');
      }
    };

const register=()=>{
      const user=dataArr.find(us=>us.email==registerEmail.value);
      if(!registerEmail.value||!newPassword.value||!confirmPassword.value||!verificationCode.value){
        ElMessage.error('请保证不为空');
      } else if(!validateEmail(registerEmail.value)){
        ElMessage.error('邮箱格式不正确');
      } else if(user){
        ElMessage.error('邮箱已绑定');
      }else if(newPassword.value!==confirmPassword.value){
        ElMessage.error("两次输入密码不一致");
      }else if(!verificationCode.value||verificationCode.value!==realVerificationCode.value){
        ElMessage.error("验证码错误");
      }else{
        dataArr.push({email:registerEmail.value,psw:newPassword.value});
        ElMessage.success('注册成功');
        changeToLogin();
      }
}
const changPsw=()=>{
      const user=dataArr.find(us=>us.email==registerEmail.value);
      if(!registerEmail.value||!newPassword.value||!confirmPassword.value||!verificationCode.value){
        ElMessage.error('请保证不为空');
      } else if(!validateEmail(registerEmail.value)){
        ElMessage.error('邮箱格式不正确');
      } else if(!user){
        ElMessage.error('邮箱未注册');
      }else if(newPassword.value!==confirmPassword.value){
        ElMessage.error("两次输入密码不一致");
      }else if(!verificationCode.value||verificationCode.value!==realVerificationCode.value){
        ElMessage.error("验证码错误");
      }else{
        user.psw=confirmPassword.value;
        ElMessage.success('密码修改成功');
        changeToLogin();
      }
}
const getVerificationCode=()=>{
  realVerificationCode.value="1111"
}
const handleForgetPassword=()=>{
  isLookfor.value=true;
  changeToRegist();
}
function changeToRegist() {
  styleObj.value.bordertoprightradius = '0px';
  styleObj.value.borderbottomrightradius = '0px';
  styleObj.value.bordertopleftradius = '15px';
  styleObj.value.borderbottomleftradius = '15px';
  styleObj.value.rightDis = '50%';
  isLogin.value = !isLogin.value;
  clearData();
}
function changeToLogin() {
  isLookfor.value=false;
  styleObj.value.bordertoprightradius = '15px';
  styleObj.value.borderbottomrightradius = '15px';
  styleObj.value.bordertopleftradius = '0px';
  styleObj.value.borderbottomleftradius = '0px';
  styleObj.value.rightDis = '0px';
  isLogin.value = !isLogin.value;
  clearData();
}
</script>

<style scoped>
html, body {
  height: 100%;
  margin: 0;
  overflow: hidden; /* 禁用滚动条 */
}
.el-button:active{

  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2); /* 点击时的阴影效果 */
  transform: scale(0.95); /* 点击时缩小效果 */
}
.el-radio__input.is-checked .el-radio__inner {
  border-color: #257B5E; 
  background-color: #257B5E; 
}
.base {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: rgb(39, 65, 39); /* 设置背景色为绿色 */
}
.title{
    width: 70%;
    height:25%;
    border-bottom: 1px solid #257B5E;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #257B5E;
    font-weight: bold;
    font-size: 24px;
    margin-bottom: 40px;
}
.forget-password{
  margin-left:150px;
  font-size: 13px; 
  cursor: pointer; 
  color:#257B5E
}
.loginAndRegist {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
}

.loginArea, .registerArea {
  background-color: rgba(255, 255, 255, 0.8);
  height: 400px;
  width: 350px;
  z-index: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  overflow: hidden;
}
.loginArea {
  border-top-left-radius: 15px;
  border-bottom-left-radius: 15px;
}
.registerArea {
  border-top-right-radius: 15px;
  border-bottom-right-radius: 15px;
}
.registForm{
  margin-top: 30px;
}
.showInfo:hover{
    background-size: 100%;
    background-position: -50px -50px;
}
.showInfo{
    border-top-right-radius: 15px;
    border-bottom-right-radius: 15px;
    display: flex;
    align-items: center;
    justify-content: center;
    position: absolute;
    height: 400px;
    width: 350px;
    z-index:2;
    top: 0;
    right: 0;
    background-image: url("../assets/mmy/greenLeaf.png");
    background-size: 90%;
    /* 匀速过渡效果 */
    transition: 0.3s linear;
}
.showInfo:hover {
  background-size: 100%;
  background-position: -50px -50px;
}
.button-container {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100%;
}
</style>
