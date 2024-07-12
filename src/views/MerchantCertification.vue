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
              <el-table-column prop="Account_ID" label="商家账号"></el-table-column>
              <el-table-column prop="state" label="申请状态"></el-table-column>
              <!-- 待添加认证图片展示 -->
              <el-table-column label="操作">
                <template #default="scope">
                  <el-button @click="pass(scope.row.Account_ID)" :disabled="scope.row.state === 'pass'" type="primary">通过申请</el-button>
                  <!-- 待添加拒绝按钮 -->
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

//测试数组
const dataArr = reactive([
  { Account_ID: 1, state: "未处理" },
  { Account_ID: 2, state: "未处理" },
  { Account_ID: 3, state: "通过" },
  { Account_ID: 4, state: "未处理" },
  { Account_ID: 5, state: "未处理" },
  { Account_ID: 6, state: "通过" },
  { Account_ID: 7, state: "未处理" },
  { Account_ID: 8, state: "未处理" },
  { Account_ID: 9, state: "通过" },
  { Account_ID: 10, state: "未处理" },
  { Account_ID: 11, state: "未处理" },
  { Account_ID: 12, state: "通过" },
]);

// const imageArr = reactive([
//   {url:"https://wx2.sinaimg.cn/mw2000/d123e1efly1hq8y19n640j20u0140797.jpg",title:"图片1"}
// ])


//通过申请按钮的响应函数
const pass = (id) => {
  const store = dataArr.find(store => store.Account_ID === id);
  if (store) {
    ElMessage.success("处理完成")
    store.state = "通过";
  }
}

// 设置表格页面大小及当前页数
const pageSize = ref(9);
const currentPage = ref(1);

const totalItems = computed(() => dataArr.length);

// 处理并分页后的数据
const paginatedData = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value;
  const end = start + pageSize.value;
  return dataArr.slice(start, end);
});

// 切换页面
const handlePageChange = (page) => {
  currentPage.value = page;
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
  text-align: left; /* 确保文本对齐样式 */
  color: #000000; /* 确保文本颜色 */
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
