<!-- 管理员页面的商家认证 -->
<template>
  <div class="container1">
    <AdminSidebarMenu />
    <div class="main-content">
      <AdminHeaderSec />
      <div class="content">
        <hr>
        <div class="container">
          <h2>审核商家认证申请</h2>
          <div>
            <el-table :data="paginatedData">
              <el-table-column prop="storeId" label="商家账号"></el-table-column>
              <el-table-column prop="status" label="申请状态"></el-table-column>
              <!-- 待添加认证图片展示 -->
              <el-table-column label="操作">
                <template #default="scope">
                  <el-button @click="pass(scope.row.storeId)" :disabled="scope.row.status !== '未处理'" type="primary">通过申请</el-button>
                  <el-button @click="reject(scope.row.storeId)" :disabled="scope.row.status !== '未处理'" type="danger">拒绝申请</el-button>
                </template>
              </el-table-column>
            </el-table>

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

            <div>
              <!-- <h4>图片测试</h4>
              <viewer :images="imageArr">
                  <img v-for="src in imageArr" :src="src.url" :key="src.title">
              </viewer> -->
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
import { ElTable, ElTableColumn, ElPagination, ElButton, ElMessage } from 'element-plus';
import 'element-plus/dist/index.css';
import axiosInstance from '../components/axios';

const records = reactive([]);
const message = ref('');

const fetchRecords = async () => {
  try {
    const response = await axiosInstance.get('/Administrator/Administrator/GetAllAuthentication');
    records.values = response.data;
    message.value = '已获取申请数据';
  } catch (error) {
    if (error.response) {
      message.value = error.response.data;
    } else {
      message.value = '获取数据失败';
    }
  }
  console.log(message.value);
};
fetchRecords();


//通过申请按钮的响应函数
const pass = (id) => {
  // const store = dataArr.find(store => store.storeId === id);
  const store = records.find(store => store.storeId === id);
  if (store) {
    ElMessage.success("处理完成")
    store.status = "通过";
  }
}

const reject = (id) => {
  // const store = dataArr.find(store => store.storeId === id);
  const store = records.find(store => store.storeId === id);
  if (store) {
    ElMessage.success("处理完成")
    store.status = "已拒绝";
  }
}

// 设置表格页面大小及当前页数
const pageSize = ref(9);
const currentPage = ref(1);

// const totalItems = computed(() => dataArr.length);
const totalItems = computed(() => records.length);

// 处理并分页后的数据
const paginatedData = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value;
  const end = start + pageSize.value;
  // return dataArr.slice(start, end);
  return records.slice(start, end);
});

// 切换页面
const handlePageChange = (page) => {
  currentPage.value = page;
};

</script>

<style scoped>
h2 {
  display: block;
  font-size: 1.5em;
  margin-block-start: 0.83em;
  margin-block-end: 0.83em;
  margin-inline-start: 0px;
  margin-inline-end: 0px;
  font-weight: bold;
  unicode-bidi: isolate;
}
.container1 {
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
  text-align: left;
  color: #000000;
}
.container {
  background-color: #ffffff;
  padding: 0 10px;
}

.pagination-container {
  display: flex;
  justify-content: center;
}

.pagination {
  max-width: 100%;
}

</style>
