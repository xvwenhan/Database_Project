<!-- 买家用户的论坛页面 -->
<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, computed,reactive } from 'vue';
import { ElPagination, ElInput} from 'element-plus';
import 'element-plus/dist/index.css';
import router from '@/router';
import axiosInstance from '../components/axios';
import type { RefSymbol } from '@vue/reactivity';
const search=ref('');
const sort=ref('time');
const currentPage = ref(1);
const pageSize=ref(15);
const totalPostNums = ref(); // 文章总数
const isSearching =ref(false);
// const postData=ref( [
//         { title: '文章标题1', author: '作者1', content: '文章内容1',isDisplayed:true,id:1 },
//         { title: '文章标题2', author: '作者2', content: '文章内容2',isDisplayed:true,id:2 },
//         { title: '文章标题3', author: '作者3', content: '文章内容3' ,isDisplayed:true,id:3},
//         { title: '文章标题4', author: '作者4', content: '文章内容4' ,isDisplayed:true,id:4},
//         { title: '文章标题5', author: '作者5', content: '文章内容5',isDisplayed:true,id:5 },
//         { title: '文章标题6', author: '作者6', content: '文章内容6',isDisplayed:true ,id:6},
//         { title: '文章标题7', author: '作者7', content: '文章内容7',isDisplayed:true ,id:7},
//         { title: '文章标题8', author: '作者8', content: '文章内容8',isDisplayed:true,id:8 },
//         { title: '文章标题9', author: '作者3', content: '文章内容3',isDisplayed:true ,id:9},
//         { title: '文章标题10', author: '作者3', content: '文章内容3' ,isDisplayed:true,id:10},
//         { title: '文章标题11', author: '作者3', content: '文章内容3' ,isDisplayed:true,id:11},
//         { title: '文章标题12', author: '作者3', content: '文章内容3',isDisplayed:true ,id:12},
//         { title: '文章标题13', author: '作者3', content: '文章内容3',isDisplayed:true,id:13},
//         { title: '文章标题14', author: '作者3', content: '文章内容3' ,isDisplayed:true,id:14},
//         { title: '文章标题15', author: '作者3', content: '文章内容3',isDisplayed:true ,id:15},
//         { title: '文章标题16', author: '作者3', content: '文章内容3',isDisplayed:true ,id:16},
//         { title: '文章标题17', author: '作者3', content: '文章内容3',isDisplayed:true ,id:17},
//         { title: '文章标题18', author: '作者3', content: '文章内容3',isDisplayed:true ,id:18},
//         { title: '文章标题19', author: '作者3', content: '文章内容3' ,isDisplayed:true,id:19},
//         { title: '文章标题20', author: '作者3', content: '文章内容3' ,isDisplayed:true,id:20},
//         { title: '文章标题21', author: '作者3', content: '文章内容3',isDisplayed:true ,id:21},
//         { title: '文章标题22', author: '作者3', content: '文章内容3',isDisplayed:true ,id:22},
//         { title: '文章标题23', author: '作者3', content: '文章内容3',isDisplayed:true ,id:23},
//         // ... 其他文章数据
//       ]);
const posts = ref<Array<{ id: string, title: string, author: string, time: string ,like:string,comment:string}>>([]);
const message=ref('');
const fetchPost= async () =>{
      axiosInstance.post('/Post/get_all_posts', {
        'sortBy':sort.value,
        'page':currentPage.value,
        'pageSize':pageSize.value
      }).then(response => {
        const data = response.data;
        if (data && Array.isArray(data.posts)) {
          // 解构帖子数组
          posts.value = data.posts.map(post => ({
            id: post.postId || '',
            title: post.postTitle || '',
            author: post.authorName || '',
            time: post.releaseTime || '',
            like:post.numberOfLikes || 0,
            comment:post.numberOfComments || 0
          }));
          
          // 设置总帖子数量
          totalPostNums.value = data.totalPostNums || 0;
          message.value="成功";
        } else {
          console.error('Invalid data format.');
        }
      }).catch(error => {
        message.value="失败";
      });
}
const displayedPosts=computed(()=> {
      fetchPost();
      console.log(message.value);
      console.log(sort.value);
      // 根据当前页码和每页条数计算需要显示的文章列表
      if (isSearching.value==false) {
        // 当处于搜索状态时，返回所有帖子
        return posts.value;
    } else {
        // 当不处于搜索状态时，根据当前页码和每页条数计算需要显示的文章列表
        const startIndex = (currentPage.value - 1) * pageSize.value;
        const endIndex = startIndex + pageSize.value;
        return posts.value.slice(startIndex, endIndex);
    }
      //else{
    //     return posts.value.filter(post => 
    //   post.title.toLowerCase().includes(search.value) ||
    //   post.author.toLowerCase().includes(search.value) ||
    //   post.content.toLowerCase().includes(search.value)
    // );
    //   }
    })

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
  }
  else  if (button.id === 2) {
  sort.value="likes";
  }
  else  if (button.id === 3) {
    sort.value="time";
  }
};
function highlightTitle(post) {
  
      post.highlighted = true;
    };


function handleSizeChange(val) {
        console.log(`每页 ${val} 条`);
        pageSize.value = val;
        currentPage.value = 1; // 切换每页条数时，重置当前页码为第一页
      };
function  handleCurrentChange(val) {
        console.log(`当前页: ${val}`);
        currentPage.value = val;
        //displayedPosts.value;
      }

function handleTitleClick( id,event) {
  localStorage.setItem('postId', id);
  router.push( 'viewpost'); // 跳转至 /viewpost 页面
}
function  handleSearch()
{
  isSearching.value=true;
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
    v-model="search"
    @input="handleSearch">
    <i slot="suffix" class="el-input__icon el-icon-search"></i>
  </el-input>
  <el-menu>
    <el-menu-item v-for="(post, index) in  displayedPosts" :key="index">
      <a class="post_title" href="#" @click.prevent="handleTitleClick(post.id)" @mouseover="highlightTitle(post)">{{ post.title }}</a>
      <div>{{ post.author }}</div>
      <div>{{ post.time }}</div>
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
      :total="totalPostNums">
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

.post {
  display: block;
}
.post-content {
  display: flex;
  flex-direction: column;
}
.post_title {
  color: #000; /* 默认标题颜色 */
  text-decoration: none;
  transition: color 0.2s ease; /* 动画过渡效果 */
}
.post_title:hover {
  color: rgb(239, 153, 153); /* 鼠标移上去时的标题颜色 */
}

</style>