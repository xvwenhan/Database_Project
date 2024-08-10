<template>
  <div class="business-setting">
    <el-tabs v-model="activeTab">
      <el-tab-pane label="认证资料" name="certification">
        <div>
          <div v-if="certificationStatus === '请求上传'">
            <el-button @click="handleCertificationUpload">上传认证资料(图片和文字)</el-button>
          </div>
          <div v-else-if="certificationStatus === '正在审核'">
            <p>您的认证资料正在审核中，请稍等。</p>
          </div>
          <div v-else-if="certificationStatus === '审核通过'">
            <p>您的认证资料已通过审核（展示）。</p>
          </div>
        </div>
      </el-tab-pane>

      <el-tab-pane label="商家信息" name="businessInfo">
        <div class="business-info">
          <el-form :model="businessInfo" label-width="80px">
            <el-form-item label="店铺名" :rules="[{ required: true, message: '请输入店铺名', trigger: 'blur' }]">
              <el-input v-model="businessInfo.username" placeholder="请输入店铺名"></el-input>
            </el-form-item>
            <el-form-item label="商家地址" :rules="[{ required: true, message: '请输入商家地址', trigger: 'blur' }]">
              <el-input v-model="businessInfo.address" placeholder="请输入商家地址"></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="updateBusinessInfo">保存</el-button>
            </el-form-item>
          </el-form>
        </div>
      </el-tab-pane>

      <el-tab-pane label="修改密码" name="account">
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
                    @click="toggleVisibility()"
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

      <!-- 新增退出登录选项卡 -->
      <el-tab-pane label="退出登录" name="accountManagement">
        <div class="logout-section">
          <el-button type="danger" @click="logout">退出登录</el-button>
        </div>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import axiosInstance from '../components/axios';
import eyeSvg from "@/assets/eye.svg"
import eyeInSvg from "@/assets/eyeIn.svg"

export default {
  name:'BusinessSetting',
  data() {
    return {
      activeTab: 'certification',
      certificationStatus: '请求上传', // '请求上传', '正在审核', '审核通过'
      businessInfo: {
        username: '',
        address: ''
      },
      password: {
        current: '',
        new: '',
        confirm: ''
      },
      passwordVisibility: {
        current: false,
        new: false,
        confirm: false
      },
      loading: false,
      error: null
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
  created() {
    this.updateBusinessInfo();  // 在组件创建时调用方法获取店铺评分
  },
  methods: {
    toggleVisibility() {
      this.passwordVisibility.current = !this.passwordVisibility.current;
    },
    toggleVisibilityTw() {
      this.passwordVisibility.new = !this.passwordVisibility.new;
    },
    toggleVisibilityTh() {
      this.passwordVisibility.confirm = !this.passwordVisibility.confirm;
    },
    handleCertificationUpload() {
      // 处理上传认证资料
    },
    logout() {
      // 清理用户认证信息，例如清除本地存储
      localStorage.removeItem('userToken'); // 示例，实际根据你的应用逻辑调整

      // 重定向到登录页
      this.$router.push('/login');
    },
    async updateBusinessInfo() {
      const storeId = 'S1234567'; // 替换为实际的 storeid
      this.loading = true;
      this.error = null;

      if (!this.businessInfo.username || !this.businessInfo.address) {
        // this.$message.error('所有字段都必须填写');
        return;
      }
      const payload = {
        StoreName: String(this.businessInfo.username),
        Address: String(this.businessInfo.address),
      };

      console.log('Request payload:', payload); 

      try {
        const response = await axiosInstance.put('/StoreFront/UpdateStoreInfo', payload, {
          params: {
            storeId: storeId,
          },
        });
        console.log('Update response:', response.data); // 打印响应数据
        this.$message.success('商家信息更新成功');
      } catch (error) {
        this.$message.error('更新商家信息失败');
        console.error('Error updating business info:', error);
      }finally {
        this.loading = false;
      }
    },
    updateAccountInfo() {
      if (this.password.new !== this.password.confirm) {
        this.$message.error('新密码和确认密码不匹配');
        return;
      }
      // 模拟验证当前密码
      if (this.password.current !== 'correct-password') { // 假设 'correct-password' 是正确的当前密码
        this.$message.error('当前密码错误');
        return;
      }
      // 处理更新用户信息逻辑
      this.$message.success('用户信息更新成功');
      // 重置密码字段
      this.password.current = '';
      this.password.new = '';
      this.password.confirm = '';
    },
    // validatePasswordConfirm(rule, value, callback) {
    //   if (value !== this.password.new) {
    //     callback(new Error('确认密码与新密码不一致'));
    //   } else {
    //     callback();
    //   }
    // }
  },
  mounted() {
    // 模拟获取用户信息
    this.businessInfo = {
      username: '张三',
      address: '北京市海淀区某某街道'
    };
    // 模拟获取其他信息
    this.password.current = '';
    this.password.new = '';
    this.password.confirm = '';
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