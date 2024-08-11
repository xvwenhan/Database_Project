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
placeholder="请输入标题(不能为空)"
v-model="inputTitle"
maxlength="10"
show-word-limit
></el-input>
<div class="spacer"></div> <!-- 添加一个间隔元素 -->
  <el-input
type="textarea"
:autosize="{ minRows: 10, maxRows: 24}"
placeholder="请输入正文(不能为空)"
v-model="inputContent"
class="input_content">
</el-input>
</div>
<!--
<el-upload
  class="upload"
  action="https://localhost:7262/api//Post/create_post"
  :on-preview="handlePreview"
  :on-remove="handleRemove"
  list-type="picture">
  <button :style="{ 
    backgroundImage: `url(${buttons[0].background})`, 
    backgroundColor: buttons[0].backgroundColor 
  }" @click="buttonClick(buttons[0])" class="upload_button" 
  @mouseover="changeButtonColor(buttons[0], true)" @mouseout="changeButtonColor(buttons[0], false)"></button>
  <div slot="tip" class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
</el-upload>
-->
<div>
    <input type="file" @change="handleFileUpload" multiple>
    <button :style="{ backgroundImage: `url(${buttons[2].background})`, 
      backgroundColor: buttons[2].backgroundColor }" @click="confirm()" class="upload_button" 
      @mouseover="changeButtonColor(buttons[2], true)" @mouseout="changeButtonColor(buttons[2], false)"></button>
    <div class="image-container">
      <div v-for="(file, index) in files" :key="index" >
        <img :src="getFileUrl(file)" class="image-preview">
      </div>
    </div>
  </div>
</template>



<script setup lang="ts">
import { ref, reactive} from 'vue';
import { ElInput} from 'element-plus';
import 'element-plus/dist/index.css';
import router from '@/router';
import axiosInstance from '../components/axios';


const inputTitle = ref('');
const inputContent = ref('');

const buttons = reactive([
  { id: 1, text: 'Button 1', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
  { id: 2, text: 'Button 2', background: 'src/assets/czw/back.svg', backgroundColor: 'transparent' },
  { id: 3, text: 'Button 2', background: 'src/assets/czw/back.svg', backgroundColor: 'transparent' },
]);


const files = ref([]);

const handleFileUpload = (event) => {
  const selectedFiles = event.target.files;
  for (let i = 0; i < selectedFiles.length; i++) {
    files.value.push(selectedFiles[i]);
  }
};

const getFileUrl = (file) => {
  return URL.createObjectURL(file);
};

const uploadImages = () => {
  console.log('上传图片:', files.value);
};
const changeButtonColor = (button, isHovered) => {
  if (isHovered) {
    if(button.id==1)
    button.backgroundColor = '#87c2a5'; // 设置鼠标悬停时的背景颜色
    else if(button.id==2||button.id==3)
    button.backgroundColor = '#5169e6'; 
  } else {
    button.backgroundColor = 'transparent'; // 恢复背景颜色为透明
  }
};

const buttonClick = (button) => {
  if(button.id==2){
    router.push('/forum'); // 跳转回 /forum 页面
  }
};

const confirm = async () => {
  const formData = new FormData();
      formData.append('PostTitle', inputTitle.value);
      formData.append('PostContent', inputContent.value);

      files.value.forEach((file, index) => {
        formData.append('Images', file); // 直接以 'Images' 为键添加文件到 FormData
    });


      axiosInstance.post('/Post/create_post', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      }).then(response => {
        console.log(response.data);
      }).catch(error => {
        console.error(error);
      });
      console.log('上传图片:', files.value);

};

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
.image-preview {
  max-width: 200px;
  margin-top: 10px;
}
</style>
