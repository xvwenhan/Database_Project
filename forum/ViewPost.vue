<script setup lang="ts">
import { ref, reactive ,watch} from 'vue';
import { ElTable, ElTableColumn, ElPagination, ElButton, ElInput,ElIcon } from 'element-plus';
import 'element-plus/dist/index.css';
// 假设这些数据是从服务器获取的
const likeCount = ref(10);
const commentCount = ref(5);
const dialogVisible =ref(false);
const post = reactive({
  title: '示例帖子',
  content: '这是一个示例帖子的内容。',
  isMakeComment:false,
  form: {
          reason:'',
          reason_else:''
        },
        formLabelWidth: '120px',
  comments: [
    {
      id: 1,
      content: '评论一',
      author: 'Alice',
      replies: [
        {
          id: 11,
          content: '回复一',
          author: 'Bob'
        },
        {
          id: 12,
          content: '回复二',
          author: 'Charlie'
        }
      ],
      collapsed: false,
      isMakeComment:false,
      
    },
    {
      id: 2,
      content: '评论二',
      author: 'Bob',
      replies: [
        {
          id: 21,
          content: '回复一',
          author: 'Alice'
        }
      ],
      collapsed: false,
      isMakeComment:false,
    }
  ]
});

const toggleReplies = (comment) => {
  comment.collapsed = !comment.collapsed;
};

function report() {
  // 处理举报逻辑
}
function reply(post) {
  post.isMakeComment = true;
}

function submitReply(){

}
const inputComment = ref('');
const inputHeight = ref(30);

const adjustHeightContent = () => {
  const textarea = document.querySelector('.input_comment');
  textarea.style.height = `${textarea.scrollHeight}px`;
  inputHeight.value = textarea.scrollHeight;
};

watch(inputComment, (newVal) => {
  if (newVal === '') {
    const textarea = document.querySelector('.input_comment');
    textarea.style.height = '30px'; // 将高度重置为最小高度
    inputHeight.value = 30;
  }
});

function cancel(post) {
  post.isMakeComment = false;
}

function resetForm()
{
  post.form.reason = '';
  post.form.reason_else = '';
}

const button = reactive([
  { id: 1, text: 'like', background: 'src/assets/czw/like.svg', backgroundColor: 'transparent' },
  { id: 2, text: 'like', background: 'src/assets/czw/reply.svg', backgroundColor: 'transparent' },
  
]);
function like(post){

}

</script>

<template>
  <div>
    <h1>{{ post.title }}</h1>
    <div class="data">
      <button :style="{ backgroundImage: `url(${button[0].background})`, 
        backgroundColor: button[0].backgroundColor }" class="like_button"></button>{{ likeCount }}
        <button :style="{ backgroundImage: `url(${button[1].background})`, 
        backgroundColor: button[1].backgroundColor }" class="like_button"></button>{{ commentCount }}
      <div class="report_button">
      <el-button type="text" @click="dialogVisible = true">举报</el-button>
    </div>
<el-dialog
  v-model="dialogVisible" title="举报详情" width="460px" @close="resetForm">
  <el-select v-model="post.form.reason" placeholder="请选择原因">
        <el-option label="违反法律法规" value="1"></el-option>
        <el-option label="不实信息" value="2"></el-option>
        <el-option label="侵犯个人权益" value="3"></el-option>
        <el-option label="扰乱社区环境" value="4"></el-option>
        <el-option label="其他" value="5"></el-option>
      </el-select>
      <div style="margin-top: 10px;"></div>
      <el-input v-if="post.form.reason === '5' "v-model="post.form.reason_else" placeholder="请输入原因"></el-input>
  <el-divider></el-divider>
</el-dialog>

    </div>
    <div class="main_content">
    <p>{{ post.content }}</p>
    </div>
    <div>
    <ul>
      <div class="hh">
      <button @click="like(post)" class="first_reply_button1">点赞</button> 
      <button @click="reply(post)" class="first_reply_button">回复</button> 
    </div>
      <div v-if="post.isMakeComment" class="make_comment">
        <el-input
        class="input_comment"
          type="textarea"
         :autosize="{ minRows: 2, maxRows: 4}"
          placeholder="请输入"
           v-model="inputComment">
          </el-input>
          <el-button type="success" plain @click="submitReply()">确认</el-button>
          <el-button type="danger" plain @click="cancel(post)">取消</el-button>
      </div>
      <li v-for="comment in post.comments" :key="comment.id" class="first_comments">
        <p>{{ comment.content }}</p>
        <p>作者：{{ comment.author }}</p>
        <div class="button_container">
        <button @click="toggleReplies(comment)" class="show_reply">显示/隐藏回复</button>
        <button @click="reply(comment)" class="second_reply_button">回复</button>
        </div>
        <div v-if="comment.isMakeComment" class="make_comment2">
          <el-input
          class="input_comment"
          type="textarea"
         :autosize="{ minRows: 2, maxRows: 4}"
          placeholder="请输入"
           v-model="inputComment">
          </el-input>
        <button @click="submitReply()" class="submit_reply_button" style="height: 30px;">提交</button>
        <button @click="cancel(comment)" class="cancel_button" style="height: 30px;">取消</button>
      </div>
        <ul v-if="!comment.collapsed">
          <li v-for="reply in comment.replies" :key="reply.id" class="reply_comments">
            <p>{{ reply.content }}</p>
            <p>作者：{{ reply.author }}</p>
          </li>
        </ul>
      </li>
    </ul>
  </div>
  </div>
</template>

<style>
.hh{
  display: flex;
  flex-direction: row;
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
  border: 1px solid #000000; /* 设置边框样式 */
  padding: 10px; /* 可选：设置内边距 */
  margin-left: 120px; /* 添加左侧外边距 */
  margin-right: 120px; /* 添加右侧外边距 */
 
  margin-bottom: 10px;
}
.first_comments {
  list-style-type: none;
  border: 1px solid #000000; /* 设置边框样式 */
  padding: 10px; /* 可选：设置内边距 */
  margin-left: 80px; /* 添加左侧外边距 */
  margin-right: 120px; /* 添加右侧外边距 */
}
.first_reply_button{
  background-color: #ffffff;
  color: #456df1;
  border:none;
  display: flex;
  justify-content: flex-start;
  margin-left: 5px;
  margin-bottom: 10px;
  
}
.first_reply_button1{
  background-color: #ffffff;
  color: #456df1;
  border:none;
  display: flex;
  justify-content: flex-start;
  margin-left: 80px;
  margin-bottom: 10px;
  
}
.second_reply_button{
  background-color: #ffffff;
  color: #456df1;
  border:none;
}
.reply_comments{
  list-style-type: none;
  border: 1px solid #000000; /* 设置边框样式 */
  padding: 10px; /* 可选：设置内边距 */
  margin-left: -40px;
  
}
.show_reply{
  display: flex;
  justify-content: flex-start;
  margin-right: 3px;
  background-color: #ffffff;
  color: #456df1;
  border:none;
}
.button_container{
  display: flex;
  flex-direction: row;
}
.make_comment{
  display: flex;
  justify-content: flex-start;
  margin-left:80px;
  margin-bottom:10px;
  
}
.make_comment2{
  display: flex;
  justify-content: flex-start;
  margin-bottom:10px;
  
}
.input_comment{
  width: 120vh;       /* 设置输入框宽度 */
}
</style>