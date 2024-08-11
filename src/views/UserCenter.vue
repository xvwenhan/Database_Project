<template>
  <div class="user-center">
    <el-tabs v-model="activeTab">
      <el-tab-pane label="个人信息" name="userInfo">
        <div class="user-info">
          <el-form :model="userInfo" label-width="80px">
            <el-form-item label="用户名" :rules="[{ required: true, message: '请输入用户名', trigger: 'blur' }]">
              <el-input v-model="userInfo.username" placeholder="请输入用户名"></el-input>
            </el-form-item>
            <!-- 新增性别选项 -->
            <el-form-item label="性别" :rules="[{ required: true, message: '请选择性别', trigger: 'change' }]">
              <el-select v-model="userInfo.gender" placeholder="请选择性别">
                <el-option label="男" value="male"></el-option>
                <el-option label="女" value="female"></el-option>
              </el-select>
            </el-form-item>
            <!-- 新增年龄选项 -->
            <el-form-item label="年龄" :rules="[{ required: true, message: '请输入年龄', trigger: 'blur' }]">
              <el-input v-model.number="userInfo.age" type="number" placeholder="请输入年龄"></el-input>
            </el-form-item>
            <el-form-item label="收货地址" :rules="[{ required: true, message: '请输入收货地址', trigger: 'blur' }]">
              <el-input v-model="address.detail" placeholder="请输入收货地址"></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="updateUserInfo">保存</el-button>
            </el-form-item>
          </el-form>
        </div>
      </el-tab-pane>
      <el-tab-pane label="消费积分" name="points">
        <div class="points">
          <el-card>
            <p>消费积分</p>
            <p>您当前的积分：{{ points }}</p>
          </el-card>
        </div>
      </el-tab-pane>
      <el-tab-pane label="钱包" name="wallet">
        <div class="wallet">
          <el-card>
            <h3>钱包</h3>
            <p>您的钱包余额：￥{{ balance }}</p>
            <el-form :model="recharge" label-width="80px">
              <el-form-item label="充值金额">
                <el-input v-model.number="recharge.amount" type="number" placeholder="请输入充值金额"></el-input>
              </el-form-item>
              <el-form-item>
                <el-button type="primary" @click="rechargeBalance">充值</el-button>
              </el-form-item>
            </el-form>
          </el-card>
        </div>
      </el-tab-pane>

      <!-- 修改密码选项卡 -->
      <el-tab-pane label="修改密码" name="changePassword">
        <div class="account-info">
          <el-form :model="password" label-width="80px">
            <!-- <el-form-item label="当前密码" :rules="[{ required: true, message: '请输入当前密码', trigger: 'blur' }]">
              <el-input 
                v-model="password.current"
                :type="passwordVisibility.current ? 'text' : 'password'"
                placeholder="请输入当前密码"
              >
                <template #suffix>
                  <img 
                     :src="currentImage"
                    @click="toggleVisibility()"
                    class="password-visibility-toggle"
                    alt="toggle visibility"
                  />
                </template>
              </el-input>
            </el-form-item> -->
            <el-form-item label="新密码" :rules="[{ required: true, message: '请输入新密码', trigger: 'blur' }]">
              <el-input 
                v-model="password.new"
                :type="passwordVisibility.new ? 'text' : 'password'"
                placeholder="请输入新密码"
              >
                <template #suffix>
                  <img 
                   :src="currentImageTw"
                    @click="toggleVisibilityTw()"
                    class="password-visibility-toggle"
                    alt="toggle visibility"
                  />
                </template>
              </el-input>
            </el-form-item>
            <el-form-item label="确认密码" :rules="[{ required: true, message: '请确认新密码', trigger: 'blur' }]">
              <el-input 
                v-model="password.confirm"
                :type="passwordVisibility.confirm ? 'text' : 'password'"
                placeholder="请确认新密码"
              >
                <template #suffix>
                  <img 
                     :src="currentImageTh"
                    @click="toggleVisibilityTh()"
                    class="password-visibility-toggle"
                    alt="toggle visibility"
                  />
                </template>
              </el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="updateAccountInfo">保存</el-button>
            </el-form-item>
          </el-form>
        </div>
      </el-tab-pane>

      <!-- 退出登录选项卡 -->
      <el-tab-pane label="退出登录" name="accountManagement">
        <div class="logout-section">
          <el-button type="danger" @click="logout">退出登录</el-button>
        </div>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import eyeSvg from "@/assets/eye.svg"
import eyeInSvg from "@/assets/eyeIn.svg"
import axiosInstance from '../components/axios';

export default {
  data() {
    return {
      activeTab: 'userInfo',
      userInfo: {
        username: '',
        gender: '', // 新增性别字段
        age: null // 新增年龄字段
      },
      points: 0,
      address: {
        detail: ''
      },
      balance: 0,
      password: {
        // current: '',
        new: '',
        confirm: ''
      },
      recharge: {
        amount: 0
      },
      currentPass:'',
      passwordVisibility: {
        current: false,
        new: false,
        confirm: false
      }
    };
  },
  computed: {
    currentImage() {
      return this.passwordVisibility.current ? eyeSvg : eyeInSvg;
    },
    currentImageTw() {
      return this.passwordVisibility.new ? eyeSvg : eyeInSvg;
    },
    currentImageTh() {
      return this.passwordVisibility.confirm ? eyeSvg : eyeInSvg;
    },
  },
  methods: {
    async getUserInfo(userId) {
      try {
        const response = await axiosInstance.get(`/Account/get_user_message/${userId}`);
        
        if (response.data.message === '用户查找成功！') { // 根据实际响应内容调整
          this.userInfo = {
            username: response.data.target_user.useR_NAME,
            gender: response.data.target_user.gender,
            age: response.data.target_user.age,
          };
          this.address = {
        detail: response.data.target_user.address
      };
          // this.currentPass=response.data.target_user.password;
        } else {
          this.$message.error('获取用户信息失败');
        }
      } catch (error) {
        console.error('请求失败:获取用户信息', error);
        this.$message.error('请求失败，请稍后再试');
      }
    },
    async updateUserInfo() {
      const userId = localStorage.getItem('userId');
  if (!this.userInfo.username || !this.userInfo.gender || this.userInfo.age === null || !this.address.detail) {
    this.$message.error('所有字段都必须填写');
    return;
  }

  // 验证年龄必须大于或等于0，并且是整数
  if (this.userInfo.age < 0 || !Number.isInteger(this.userInfo.age)) {
    this.$message.error('年龄必须是大于或等于0的整数');
    return;
  }

  try {
    const response = await axiosInstance.post('/Account/modify_buyer_message', {
      accountId: userId, // 替换为实际的 accountId
      userName: this.userInfo.username,
      gender: this.userInfo.gender,
      age: this.userInfo.age,
      address: this.address.detail
    });

    if (response.data.message === "用户信息修改成功！") { 
      this.$message.success('用户信息更新成功');
    } else {
      this.$message.error(`用户信息更新失败: ${response.data.detail}`);
    }
  } catch (error) {
    console.error('请求失败:', error); // 输出错误信息
    this.$message.error('请求失败，请稍后再试');
  }
},
async getWalletBalance(userId) {
  try {
    const response = await axiosInstance.post('/Payment/GetWalletBalance', null, {
      params: {
        accountID: userId
      }
    });

    console.log('API response:', response.data); // 输出完整的响应内容

    // 直接将返回的 response.data 视为余额
    if (typeof response.data === 'number') {
      this.balance = response.data;
    } else {
      this.$message.warning('未返回有效的余额信息');
      this.balance = 0; // 设置默认值为 0 或其他默认值
    }
  } catch (error) {
    console.error('请求失败:钱包数值获取', error);
    this.$message.error('请求失败，请稍后再试');
  }
},
async getBuyerCredits(userId) {
    try {
      // 调用接口获取买家消费积分
      const response = await axiosInstance.get('/Shopping/GetBuyerCredits', {
        params: {
          buyerId: userId // 替换为实际的买家账号ID
        }
      });

      // 输出响应内容以进行调试
      console.log('API response:', response.data);

      // 直接将返回的积分值赋给 points
      if (Number.isInteger(response.data)) {
        this.points = response.data;
      } else {
        this.$message.warning('未返回有效的积分信息');
      }
    } catch (error) {
      console.error('请求失败:获取消费积分', error);
      this.$message.error('请求失败，请稍后再试');
    }
  },
  async logout() {
      try {
        // 发送请求到后端接口进行退出登录
        const response = await axiosInstance.post('/Account/logout');

        // 在控制台输出响应内容以进行调试
        console.log('API response:', response.data);

        // 检查响应内容并反馈给用户
        if (response.data.message === '登出账号成功！') {
          this.$message.success('退出登录成功');
          localStorage.removeItem('userToken'); // 清除本地存储的用户信息
          this.$router.push('/loginandregister');
        } else {
          this.$message.error(`退出登录失败: ${response.data.message}`);
        }
      } catch (error) {
        console.error('请求失败:', error);
        this.$message.error('请求失败，请稍后再试');
      }
    },
    async rechargeBalance() {
  if (this.recharge.amount <= 0) {
    this.$message.error('充值金额必须大于零');
    return;
  }

  const userId = localStorage.getItem('userId'); // 获取当前用户 ID
  // 创建 FormData 对象
  const formData = new FormData();
  formData.append('BuyerId', userId); // 假设这是当前用户的 ID
  formData.append('Amount', this.recharge.amount);

  try {
    const response = await axiosInstance.post('/Payment/RechargeWallet', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });

    if (response.status === 200 || response.data.success || response.data.code === 0) {
  this.$message.success('充值成功');
  this.balance += this.recharge.amount; // 充值成功后更新余额
} else {
  this.$message.error(`充值失败: ${response.data.message || '未知错误'}`);
}
  } catch (error) {
    console.error('请求失败:', error);
    this.$message.error('请求失败，请稍后再试');                               
  }

  this.recharge.amount = 0; // 重置充值金额
},

async updateAccountInfo() {
  if (this.password.current === '' || this.password.new === '' || this.password.confirm === '') {
    this.$message.error('请填写所有必填项');
    return;
  }
  // console.log(this.password.current);
  // console.log(this.currentPass);

  await this.resetPassword();
},
async resetPassword() {
  // if(this.password.current!=this.currentPass){
  //   this.$message.error('原密码不正确');
  //   return;
  // }
  const userId = localStorage.getItem('userId'); // 获取当前用户 ID
  if (this.password.new !== this.password.confirm) {
    this.$message.error('新密码和确认密码不匹配');
    return;
  }

  try {
    // 发送请求到后端重置密码
    const response = await axiosInstance.post('/Account/password_reset', {
      username:  userId,  // 使用当前用户的用户名
      password: this.password.new  // 使用新密码
    });

    // 在控制台输出响应内容以进行调试
    console.log('API response:', response);

    // 检查响应内容并反馈给用户
    if (response.data.message === "密码重置成功！") {
      this.$message.success('密码重置成功');
      this.password.current = '';
      this.password.new = '';
      this.password.confirm = '';
    } else {
      this.$message.error(`密码重置失败: ${response.data.message}`);
    }
  } catch (error) {
    console.error('请求失败:', error);
    this.$message.error('请求失败，请稍后再试');
  }
},
    toggleVisibility() {
      this.passwordVisibility.current = !this.passwordVisibility.current;
    },
    toggleVisibilityTw() {
      this.passwordVisibility.new = !this.passwordVisibility.new;
    },
    toggleVisibilityTh() {
      this.passwordVisibility.confirm = !this.passwordVisibility.confirm;
    },
    validateEmail(email) {
      const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@(([^<>()[\]\.,;:\s@"]+\.)+[^<>()[\]\.,;:\s@"]{2,})$/i;
      return re.test(email);
    }
  },
  mounted() {
    const userId = localStorage.getItem('userId');  // 替换为实际的用户 ID
    if (userId) {
    this.getUserInfo(userId);
    this.getWalletBalance(userId);
    this.getBuyerCredits(userId); // 调用获取买家积分的方法
  } else {
    this.$message.error('用户ID不存在，请登录后再试');
  }
  }
};
</script>

<style scoped>
.password-visibility-toggle {
  cursor: pointer;
  width: 20px;
  height: 20px;
  margin-left: 10px;
}
</style>