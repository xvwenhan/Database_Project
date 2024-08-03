<template>
  <div class="user-center">
    <el-tabs v-model="activeTab">
      <el-tab-pane label="个人信息" name="userInfo">
        <div class="user-info">
          <el-form :model="userInfo" label-width="80px">
            <el-form-item label="用户名" :rules="[{ required: true, message: '请输入用户名', trigger: 'blur' }]">
              <el-input v-model="userInfo.username" placeholder="请输入用户名"></el-input>
            </el-form-item>
            <el-form-item label="邮箱" :rules="[{ required: true, message: '请输入邮箱', trigger: 'blur' }, { type: 'email', message: '请输入有效的邮箱地址', trigger: 'blur' }]">
              <el-input v-model="userInfo.email" placeholder="请输入邮箱"></el-input>
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
            <el-form-item label="当前密码" :rules="[{ required: true, message: '请输入当前密码', trigger: 'blur' }]">
              <el-input 
                v-model="password.current"
                :type="passwordVisibility.current ? 'text' : 'password'"
                placeholder="请输入当前密码"
              >
                <template #suffix>
                  <img 
                     :src="currentImage"
                    @click="toggleVisibility('current')"
                    class="password-visibility-toggle"
                    alt="toggle visibility"
                  />
                </template>
              </el-input>
            </el-form-item>
            <el-form-item label="新密码" :rules="[{ required: true, message: '请输入新密码', trigger: 'blur' }]">
              <el-input 
                v-model="password.new"
                :type="passwordVisibility.new ? 'text' : 'password'"
                placeholder="请输入新密码"
              >
                <template #suffix>
                  <img 
                   :src="currentImageTw"
                    @click="toggleVisibility('new')"
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
                    @click="toggleVisibility('confirm')"
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

export default {
  data() {
    return {
      activeTab: 'userInfo',
      userInfo: {
        username: '',
        email: ''
      },
      points: 1000,
      address: {
        detail: ''
      },
      balance: 5000,
      password: {
        current: '',
        new: '',
        confirm: ''
      },
      recharge: {
        amount: 0
      },
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
    updateUserInfo() {
      if (!this.userInfo.username || !this.userInfo.email || !this.address.detail) {
        this.$message.error('所有字段都必须填写');
        return;
      }
      if (!this.validateEmail(this.userInfo.email)) {
        this.$message.error('请输入有效的邮箱地址');
        return;
      }
      // 处理更新用户信息逻辑
      this.$message.success('用户信息更新成功');
    },
    logout() {
      localStorage.removeItem('userToken'); // 示例，实际根据你的应用逻辑调整
      this.$router.push('/login');
    },
    rechargeBalance() {
      if (this.recharge.amount <= 0) {
        this.$message.error('充值金额必须大于零');
        return;
      }
      this.balance += this.recharge.amount;
      this.recharge.amount = 0; // 重置充值金额
      this.$message.success('充值成功');
    },
    updateAccountInfo() {
      if (this.password.new !== this.password.confirm) {
        this.$message.error('新密码和确认密码不匹配');
        return;
      }
      if (this.password.current !== 'correct-password') { // 假设 'correct-password' 是正确的当前密码
        this.$message.error('当前密码错误');
        return;
      }
      this.$message.success('用户信息更新成功');
      this.password.current = '';
      this.password.new = '';
      this.password.confirm = '';
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
    this.userInfo = {
      username: '张三',
      email: 'zhangsan@example.com'
    };
    this.address = {
      detail: '北京市海淀区某某街道'
    };
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