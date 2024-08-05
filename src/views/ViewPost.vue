<!-- 论坛的查看具体帖子页面 -->
<script setup lang="ts">
import { ref, reactive } from 'vue';
import {  ElButton, ElInput } from 'element-plus';
import 'element-plus/dist/index.css';
import router from '@/router';
import axiosInstance from '../components/axios';
// 假设这些数据是从服务器获取的
const dialogVisible =ref(false);
const liked =ref(false);
const inputComment = ref('');
const postIdString = localStorage.getItem('postId');
const postId=ref('');
console.log('postIdString:', postIdString);
// 检查获取的值是否是有效的 JSON 字符串
if (postIdString) {
  try {
    postId.value = JSON.parse(JSON.stringify(postIdString)); // 将数字转换为字符串再解析
    console.log("成功ID");
  } catch (error) {
    console.error('Error parsing JSON:', error);
  }
}

// const post = reactive({
//   title: '示例帖子',
//   content: '这是一个示例帖子的内容。',
//   isMakeComment: false,
//   collapsed: false,
//   form: {
//     reason: '',
//     reason_else: ''
//   },
//   formLabelWidth: '120px',
//   comments: [
//     {
//       id: 1,
//       content: '评论一',
//       author: 'Alice',
//       isMakeComment: false,
//       replies: [
//         {
//           id: 11,
//           content: '评论一的回复一',
//           author: 'Carol'
//         },
//         {
//           id: 12,
//           content: '评论一的回复二',
//           author: 'David'
//         }
//       ]
//     },
//     {
//       id: 2,
//       content: '评论二',
//       author: 'Bob',
//       isMakeComment: false,
//       replies: [
//         {
//           id: 21,
//           content: '评论二的回复一',
//           author: 'Eve'
//         }
//       ]
//     }
//   ]
// });
interface Comment {
  id: string;
  author: string;
  time: string;
  content: string;
}
const post = ref<{
  title: string,
  author: string,
  content:string,
  time: string,
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
  isMakeComment: false,
  collapsed: false,
  likeCount: '0',
  commentCount: '0',
  reason: '',
  reason_else: '',
  comments: [],
  images: []
});
const message = ref('');
const formatDate = (dateStr) => {
          const date = new Date(dateStr);
          const formatter = new Intl.DateTimeFormat('en', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
          });
          return formatter.format(date);
};
const fetchPost = async () => {
  console.log("正在获取");
  axiosInstance.get(`/Post/get_a_post/${ postId.value}`)
    .then(response => {
      const data = response.data;
      if (data && data.data) {
        const postData = data.data;
        const images = postData.images || [];
        post.value = {
          title: postData.postTitle || '',
          author: postData.authorName || '',
          content: postData.postContent || '',
          time: formatDate(postData.releaseTime) || '',
          isMakeComment: false,
          collapsed: false,
          likeCount: postData.numberOfLikes || '0',
          commentCount: postData.numberOfComments || '0',
          reason: '', // 初始化为''
          reason_else: '', // 初始化为''
          comments: postData.comments.map(comment => ({
            id: comment.commentId || '',
            author: comment.authorName || '',
            time: formatDate(comment.commentTime) || '',
            content: comment.commentContent || ''
          })),
          images: images.map(image => image.imageUrl) // 提取每个图片的 URL
        };
        console.log("成功获取");
      } else {
        console.error('Invalid data format.');
      }
    })
    .catch(error => {
      message.value = "失败";
    });
}
fetchPost();

const toggleReplies = (post) => {
  post.collapsed = !post.collapsed;
};

function reply() {
  post.value.isMakeComment = true;
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
function cancel() {
  post.value.isMakeComment = false;
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

  
]);
const like=async()=>{
  if(liked.value==false){
  const formData = new FormData();
      formData.append('postId',postId.value );
      axiosInstance.post('/Post/like_post', formData, {
      }).then(response => {
        console.log(response.data);
      }).catch(error => {
        console.error(error);
      });
      liked.value=true;
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
      liked.value=false;
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
  if(button.id==3){
    router.push('/forum'); // 跳转回 /forum 页面
         
  }
};

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
        <li v-for="(image, index) in post.images" :key="index">
            <img :src="image" class="show_image">
        </li>
</div>
    </div>
    <el-container>
      <div class="hh">
        <button :style="{ backgroundImage: `url(${button[0].background})`, 
        backgroundColor: button[0].backgroundColor }" class="like_button" @click="like()"></button>
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
          <el-button type="success" plain @click="submitReply()">确认</el-button>
          <el-button type="danger" plain @click="cancel()">取消</el-button>
      </div>
    </el-container>
    <el-container>
      <div v-if="!post.collapsed">
    <el-menu>
    <el-menu-item v-for="(reply, index) in  post.comments" :key="index" class="comments">
      <div>{{ reply.content}}</div>
      <div>{{ reply.author }}</div>
    </el-menu-item>
  </el-menu>
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
  color: #456df1;
  border:none;
}
.input_comment{
  width: 120vh;       /* 设置输入框宽度 */
}
.comments{
  margin-left: 12vh;
  width:150vh;
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

.input_comment{
  width: 120vh;       /* 设置输入框宽度 */
}
.comments{
  margin-left: 12vh;
  width:150vh;
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
  max-width: 50vh;
  max-height: 50vh;
}

</style>