<!-- 论坛的发布帖子页面 -->
<template>
  <div class="container">
    <el-header>
      <button :style="{ backgroundImage: `url(${buttons[1].background})`, 
      backgroundColor: buttons[1].backgroundColor }" @click="buttonClick(buttons[1])" class="back_button" 
      @mouseover="changeButtonColor(buttons[1], true)" @mouseout="changeButtonColor(buttons[1], false)"></button>
      发布新帖
    </el-header>
    <el-input
type="text"
placeholder="请输入标题"
v-model="inputTitle"
maxlength="10"
show-word-limit
></el-input>
<div class="spacer"></div> <!-- 添加一个间隔元素 -->
  <el-input
type="textarea"
:autosize="{ minRows: 10, maxRows: 24}"
placeholder="请输入正文"
v-model="inputContent"
class="input_content">
</el-input>
</div>
<el-upload
class="upload"
action="https://jsonplaceholder.typicode.com/posts/"
:on-preview="handlePreview"
:on-remove="handleRemove"
list-type="picture">
<button :style="{ backgroundImage: `url(${buttons[0].background})`, 
      backgroundColor: buttons[0].backgroundColor }" @click="buttonClick(buttons[0])" class="upload_button" 
      @mouseover="changeButtonColor(buttons[0], true)" @mouseout="changeButtonColor(buttons[0], false)"></button>
<div slot="tip" class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
</el-upload>
</template>

<script setup lang="ts">
import { ref, watch,computed ,reactive} from 'vue';
import { ElTable, ElTableColumn, ElPagination, ElButton, ElMessage ,ElInput} from 'element-plus';
import 'element-plus/dist/index.css';
import router from '@/router';



const inputTitle = ref('');
const inputContent = ref('');
const inputHeight = ref(30);

const buttons = reactive([
  { id: 1, text: 'Button 1', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
  { id: 2, text: 'Button 2', background: 'src/assets/czw/back.svg', backgroundColor: 'transparent' },
  { id: 3, text: 'Button 3', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
  { id: 4, text: 'Button 4', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
  { id: 5, text: 'Button 5', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
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
  if(button.id==2){
    router.push('/forum'); // 跳转回 /forum 页面
    //处理返回
     //处理返回
      //处理返回
       //处理返回
        //处理返回
         //处理返回到ForunView
         
  }
};

function handleRemove(file, fileList) {
        console.log(file, fileList);
      };
function handlePreview(file) {
        console.log(file);
      }
</script>


<style scoped>

.container{
  margin-right: 25vh;
  margin-left: 25vh;
}
.upload_button {
  width: 5vh;
  height:5vh;
  /* 按钮样式 */
  border: none;
  cursor: pointer;
  /* 其他样式 */
  background-repeat: no-repeat;
  background-size: cover;
  transition: background-color 0.3s ease; /* 添加过渡效果 */
  background-size: 100% 100%; /* 调整背景图像的尺寸 */
}
.spacer {
  height: 10px; /* 设置间隔的高度 */
}
.upload{
  margin-right: auto;
  margin-left: 25vh;
}
.el-header {
    background-color: #B3C0D1;
    color: #333;
    line-height: 60px;
    font-size: 5vh;
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
