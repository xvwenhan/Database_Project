<!-- 论坛的查看具体帖子页面 -->
<script setup lang="ts">
import { ref, reactive ,watch} from 'vue';
import { ElTable, ElTableColumn, ElPagination, ElButton, ElInput,ElIcon } from 'element-plus';
import 'element-plus/dist/index.css';
import router from '@/router';
// 假设这些数据是从服务器获取的
const likeCount = ref(10);
const commentCount = ref(5);
const dialogVisible =ref(false);
const post = reactive({
  title: '示例帖子',
  content: '这是一个示例帖子的内容。',
  isMakeComment:false,
  collapsed: false,
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
      isMakeComment:false,
      
    },
    {
      id: 2,
      content: '评论二',
      author: 'Bob',
      isMakeComment:false,
    }
  ]
});

const toggleReplies = (post) => {
  post.collapsed = !post.collapsed;
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
  { id: 3, text: 'like', background: 'src/assets/czw/back.svg', backgroundColor: 'transparent' },

  
]);
function like(post){
    likeCount.value++;
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
    //处理返回
     //处理返回
      //处理返回
       //处理返回
        //处理返回
         //处理返回到ForunView
         
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
        backgroundColor: button[0].backgroundColor }" class="like_button"></button>{{ likeCount }}
        <button :style="{ backgroundImage: `url(${button[1].background})`, 
        backgroundColor: button[1].backgroundColor }" class="like_button"></button>{{ commentCount }}
      <div class="report_button">
      <el-button type="text" @click="dialogVisible = true">举报</el-button>
    </div>
<el-dialog
:draggable="true"
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
    <el-container>
      <div class="hh">
        <button :style="{ backgroundImage: `url(${button[0].background})`, 
        backgroundColor: button[0].backgroundColor }" class="like_button" @click="like(post)"></button>
         <button :style="{ backgroundImage: `url(${button[1].background})`, 
        backgroundColor: button[1].backgroundColor }" class="like_button" @click="reply(post)"></button>
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
          <el-button type="danger" plain @click="cancel(post)">取消</el-button>
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


</style>