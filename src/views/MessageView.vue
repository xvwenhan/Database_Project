<!-- 论坛的消息中心 -->
<script setup lang="ts">
import { ref, computed,reactive } from 'vue';
import { ElMenu,ElInput} from 'element-plus';
import 'element-plus/dist/index.css';
import router from '@/router';
const news=ref( [
        { title: '文章标题1', author: '作者1', content: '文章内容1' },
        { title: '文章标题2', author: '作者2', content: '文章内容2' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        { title: '文章标题3', author: '作者3', content: '文章内容3' },
        // ... 其他文章数据
      ]);

const likes=ref([
  {content:'莱卡恩给我点赞'},
  {content:'艾莲给我点赞'},
  {content:'丽娜给我点赞'},
  {content:'可琳给我点赞'},
  {content:'妮可给我点赞'},
  {content:'安比给我点赞'},
  {content:'比利给我点赞'},
]);
const comments=ref([
  {content:'莱卡恩给我评论'},
  {content:'艾莲给我评论'},
  {content:'丽娜给我评论'},
  {content:'可琳给我评论'},
  {content:'妮可给我评论'},
  {content:'安比给我评论'},
  {content:'比利给我评论'},
]);

const option=ref('news');

function myNews(){
     option.value='news';
};
function myLikes(){
  option.value='likes';
};
function myComments(){
  option.value='comments';
};

const button = reactive([
  { id: 1, text: 'like', background: 'src/assets/czw/back.svg', backgroundColor: 'transparent' },

  
]);
const changeButtonColor = (button, isHovered) => {
  if (isHovered) {
    if(button.id==1)
    button.backgroundColor = '#87c2a5'; // 设置鼠标悬停时的背景颜色
    else if(button.id==2)
    button.backgroundColor = '#5169e6'; 
  } else {
    button.backgroundColor = 'transparent'; // 恢复背景颜色为透明
  }
};
const buttonClick = (button) => {
  if(button.id==1){
    router.push('/forum'); // 跳转回 /forum 页面
  }
};

</script>

<template>
   <el-container>
    <el-aside width="200px" style="background-color: rgb(238, 241, 246)">
      <h2>消息中心</h2>
    <el-menu :default-openeds="['1', '3']">
      <el-menu-item index="1" @click="myNews">
        <i class="el-icon-menu"></i>
        <span slot="title">我的消息</span>
      </el-menu-item>
      <el-menu-item index="2"  @click="myComments">
        <i class="el-icon-menu"></i>
        <span slot="title">回复我的</span>
      </el-menu-item>
      <el-menu-item index="3"  @click="myLikes">
        <i class="el-icon-menu"></i>
        <span slot="title">收到的赞</span>
      </el-menu-item>
    </el-menu>
  </el-aside>
   <el-container>
    <el-header style="text-align: left">
      <button :style="{ backgroundImage: `url(${button[0].background})`, 
        backgroundColor: button[0].backgroundColor }" @click="buttonClick(button[0])" class="back_button" 
        @mouseover="changeButtonColor(button[0], true)" @mouseout="changeButtonColor(button[0], false)"></button>
      <div v-if="option === 'news'" style="line-height: 6vh;">
        <span style="font-size: 2vh; color: #333;">我的消息</span>
      </div>
      <div v-if="option === 'likes'" style="line-height: 6vh;">
        <span style="font-size: 2vh; color: #333;">收到的赞</span>
      </div>
      <div v-if="option === 'comments'" style="line-height: 6vh;">
        <span style="font-size: 2vh; color: #333;">回复我的</span>
      </div>
    </el-header>
    <el-menu>
      <div v-if="option === 'news'">
    <el-menu-item v-for="(anew, index) in  news" :key="index">
      <a class="post_title" href="#">{{ anew.title }}</a>
      <div>{{ anew.author }}</div>
      <div>{{ anew.content }}</div>
    </el-menu-item>
      </div>

      <div v-if="option === 'likes'">
    <el-menu-item v-for="(like, index) in  likes" :key="index">
      <div>{{ like.content }}</div>
    </el-menu-item>
      </div>

      <div v-if="option === 'comments'">
    <el-menu-item v-for="(comment, index) in  comments" :key="index">
      <div>{{ comment.content }}</div>
    </el-menu-item>
      </div>
  </el-menu>

  <el-main>


  </el-main>
</el-container>
</el-container>
    
</template>

<style scoped>
.el-header {
    background-color: #B3C0D1;
    color: #333;
    line-height: 18vh;
  }
  .back_button {
  float: left;
  width: 5vh;
  height:5vh;
  background-repeat: no-repeat;
  background-size: cover;
  transition: background-color 0.3s ease; /* 添加过渡效果 */
  background-size: 100% 100%; /* 调整背景图像的尺寸 */
  border: none;
}

</style>
