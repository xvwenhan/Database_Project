<template>
  <div class="business-setting">
    <el-tabs v-model="activeTab">
      <el-tab-pane label="认证资料" name="certification">
        <div>
          <div v-if="certificationStatus === '请求上传'">
            <el-form :model="certificationUp" :rules="rules" ref="form">
              <el-form-item label="认证资料图片上传(.jpg)" prop="image">
                <img :src="certificationUp.image" alt="当前图片" v-if="certificationUp.image" style="width: 200px; height: 200px;" />
                <input type="file" @change="handleFileChange" accept=".jpg" />
              </el-form-item>
              <el-form-item label="认证资料文字描述" prop="description">
                <el-input v-model="certificationUp.description"></el-input>
              </el-form-item>
            </el-form>
            <el-button type="primary" @click="handleCertificationUpload">上传认证资料(图片和文字)</el-button>
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
      certificationUp:{
        image:'',
        description:''
      },
      uploadStatus:false,
      passStatus:false,
      businessInfo: {
        username: '',
        address: ''
      },
      password: {
        // current: '',
        new: '',
        confirm: ''
      },
      passwordVisibility: {
        // current: false,
        new: false,
        confirm: false
      },
      loading: false,
      error: null,
      rules: {
        image: [
          { required: true, message: '请提供认证图片', trigger: 'change' }
        ],
        description: [
          { required: true, message: '请提供认证文字描述', trigger: 'blur' }
        ]
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
    handleFileChange(event) {
  const file = event.target.files[0];
  if (file) {
    console.log('Selected file:', file); // 输出文件信息
    const validTypes = ['image/jpeg', 'image/jpg', 'image/png'];
    if (validTypes.includes(file.type)) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.certificationUp.image = e.target.result;
      };
      reader.readAsDataURL(file);
    } else {
      this.$message.error('请上传正确格式的图片 (.jpg 或 .png)');
    }
  }
},

async handleCertificationUpload() {
  try {
    if (!this.certificationUp.image) {
      this.$message.error('请提供图片');
      return;
    }

    // 从图片数据中去除 Base64 前缀
    const photoBase64 = this.certificationUp.image.split(',')[1]; 
    const authentication = this.certificationUp.description;
    const storeId = 'S1234567'; 

    if (!photoBase64 || !authentication) {
      this.$message.error('请提供图片和认证资料');
      return;
    }

    const response = await axiosInstance.post('/StoreFront/SubmitAuthentication', {
      photoBase64,
      authentication,
    }, {
      params: {
        storeId,
      },
    });

    if (response.status === 200) {
      this.$message.success('认证资料上传成功');
      this.certificationStatus = '正在审核'; 
    } else {
      this.$message.error(`上传失败: ${response.data.message}`);
    }
  } catch (error) {
    console.error('请求失败:', error.response ? error.response.data : error.message);
    this.$message.error('请求失败，请稍后再试');
  }
},
    async getUserInfo(userId) {
      try {
        const response = await axiosInstance.get(`/Account/get_user_message/${userId}`);
        
        if (response.data.message === '用户查找成功！') { // 根据实际响应内容调整
          this.businessInfo = {
            // username: response.data.target_user.useR_NAME,
            // gender: response.data.target_user.gender,
            // age: response.data.target_user.age,
            address:response.data.target_user.address
          };

          // this.currentPass=response.data.target_user.password;
        } else {
          this.$message.error('获取用户信息失败');
        }
      } catch (error) {
        console.error('请求失败:', error);
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
  if (this.password.new !== this.password.confirm) {
    this.$message.error('新密码和确认密码不匹配');
    return;
  }

  try {
    // 发送请求到后端重置密码
    const response = await axiosInstance.post('/Account/password_reset', {
      username: "S1234567",  // 使用当前用户的用户名
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
async fetchStoreName() {
      const storeId = 'S1234567'; // 替换为实际的 storeid
      this.loading = true;
      this.error = null;
      this.storeScoreName = null;

      try {
        const response = await axiosInstance.get('/StoreFront/UpdateStoreScore', {
          params: { storeId }
        });
        this.businessInfo.username = response.data.storeName;
      } catch (error) {
        this.error = 'Failed to fetch store score';
        console.error('Error fetching store score:', error);
      } finally {
        this.loading = false;
      }
    },
    async checkCertificationStatus() {
  const storeId = 'S1234567'; // 替换为实际的 storeId
  try {
    const response = await axiosInstance.get('/StoreFront/UpdateStoreAuth', {
      params: {
        storeId
      }
    });

    this.passStatus = response.data.certification; // 获取返回的认证状态
    console.log(response.data.certification);
    if(this.uploadStatus==false){
      this.certificationStatus = '请求上传';
    }
    else if (this.uploadStatus==true&&this.passStatus) {
      this.certificationStatus = '审核通过';
    } else if(this.uploadStatus==true&&this.passStatus==false){
      this.certificationStatus = '正在审核';
    }  
    else{
      this.$message.error('认证资料状态判定失败');
    }                          
  } catch (error) {
    console.error('获取认证状态失败:', error);
    this.$message.error('获取认证状态失败，请稍后再试');
  }
},
async checkUploadStatus() {
  const storeId = 'S1234567'; // 替换为实际的 storeId
  try {
    const response = await axiosInstance.get('/StoreFront/IfSubmitStoreAuth', {
      params: {
        storeId
      }
    });
    this.uploadStatus = response.data.certification; // 获取返回的认证状态
    console.log(response.data.certification);                   
  } catch (error) {
    console.error('获取上传状态失败:', error);
    this.$message.error('获取上传状态失败，请稍后再试');
  }
}
  },
  mounted() {
    const userId = 'S1234567'; // 替换为实际的用户 ID
    this.getUserInfo(userId);
    this.fetchStoreName();
    this.checkUploadStatus();
    this.checkCertificationStatus(); 
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