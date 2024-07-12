<!-- 管理员页面的市集管理 -->
<template>
  <div class="container">
    <AdminSidebarMenu />
    <div class="main-content">
      <AdminHeaderSec />
      <div class="content">
        <hr>
        <div class="main-container">
          <div class="form-container">
            <h2>市集设置</h2>
            <el-form>
              <el-row :gutter="100">
                <el-col :span="7">
                  <el-form-item label="市集主题">
                    <el-input v-model="formData.topic"></el-input>
                  </el-form-item>
                </el-col>
                <el-col :span="7"> 
                  <el-form-item label="选择类型">
                    <el-select v-model="formData.selectedOptions" multiple placeholder="将向以下类型商家发送邀请" style="width: 300px;">
                      <el-option
                        v-for="item in options"
                        :key="item.value"
                        :label="item.label"
                        :value="item.value">
                      </el-option>
                    </el-select>
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row :gutter="100">
                <el-col :span="7">
                  <el-form-item label="开始时间">
                    <el-input type="date" v-model="formData.start" placeholder="请选择日期"></el-input>
                  </el-form-item>
                </el-col>
                <el-col :span="7">
                  <el-form-item label="结束时间">
                    <el-input type="date" v-model="formData.end" placeholder="请选择日期"></el-input>
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row :gutter="100">
                <el-col :span="20"> 
                  <el-form-item label="市集信息">
                    <el-input type="textarea" v-model="formData.detail" style="width: 30%; min-height: 10px;" placeholder="请输入市集相关信息"> </el-input>
                  </el-form-item>
                </el-col>
              </el-row>
              <el-form-item label="图片上传">
                <input type="file" @change="handleFileUpload" accept=".png, .jpg">
              </el-form-item>
              <el-form-item>
                <el-button @click="handleSubmit" type="primary" >提交</el-button>
              </el-form-item>
            </el-form>      
          </div>


          <div class="table-container">
            <h2>市集管理</h2>
            <el-table :data="paginatedData">
                <el-table-column prop="id" label="市集id"></el-table-column>
                <el-table-column prop="start" label="开始时间"></el-table-column>
                <el-table-column prop="end" label="结束时间"></el-table-column>
                <el-table-column label="操作">
                <template #default="scope">
                  <el-button @click="search(scope.row.id)" type="primary">查看详情</el-button>
                  <!-- 删除按钮待添加（删除还未开始的市集） -->
                </template>
              </el-table-column>
            </el-table>

            <el-dialog v-model="dialogVisible" title="详情" width="60%" >
              <p>{{ selectedDetail }}</p>
              <!-- <template #footer>
                <el-button @click="dialogVisible = false">关闭</el-button>
              </template> -->
            </el-dialog>

            <div class="pagination-container">
              <div class="pagination">
                <div style="text-align: center;">
                  <span>共 {{ totalItems }} 条</span>
                </div>          
                <el-pagination
                  background
                  layout="prev, pager, next"
                  :total="totalItems"
                  :page-size="pageSize"
                  @current-change="handlePageChange"
                  :current-page="currentPage">
                </el-pagination>
              </div>
            </div>
          </div>
        </div>
        
      </div>
    </div>
  </div>
  
</template>
  
<script setup>
import AdminSidebarMenu from '../components/AdminSidebarMenu.vue'
import AdminHeaderSec from '../components/AdminHeaderSec.vue'
import { reactive, ref, computed } from 'vue';
import { ElTable, ElTableColumn, ElPagination, ElButton, ElDialog, ElForm, ElFormItem, ElInput, ElRow, ElCol, ElSelect, ElOption, ElMessage } from 'element-plus';
import 'element-plus/dist/index.css';



// 测试数组
const dataArr = reactive([
  { id: 1, start: "2024-1-12", end: "2024-1-12", detail:"1这是市集详情这是市集详情这是市集详情这是市集详情"},
  { id: 2, start: "2024-1-12", end: "2024-1-12", detail:"2这是市集详情这是市集详情这是市集详情这是市集详情"},
  { id: 3, start: "2024-1-12", end: "2024-1-12", detail:"3这是市集详情这是市集详情这是市集详情这是市集详情"},
  { id: 4, start: "2024-1-12", end: "2024-1-12", detail:"4这是市集详情这是市集详情这是市集详情这是市集详情"},
  { id: 5, start: "2024-1-12", end: "2024-1-12", detail:"5这是市集详情这是市集详情这是市集详情这是市集详情"},
  { id: 6, start: "2024-1-12", end: "2024-1-12", detail:"6这是市集详情这是市集详情这是市集详情这是市集详情"},
  { id: 7, start: "2024-1-12", end: "2024-1-12", detail:"7这是市集详情这是市集详情这是市集详情这是市集详情"},
  { id: 8, start: "2024-1-12", end: "2024-1-12", detail:"8这是市集详情这是市集详情这是市集详情这是市集详情"},
  { id: 9, start: "2024-1-12", end: "2024-1-12", detail:"9这是市集详情这是市集详情这是市集详情这是市集详情"},
]);
const options = ref([
  { value: 'option1', label: '家居' },
  { value: 'option2', label: '文具' },
  { value: 'option3', label: '装饰' }
]);

// 页面大小及当前页数
const pageSize = ref(4);
const currentPage = ref(1);

// dialog（详情窗口）相关变量
const dialogVisible = ref(false);
const selectedDetail = ref('');

const totalItems = computed(() => dataArr.length);

// 处理并分页后的数据
const paginatedData = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value;
  const end = start + pageSize.value;
  return dataArr.slice(start, end);
});

const handlePageChange = (page) => {
  currentPage.value = page;
};

const search = (id) => {
  const market = dataArr.find(market => market.id === id);
  if (market) {
    selectedDetail.value = market.detail;
    dialogVisible.value = true;
  }
};

const formData = reactive({
  topic: '',
  start: '',
  end: '',
  detail: '',
  selectedOptions:[],
  img:''
});

const handleFileUpload = (event) => {
  const file = event.target.files[0];
  formData.img = file;
};

const submitted = ref(false);

const handleSubmit = () => {
  for (const key in formData) {
    if (formData[key] === '') {
      ElMessage.error("表单不可有空")
      return
    }
  }
  if(formData.end <= formData.start){
    ElMessage.error("结束时间不能早于开始时间")
    return
  }
  else{
    submitted.value = true;
    ElMessage.success("提交成功")
  }
  formData.topic='';
  formData.start='';
  formData.end='';
  formData.detail='';
  formData.img='';
  formData.selectedOptions=[];
};

</script>

<style scoped>
h2 {
  display: block; /* 元素作为块级元素显示，占据其父容器的整个宽度 */
  font-size: 1.5em; /* 字体大小设置为1.5em，em是相对于父元素字体大小的单位 */
  margin-block-start: 0.83em; /* 元素上方的外边距（margin），以em为单位 */
  margin-block-end: 0.83em; /* 元素下方的外边距，以em为单位 */
  margin-inline-start: 0px; /* 元素左侧的外边距，设置为0 */
  margin-inline-end: 0px; /* 元素右侧的外边距，设置为0 */
  font-weight: bold; /* 字体加粗 */
  unicode-bidi: isolate; /* 设置双向文本的隔离行为，防止混合使用不同方向的文本对布局的影响 */
}
.container {
  display: flex;
  height: 100vh;
}

.main-content {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.content {
  flex: 1;
  padding: 0 15px;
}
.main-container {
  background-color: #ffffff;
  padding: 0 10px;
  text-align: left; /* 确保文本对齐样式 */
  color: #000000; /* 确保文本颜色 */
}

.pagination-container {
  display: flex;
  justify-content: center;
}


</style>
  