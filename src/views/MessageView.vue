<!-- 论坛的消息中心 -->
<script setup lang="ts">
import { ref, reactive } from 'vue';
import { ElMenu,} from 'element-plus';
import 'element-plus/dist/index.css';
import axiosInstance from '../components/axios';
import router from '@/router';
const commentId=ref('');
interface commentMessage {
  id:string;
  author: string;
  time: string;
  content: string;
  postTitle:string;
}
const way=ref('message');
const myCommentsMessage = ref<{
  comments: Array<commentMessage>
}>({
  comments:[]
});
const myCommentsMessageOld = ref<{
  comments: Array<commentMessage>
}>({
  comments:[]
});
const fetchComment = async () => {
  axiosInstance.get(`/Post/my_new_comments`)
    .then(response => {
      const data = response.data;
       if (data && data.data) {
        myCommentsMessage.value = {
          comments: data.data.map(comment => ({
            id:comment.commentId||'',
            author: comment.authorName || '',
            time: comment.commentTime || '',
            content: comment.commentContent || '',
            postTitle:comment.postTitle||'',
          })),
        };
        console.log("成功获得评论");
      } else {
        console.error('Invalid data format.');
      }
    })
    .catch(error => {
      console.log("失败获得评论");
    });
}
fetchComment();
const fetchCommentOld = async () => {
  axiosInstance.get(`/Post/my_read_comments`)
    .then(response => {
      const data = response.data;
       if (data && data.data) {
        myCommentsMessageOld.value = {
          comments: data.data.map(comment => ({
            id:comment.commentId||'',
            author: comment.authorName || '',
            time: comment.commentTime || '',
            content: comment.commentContent || '',
            postTitle:comment.postTitle||'',
          })),
        };
        console.log("成功获得评论过去");
      } else {
        console.error('Invalid data format.');
      }
    })
    .catch(error => {
      console.log("失败获得评论过去");
    });
}
fetchCommentOld();
interface likeMessage {
  id: string;
  time: string;
  postTitle:string;
}
const myLikeMessage = ref<{
  likes: Array<likeMessage>
}>({
  likes:[]
});
const fetchLike = async () => {
  axiosInstance.get(`/Post/my_liked_posts`)
    .then(response => {
      const data = response.data;
       if (data && data.data) {
        myLikeMessage.value = {
          likes: data.data.map(comment => ({
            id: comment.postId || '',
            time: comment.postReleaseTime || '',
            postTitle:comment.postTitle||'',
          })),
        };
        console.log("成功获得点赞");
      } else {
        console.error('Invalid data format.');
      }
    })
    .catch(error => {
      console.log("失败获得");
    });
}
fetchLike();
const myPosts = ref<Array<{ id: string, title: string, time: string ,like:string,comment:string}>>([]);
  const getMyPost = async () => {
  axiosInstance.get(`/Post/get_my_posts`)
    .then(response => {
        const data = response.data;
        if (data && Array.isArray(data.target_posts)) {
            myPosts.value = data.target_posts.map(post => ({
                id: post.posT_ID || '',
                title: post.posT_TITLE || '',
                time: convertToReadableTime(post.releasE_TIME) || '',
                like: post.numbeR_OF_LIKES || 0,
                comment: post.numbeR_OF_COMMENTS || 0
            }));
            console.log("成功获得帖子");
        } else {
            console.error('Invalid data format.');
        }
    }).catch(error => {
        console.error(error);
    });
};
getMyPost();
function convertToReadableTime(isoTime) {
  if (!isoTime) return '';
  
  const date = new Date(isoTime);

  // 使用toLocaleString()格式化日期并根据需要自定义选项
  return date.toLocaleString('zh-CN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  });
}
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
function markAsRead(id) {
  commentId.value=id;
  isRead();
}
const isRead=async()=>{
  axiosInstance.post(`/Post/mark_comment/${ commentId.value}`).then(response => {
        console.log("成功已读");
      }).catch(error => {
        console.log("失败已读");
      });
};
function viewPost(id,event)
{
  localStorage.setItem('postId', id);
  localStorage.setItem('way', way.value);
  router.push( 'viewpost'); // 跳转至 /viewpost 页面
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
        <span slot="title">我的点赞</span>
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
        <span style="font-size: 2vh; color: #333;">我的点赞</span>
      </div>
      <div v-if="option === 'comments'" style="line-height: 6vh;">
        <span style="font-size: 2vh; color: #333;">回复我的</span>
      </div>
    </el-header>
    <el-menu>
      <div v-if="option === 'news'">
    <el-menu-item v-for="(anew, index) in  myPosts" :key="index">
      <a class="post_title" href="#" @click="viewPost(anew.id)">{{ anew.title }}</a>
      <div>{{ anew.title }}</div>
      <div>{{ anew.time }}</div>
    </el-menu-item>
      </div>

      <div v-if="option === 'likes'">
    <el-menu-item v-for="(like, index) in  myLikeMessage.likes" :key="index">
      <a class="post_title" href="#"  @click="viewPost(like.id)">{{ like.postTitle }}</a>
      <div>{{ like.time }}</div>
    </el-menu-item>
      </div>

      <div v-if="option === 'comments'">
    <el-menu-item v-for="(comment, index) in  myCommentsMessage.comments" :key="index" @click="markAsRead(comment.id)">
      <span class="new-comment-dot">&#8226;</span>
      <div>{{ comment.content }} </div>
    </el-menu-item>
    <el-menu-item v-for="(comment, index) in  myCommentsMessageOld.comments" :key="index">
      <div>{{ comment.content }} </div>
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
.new-comment-dot {
  color: red; /* 设置红色点的样式 */
  margin-right: 5px; /* 距离评论内容一定距离 */
}
</style>
