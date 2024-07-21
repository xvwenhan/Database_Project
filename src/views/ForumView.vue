<!-- 买家用户的论坛页面 -->
<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, computed, reactive } from 'vue';
import { useRouter } from 'vue-router';

// 假设这是你的帖子数据
const posts = ref([
    '帖子1', '帖子2', '帖子3', '帖子4', '帖子5',
    '帖子6', '帖子7', '帖子8', '帖子9', '帖子10',
    '帖子11', '帖子12', '帖子13', '帖子14', '帖子15',
    '帖子16', '帖子17', '帖子18', '帖子19', '帖子20',
    '帖子21', '帖子22', '帖子23'
]);

// 当前页码
const currentPage = ref(1);
// 每页显示的帖子数量
const postsPerPage = 20;

// 计算当前页的帖子列表
const paginatedPosts = computed(() => {
  const startIndex = (currentPage.value - 1) * postsPerPage;
  const endIndex = startIndex + postsPerPage;
  return posts.value.slice(startIndex, endIndex);
});

// 计算总页数
const totalPages = computed(() => Math.ceil(posts.value.length / postsPerPage));

// 翻页函数
function goToPage(page) {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
}
const buttons = reactive([
  { id: 1, text: '发布', background: 'src/assets/czw/release.svg', backgroundColor: 'transparent' },
  { id: 2, text: '按热度排序', background: 'src/assets/czw/hot.svg', backgroundColor: 'transparent' },
  { id: 3, text: '按时间排序', background: 'src/assets/czw/time.svg', backgroundColor: 'transparent' },
]);

const changeButtonColor = (button, isHovered) => {
  if (isHovered) {
    button.backgroundColor = '#87c2a5'; // 设置鼠标悬停时的背景颜色
  } else {
    button.backgroundColor = 'transparent'; // 恢复背景颜色为透明
  }
};

const router = useRouter(); // 使用 vue-router

const buttonClick = (button) => {
  if (button.id === 1) {
    router.push('/releasepost'); // 跳转至 /releasepost 页面
  }
  // 可以添加其他按钮的处理逻辑
};
</script>

<template>
    <Navbar />
  <div>
    <h1 class="forum_title">论坛</h1>
    <div class="main_part">
        <div class="button_list">
          <button v-for="button in buttons" :key="button.id" :style="{ backgroundImage: `url(${button.background})`, 
        backgroundColor: button.backgroundColor }" @click="buttonClick(button)" class="forum_button" 
        @mouseover="changeButtonColor(button, true)" @mouseout="changeButtonColor(button, false)">
        </button>
        </div>
        <div class="right_part">
            <input type="text" placeholder="搜索...">
            <ul class="post_list">
        <li v-for="post in paginatedPosts" :key="post">{{ post }}</li>
      </ul>
        </div>
    </div>
    <!-- 这里放置翻页按钮 -->
    <div class="pagination">
    <div class="page_info">
      当前页: {{ currentPage }} / 总页数: {{ totalPages }}
    </div>
    <div class="page_buttons">
      <button class="page_button" @click="goToPage(currentPage - 1)" :disabled="currentPage === 1">上一页</button>
      <button class="page_button" @click="goToPage(currentPage + 1)" :disabled="currentPage === totalPages">下一页</button>
    </div>
  </div>
</div>
</template>

<style scoped>
html, body {
  height: 100%;
  margin: 0;
  overflow-x: hidden; /* 隐藏横向滚动条 */
}

.forum_title {
  height: 50px; /* 根据需求设置标题的高度 */
  font-size: 24px; /* 根据需求设置标题的字体大小 */
}

.main_part {
  display: flex;
  flex-direction: row; /* 设置横向排列 */
  width: calc(100% - 40px); /* 减少宽度以避免超出视口 */
  margin-left: 20px; /* 调整左侧边距 */
}

.button_list {
  display: flex;
  align-items: flex-start;
  flex-direction: column;
  margin-top: 50px;
  margin-left: 20px; /* 调整左侧边距 */
}

.forum_button {
  width: 30px;
  height: 30px;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  background-repeat: no-repeat;
  background-size: cover;
  transition: background-color 0.3s ease; /* 添加过渡效果 */
  background-size: 100% 100%; /* 调整背景图像的尺寸 */
  margin-bottom: 20px;
}

input[type="text"] {
  width: 250px;
  height: 3vh; /* 调整搜索框的高度 */
  font-size: 16px; /* 调整输入文字的大小 */
  margin-bottom: 20px; /* 添加下方外边距 */
}

.right_part {
  margin-left: 20px; /* 调整左侧边距 */
  flex: 1;
}

.post_list {
  list-style-type: none;
  padding: 0;
  margin: 0;
}

.post_list li {
  border: 1px solid black; /* 添加边框，可以根据需要调整宽度和颜色 */
  padding: 10px; /* 添加内边距，以增加框与内容之间的间距 */
  margin-bottom: 10px; /* 添加外边距，以增加各个列表项之间的间距 */
  width: 100%; /* 设置宽度为父容器宽度 */
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
}

.page_info {
  margin-right: 20px;
}

.page_buttons .page_button {
  margin: 0 5px;
  padding: 5px 10px;
}
</style>
