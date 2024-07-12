<!-- 管理员页面的举报管理 -->
<template>
  <div class="container">
    <AdminSidebarMenu />
    <div class="main-content">
      <AdminHeaderSec />
      <div class="content">

        <hr>
        <div class="report-management">
          <h2>举报管理</h2>
          <el-table :data="paginatedData">
              <el-table-column prop="id" label="举报账号id"></el-table-column>
              <el-table-column prop="time" label="举报时间"></el-table-column>
              <el-table-column prop="state" label="审核状态"></el-table-column>
              <el-table-column label="操作">
              <template #default="scope">
                <el-button @click="search(scope.row.id)" type="primary">查看详情</el-button>
                <el-button @click="delete_post(scope.row.id)" :disabled="scope.row.state === '已处理'" type="danger">删除帖子</el-button>
                <el-button @click="ignore_report(scope.row.id)" :disabled="scope.row.state === '已处理'" type="warning">忽略</el-button>
              </template>
            </el-table-column>
          </el-table>

          <el-dialog v-model="dialogVisible" title="举报详情" width="60%" >
            <p>未排版数据</p>
            <p>{{ selectedDetail }}</p>
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
  
</template>

<script setup>
import AdminSidebarMenu from '../components/AdminSidebarMenu.vue'
import AdminHeaderSec from '../components/AdminHeaderSec.vue'
import { reactive, ref, computed} from 'vue';
import { ElTable, ElTableColumn, ElPagination, ElButton, ElDialog, ElMessage } from 'element-plus';
import 'element-plus/dist/index.css';

// 页面大小及当前页数
const pageSize = ref(8);
const currentPage = ref(1);

const totalItems = computed(() => report.length);

// 处理并分页后的数据
const paginatedData = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value;
  const end = start + pageSize.value;
  return report.slice(start, end);
});

const handlePageChange = (page) => {
  currentPage.value = page;
};

// dialog（详情窗口）相关变量
const dialogVisible = ref(false);
const selectedDetail = reactive({
  id: '',
  time: '',
  reason: '',
  post: '',
  state:''
});

//测试数据
const report =reactive([
  {id:12345,time:"20:12 2024-7-11",reason:"举报理由举报理由巴拉巴拉",post:"帖子内容帖子内容",state:"未审核"},
  {id:23456,time:"20:15 2024-7-11",reason:"举报理由举报理由巴拉巴拉",post:"帖子内容帖子内容",state:"未审核"},
  {id:34567,time:"20:17 2024-7-11",reason:"举报理由举报理由巴拉巴拉",post:"帖子内容帖子内容",state:"未审核"},
  {id:45678,time:"20:10 2024-7-11",reason:"举报理由举报理由巴拉巴拉",post:"帖子内容帖子内容",state:"未审核"},
  {id:56789,time:"20:33 2024-7-11",reason:"举报理由举报理由巴拉巴拉",post:"帖子内容帖子内容",state:"未审核"},
  {id:67890,time:"20:42 2024-7-11",reason:"举报理由举报理由巴拉巴拉",post:"帖子内容帖子内容",state:"未审核"}
])

const search = (id) => {
  const post = report.find(post => post.id === id);
  if (post) {
    Object.assign(selectedDetail, post);
    dialogVisible.value = true;
  }
};

const delete_post = (id) => {
  const post = report.find(post => post.id === id);
  if (post) {
    //删除帖子的实现
    ElMessage.error("帖子已删除")
    post.state="已处理"
  }
};

const ignore_report= (id) => {
  const post = report.find(post => post.id === id);
  if (post) {
    ElMessage.success("处理完成")
    post.state="已处理"
  }
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

.report-management {
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
