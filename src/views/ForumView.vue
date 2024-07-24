<!-- 买家用户的论坛页面 -->
<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { useRouter } from 'vue-router';
import { ref, computed,reactive } from 'vue';
import { ElTable, ElTableColumn, ElPagination, ElButton, ElMessage ,ElInput} from 'element-plus';
import 'element-plus/dist/index.css';
import router from '@/router';
const search=ref('');
const postData=ref( [
        { title: '文章标题1', author: '作者1', content: '文章内容1' },
        { title: '文章标题2', author: '作者2', content: '文章内容2' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        // ... 其他文章数据
      ]);
const buttons = reactive([
  { id: 1, text: '发布', background: 'src/assets/czw/release.svg', backgroundColor: 'transparent' },
  { id: 2, text: '按热度排序', background: 'src/assets/czw/hot.svg', backgroundColor: 'transparent' },
  { id: 3, text: '按时间排序', background: 'src/assets/czw/time.svg', backgroundColor: 'transparent' },
  { id: 4, text: '消息中心', background: 'src/assets/czw/ring.svg', backgroundColor: 'transparent' },
]);
const changeButtonColor = (button, isHovered) => {
  if (isHovered) {
    button.backgroundColor = '#87c2a5'; // 设置鼠标悬停时的背景颜色
  } else {
    button.backgroundColor = 'transparent'; // 恢复背景颜色为透明
  }
};

const buttonClick = (button) => {
  if (button.id === 1) {
    router.push('/releasepost'); // 跳转至 /releasepost 页面
  }
 else  if (button.id === 4) {
  router.push('/messageview'); // 跳转至 /messageview 页面
    //在这里写跳转到messageView
    //在这里写跳转到messageView
    //在这里写跳转到messageView
    //在这里写跳转到messageView
    //在这里写跳转到messageView
    //在这里写跳转到messageView
    //在这里写跳转到messageView
    
  }
};

function highlightTitle(post) {
      // 在这里修改标题的样式或应用其他效果
      // 例如，可以通过修改post对象的属性来实现样式变化
      post.highlighted = true;
    };

const currentPage = ref(1);
const pageSize=ref(15);
const totalItems = 20; // 文章总数

function handleSizeChange(val) {
        console.log(`每页 ${val} 条`);
        pageSize.value = val;
        currentPage.value = 1; // 切换每页条数时，重置当前页码为第一页
      };
function  handleCurrentChange(val) {
        console.log(`当前页: ${val}`);
        currentPage.value = val;

      }

const displayedPosts=computed(()=> {
      // 根据当前页码和每页条数计算需要显示的文章列表
      const startIndex = (currentPage.value - 1) * pageSize.value;
      const endIndex = startIndex + pageSize.value;
      return postData.value.slice(startIndex, endIndex);
    })

function handleTitleClick(event, post) {
  router.push('/viewpost'); // 跳转至 /viewpost 页面
        // 在这里跳转到viewpost
        // 在这里跳转到viewpost
        // 在这里跳转到viewpost
        // 在这里跳转到viewpost
        // 在这里跳转到viewpost
        // 在这里跳转到viewpost
}
</script>

<template>
  <Navbar />
  <h1 class="forum_title">论坛</h1>
  <el-container style="height:100vh; border: 1px solid #eee">
    <el-aside width="25vh" style="background-color: rgb(238, 241, 246)">
      <el-menu>
        <div class="button_list">
          <button v-for="button in buttons" :key="button.id" :style="{ backgroundImage: `url(${button.background})`, 
        backgroundColor: button.backgroundColor }" @click="buttonClick(button)" class="forum_button" 
        @mouseover="changeButtonColor(button, true)" @mouseout="changeButtonColor(button, false)">
       </button>
        </div>
    </el-menu>
    </el-aside>
    <el-container>
    <el-main>
      <el-input
      class="search_input"
    placeholder="请输入关键词"
    v-model="search">
    <i slot="suffix" class="el-input__icon el-icon-search"></i>
  </el-input>
  <el-menu>
    <el-menu-item v-for="(post, index) in  displayedPosts" :key="index">
      <a class="post_title" href="#" @click.prevent="handleTitleClick(post)" @mouseover="highlightTitle(post)">{{ post.title }}</a>
      <div>{{ post.author }}</div>
      <div>{{ post.content }}</div>
    </el-menu-item>
  </el-menu>
  <div style="display: flex; justify-content: center; margin-top: 1vh;">
  <el-pagination
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
      :current-page="currentPage"
      :page-sizes="[15,10]"
      :page-size="pageSize"
      layout="total, sizes, prev, pager, next, jumper"
      :total="totalItems">
    </el-pagination>
  </div>
    </el-main>
    </el-container>
  </el-container>
</template>
  
<style scoped>

 .forum_title {
  height: 10vh; /* 根据需求设置标题的高度 */
  font-size: calc(10vh * 0.8);  /* 根据需求设置标题的字体大小 */
  background-color: #287658; /* 设置背景颜色 */
  font-family: Arial, sans-serif;
  display: flex;
  align-items: center;
  justify-content: center;
}

  .main_part {
    display: flex;
   /* align-items: center;*/
    flex-direction: row; /* 设置横向排列 */
    /*border: 1px solid black; /* 添加边界样式 */ 
    /*padding: 10px; /* 添加内边距以增加边界与内容之间的间距 */
}
 .button_list {
  display:flex;
  align-items: flex-start;
  flex-direction: column;
  margin-left:15vh; 
 }
 .forum_button {
    width: 3vh;
  height: 3vh;
  /* 按钮样式 */
  border: none;
  cursor: pointer;
  border-radius: 4px;
  /* 其他样式 */
  background-repeat: no-repeat;
  background-size: cover;
  transition: background-color 0.3s ease; /* 添加过渡效果 */
  background-size: 100% 100%; /* 调整背景图像的尺寸 */
  margin-bottom:2vh;
  }
  .search_input {
    width: 35vh;
    height: 3vh; /* 调整搜索框的高度 */
    font-size: calc(3vh * 0.6); /* 调整输入文字的大小 */
    left: 25%;
    /* 其他样式属性 */
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
    width: 1300px; /* 设置宽度 */
    /*height: 200px; /* 设置高度 */
}
.post_title {
    color: #000; /* 默认标题颜色 */
    text-decoration: none;
    transition: color 0.2s ease; /* 动画过渡效果 */
  }

  .post_title:hover {
    color: #f00; /* 鼠标移上去时的标题颜色 */
  }

</style>