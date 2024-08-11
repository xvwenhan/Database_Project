<!-- 论坛的查看具体帖子页面 -->
<script setup lang="ts">
import { ref, reactive } from 'vue';
import {  ElButton, ElInput } from 'element-plus';
import 'element-plus/dist/index.css';
import router from '@/router';
import axiosInstance from '../components/axios';
// 假设这些数据是从服务器获取的
const dialogVisible =ref(false);
const inputComment = ref('');
const postIdString = localStorage.getItem('postId');
const way = localStorage.getItem('way');
const postId=ref('');
const subId=ref('');
const makeCommentId=ref('');
let dataLoaded = false;
console.log('postIdString:', postIdString);
console.log('way:', way);
let allSubComments = [];
// 检查获取的值是否是有效的 JSON 字符串
if (postIdString) {
  try {
    postId.value = JSON.parse(JSON.stringify(postIdString)); // 将数字转换为字符串再解析
    console.log("成功ID");
  } catch (error) {
    console.error('Error parsing JSON:', error);
  }
}
interface Comment {
  id: string;
  author: string;
  time: string;
  content: string;
  isSubMakeComment: boolean,
  subCollapsed: boolean,
}
const post = ref<{
  title: string,
  author: string,
  content:string,
  time: string,
  liked:boolean,
  isMakeComment: boolean,
  collapsed: boolean,
  likeCount: string,
  commentCount: string,
  reason: string,
  reason_else: string,
  comments: Array<Comment>,
  images: Array<string>
}>({
  title: '',
  author: '',
  content:'',
  time: '',
  liked:false,
  isMakeComment: false,
  collapsed: false,
  likeCount: '0',
  commentCount: '0',
  reason: '',
  reason_else: '',
  comments: [],
  images: [],
});
interface subComment {
  fatherId:string,
  id: string;
  author: string;
  time: string;
  content: string;
}
const subComments = ref<{
  sub: Array<subComment>
}>({
  sub:[]
});
const message = ref('');
const fetchPost = async () => {
  console.log("正在获取");
  axiosInstance.get(`/Post/get_a_post/${ postId.value}`)
    .then(response => {
      const data = response.data;
      if (data && data.data) {
        const postData = data.data;
        const images = postData.images || [];
        post.value =reactive( {
          title: postData.postTitle || '',
          author: postData.authorName || '',
          content: postData.postContent || '',
          time:  convertToReadableTime(postData.releaseTime) || '',
          liked:postData.liked|| false,
          isMakeComment: false,
          collapsed: true,
          likeCount: postData.numberOfLikes || '0',
          commentCount: postData.numberOfComments || '0',
          reason: '', // 初始化为''
          reason_else: '', // 初始化为''
          comments: postData.comments.map(comment => ({
            id: comment.commentId || '',
            author: comment.authorName || '',
            time: convertToReadableTime(comment.commentTime) || '',
            content: comment.commentContent || '',
            isSubMakeComment: false,
            subCollapsed: true,
          })),
          images: images.map(image => image.imageUrl) // 提取每个图片的 URL
        });
        console.log("成功获取");
        getSub();
        dataLoaded=true;
        console.log("加载完毕");
      } else {
        console.error('Invalid data format.');
      }
    })
    .catch(error => {
      message.value = "失败";
    });
}
fetchPost();
const fetchComment = async () => {
  console.log("正在获取二级");
  axiosInstance.get(`/Post/get_sub_comments/${ subId.value}`)
    .then(response => {
      const data = response.data;
       if (data && data.data) {
        subComments.value = {
          sub: data.data.map(comment => ({
            fatherId: comment.commentedCommentId,
            id:comment.commentId,
            author: comment.authorName || '',
            time: comment.commentTime || '',
            content: comment.commentContent || '',
          })),
        };
        console.log("成功获得二级评论");
        allSubComments.push(...subComments.value.sub);
        console.log(allSubComments);
      } else {
        console.error('Invalid data format.');
      }
    })
    .catch(error => {
      message.value = "失败二级";
    });
}
function getSub()
{
  post.value.comments.forEach(comment => {
    subId.value=comment.id;
    fetchComment();
  });
}
const toggleReplies = (post) => {
  post.collapsed = !post.collapsed;
};
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
function reply() {
  post.value.isMakeComment = true;
}
function replySub(id) {
  let found = false;
  post.value.comments.forEach(comment => {
    if (!found && comment.id === id) {
      comment.isSubMakeComment = true;
      found=true; 
    }
  });
}
const submitReply= async () =>{
  post.value.isMakeComment=false;
  const formData = new FormData();
      formData.append('postId', postId.value);
      formData.append('commentContext', inputComment.value);
      axiosInstance.post('/Post/add_comment_to_post', formData, {
      }).then(response => {
        console.log("评论成功");
      }).catch(error => {
        console.error(error);
      });
}
function submit(id){
  let found = false;
  post.value.comments.forEach(comment => {
    if (!found && comment.id === id) {
      comment.isSubMakeComment = false;
      found=true; 
    }
  });
  makeCommentId.value=id;
  submitSubReply();
};
const submitSubReply= async () =>{
  const formData = new FormData();
      formData.append('commentId', makeCommentId.value);
      formData.append('commentContext', inputComment.value);
      axiosInstance.post('/Post/add_comment_to_comment', formData, {
      }).then(response => {
        console.log("评论成功");
      }).catch(error => {
        console.error(error);
      });
}
function cancel() {
  post.value.isMakeComment = false;
}
function cancelSub(id) {
  let found = false;
  post.value.comments.forEach(comment => {
    if (!found && comment.id === id) {
      comment.isSubMakeComment = false;
      found=true; 
    }
  });
}
function resetForm() {
  post.value.reason = '';
  post.value.reason_else = '';
}
const submitReason=async()=>{
  const formData = new FormData();
      formData.append('postId',postId.value );
      if (post.value.reason) {
        formData.append('reportReason', post.value.reason);
      } else if (post.value.reason_else) {
        formData.append('reportReason', post.value.reason_else);
      }
      axiosInstance.post('/Post/report_post', formData, {
      }).then(response => {
        console.log(response.data);
      }).catch(error => {
        console.error(error);
      });
}
const button = reactive([
  { id: 1, text: 'like', background: 'src/assets/czw/like.svg', backgroundColor: 'transparent' },
  { id: 2, text: 'like', background: 'src/assets/czw/reply.svg', backgroundColor: 'transparent' },
  { id: 3, text: 'like', background: 'src/assets/czw/back.svg', backgroundColor: 'transparent' },
  { id: 4, text: 'liked', background: 'src/assets/czw/liked.svg', backgroundColor: 'transparent' },
  
]);
const like=async()=>{
  if(post.value.liked==false){
  const formData = new FormData();
      formData.append('postId',postId.value );
      axiosInstance.post('/Post/like_post', formData, {
      }).then(response => {
        console.log(response.data);
      }).catch(error => {
        console.error(error);
      });
      post.value.liked=true;
  }
  else{
    const formData = new FormData();
      formData.append('postId',postId.value );
      axiosInstance.post('/Post/unlike_post', formData, {
      }).then(response => {
        console.log(response.data);
      }).catch(error => {
        console.error(error);
      });
      post.value.liked=false;
  }
}

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
  if(button.id==3&&way=='forum'){
    router.push('/forum'); // 跳转回 /forum 页面
  }
  if(button.id==3&&way=='message'){
    router.push('/messageview'); // 跳转回 /forum 页面
  }
};

function currentBackground() {
  console.log(post.value.liked);
  console.log(post.value.likeCount);
  if(post.value.liked==false)
    return this.button[0].background;
  else
    return this.button[3].background;
};
function currentBackgroundColor() {
  if(post.value.liked==false)
    return this.button[0].backgroundColor;
  else
    return this.button[3].backgroundColor;
};
function toggleSubComments(fatherId) {
  let found = false;
  post.value.comments.forEach(comment => {
    if (!found && comment.id === fatherId) {
      comment.subCollapsed = !comment.subCollapsed;
      found=true; 
    }
  });
};
function getSubComments(fatherId) {
      return allSubComments.filter(comment => comment.fatherId === fatherId);
};
function hasSubComments(fatherId) {
    return allSubComments.some(comment => comment.fatherId === fatherId);
};
function highlightPost(event) {
      event.currentTarget.style.backgroundColor = 'lightblue';
};
function resetPost(event) {
      event.currentTarget.style.backgroundColor = 'initial';
}
</script>

<template>

    <el-header>{{ post.title }}
      <button :style="{ backgroundImage: `url(${button[2].background})`, 
        backgroundColor: button[2].backgroundColor }" @click="buttonClick(button[2])" class="back_button" 
        @mouseover="changeButtonColor(button[2], true)" @mouseout="changeButtonColor(button[2], false)"></button>
    </el-header>
    <el-main>
    <div class="data">
      <button :style="{ backgroundImage: `url(${button[0].background})`, 
        backgroundColor: button[0].backgroundColor }" class="like_button"></button>{{ post.likeCount }}
        <button :style="{ backgroundImage: `url(${button[1].background})`, 
        backgroundColor: button[1].backgroundColor }" class="like_button"></button>{{ post.commentCount }}
      <div class="report_button">
      <el-button type="text" @click="dialogVisible = true">举报</el-button>
    </div>
<el-dialog
:draggable="true"
  v-model="dialogVisible" title="举报详情" width="460px" @close="resetForm">
  <el-select v-model="post.reason" placeholder="请选择原因">
        <el-option label="违反法律法规" value="1"></el-option>
        <el-option label="不实信息" value="2"></el-option>
        <el-option label="侵犯个人权益" value="3"></el-option>
        <el-option label="扰乱社区环境" value="4"></el-option>
        <el-option label="其他" value="5"></el-option>
      </el-select>
      <div style="margin-top: 10px;"></div>
      <el-input v-if="post.reason === '5' "v-model="post.reason_else" placeholder="请输入原因"></el-input>
  <el-divider></el-divider>
  <div slot="footer" class="dialog-footer">
    <el-button type="primary" @click="submitReason()">确 定</el-button>
    </div>
</el-dialog>

    </div>
    <div class="main_content">
    <p>{{ post.content }}</p>
    <div v-if="post.images.length > 0">
      <ul class="image-list">
    <li v-for="(image, index) in post.images" :key="index">
      <img :src="image" class="show_image">
    </li>
  </ul>
</div>
    </div>
    <el-container>
      <div class="hh">
        <transition name="fade">
        <button :style="{  backgroundImage: `url(${currentBackground()})`,
      backgroundColor: currentBackgroundColor() }" class="like_button" @click="like()"></button></transition>
         <button :style="{ backgroundImage: `url(${button[1].background})`, 
        backgroundColor: button[1].backgroundColor }" class="like_button" @click="reply()"></button>
        </div>
    </el-container>
    <el-container>
      <div v-if="post.isMakeComment" class="make_comment">
        <el-input
        class="input_comment"
          type="textarea"
         :autosize="{ minRows: 2, maxRows: 4}"
          placeholder="请输入"
          v-model="inputComment">
          </el-input>
          <el-button type="primary" plain @click="submitReply()">确认</el-button>
          <el-button type="danger" plain @click="cancel()">取消</el-button>
      </div>
    </el-container>
    <el-container>
      <div v-if="!post.collapsed">
    <!-- <el-menu> -->
    <div v-for="(reply, index) in  post.comments" :key="index" class="comments">
      <div  @mouseover="highlightPost" @mouseout="resetPost">
      <div class="user-line">用户：{{ reply.author }}</div>
      <div>{{ reply.content}}</div>
      </div>
      <el-container>
      <div v-if="!reply.subCollapsed">
    <!-- <el-menu> -->
      <div v-for="(subComment, subIndex) in getSubComments(reply.id)" :key="subIndex" class="subComments">
      <div  @mouseover="highlightPost" @mouseout="resetPost">
      <div class="user-line">用户：{{ subComment.author }}</div>
      <div>{{ subComment.content}}</div>
      </div>
      </div>
  <!-- </el-menu> -->
    </div>
</el-container>
<div class="sub">
<button :style="{ backgroundImage: `url(${button[1].background})`, 
        backgroundColor: button[1].backgroundColor }" class="like_button" @click="replySub(reply.id)"></button>
<button v-if="hasSubComments(reply.id)" @click="toggleSubComments(reply.id)" class="show_sub_reply">显示/隐藏回复</button>
</div>
<el-container>
      <div v-if="reply.isSubMakeComment" class="make_comment">
        <el-input
        class="input_sub_comment"
          type="textarea"
         :autosize="{ minRows: 2, maxRows: 4}"
          placeholder="请输入"
          v-model="inputComment">
          </el-input>
          <el-button type="primary" plain @click="submit(reply.id)">确认</el-button>
          <el-button type="danger" plain @click="cancelSub(reply.id)">取消</el-button>
      </div>
    </el-container>
</div>
  <!-- </el-menu> -->
</div>
</el-container>
      <button @click="toggleReplies(post)" class="show_reply">显示/隐藏回复</button>
</el-main>
</template>

<style scoped>
.el-header {
    background-color: #B3C0D1;
    color: #333;
    text-align: center;
    line-height: 60px;
    margin-left: 14vh;
  width:150vh;
}
.data{
  display: flex;
  justify-content: flex-end;
  margin-right: 120px;
}
.like_button{
  width: 25px;
  height: 25px;
  /* 按钮样式 */
  border: none;
  cursor: pointer;
  border-radius: 4px;
  /* 其他样式 */
  background-repeat: no-repeat;
  background-size: cover;
  transition: background-color 0.3s ease; /* 添加过渡效果 */
  background-size: 100% 100%; /* 调整背景图像的尺寸 */
  margin-right: 3px;
  margin-left: 5px;
}
.report_button {
  margin-left: 5px;
  
}
.report_button .el-button {
  background-color: #ffffff;
  color: #456df1;
  padding: 8px 10px;
  border-radius: 4px;
  border:none;
}
.main_content{
  border: none; /* 设置边框样式 */
  padding: 10px; /* 可选：设置内边距 */
  margin-bottom: 10px;
  background-color: #c6d2fa;
  margin-left: 12vh;
  width:150vh;
}
.hh{
  display: flex;
  flex-direction: row;
  margin-left: 12vh;
}
.show_reply{
  display: flex;
  justify-content: flex-start;
  margin-left: 12vh;
  background-color: #ffffff;
  color: #96affe;
  border:none;
}
.input_comment{
  width: 120vh;       /* 设置输入框宽度 */
}
.input_sub_comment{
  width: 120vh;
  margin-left:-12vh;
}
.comments{
  margin-left: 12vh;
  width:150vh;
  text-align: left;
  margin-bottom: 1vh;
  margin-top: 1vh;
}
.make_comment{
  display: flex;
  justify-content: flex-start;
  
  align-items: flex-end;
}
.make_comment .el-button {
        flex: 0 0 auto; /* 防止按钮拉伸 */
        font-size: 12px; /* 按钮字体大小 */
        padding: 5px 10px; /* 按钮内边距 */
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

.input_comment{
  width: 120vh;       /* 设置输入框宽度 */
}
.comments{
  margin-left: 12vh;
  width:150vh;
}
.subComments{
  margin-left: 5vh;
  width:145vh;
}
.make_comment{
  display: flex;
  justify-content: flex-start;
  margin-left:12vh;
  
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
.show_image{
  max-width: 35vh;
  max-height: 35vh;
}
.image-list {
  display: flex;
  flex-direction: row;
    list-style-type: none;
    padding: 0; /* Remove default padding */
  }
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.5s;
}
.fade-enter, .fade-leave-to {
  opacity: 0;
}
.show_sub_reply{
  display: flex;
  justify-content: flex-start;
  
  background-color: #ffffff;
  color: #96affe;
  border:none;
}
.sub{
  display: flex;
  flex-direction: row;
}
.user-line {
  font-size: 12px; /* 调整字体大小 */
}
</style>