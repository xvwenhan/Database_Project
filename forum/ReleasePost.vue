<script setup lang="ts">
import { ref, watch,computed ,reactive} from 'vue';

const inputTitle = ref('');
const inputContent = ref('');
const inputHeight = ref(30);

const buttons = reactive([
  { id: 1, text: 'Button 1', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
  { id: 2, text: 'Button 2', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
  { id: 3, text: 'Button 3', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
  { id: 4, text: 'Button 4', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
  { id: 5, text: 'Button 5', background: 'src/assets/czw/picture+.svg', backgroundColor: 'transparent' },
]);

const changeButtonColor = (button, isHovered) => {
  if (isHovered) {
    button.backgroundColor = '#87c2a5'; // 设置鼠标悬停时的背景颜色
  } else {
    button.backgroundColor = 'transparent'; // 恢复背景颜色为透明
  }
};

const buttonClick = (button) => {
  // 处理按钮点击事件
  console.log(`Button ${button.id} clicked`);
};
</script>

<template>
    <div class="container">
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
  :autosize="{ minRows: 2, maxRows: 4}"
  placeholder="请输入正文"
  v-model="inputContent"
  class="input_content">
</el-input>
</div>
    <div>
      <button v-for="button in buttons" :key="button.id" :style="{ backgroundImage: `url(${button.background})`, 
        backgroundColor: button.backgroundColor }" @click="buttonClick(button)" class="release_button" 
        @mouseover="changeButtonColor(button, true)" @mouseout="changeButtonColor(button, false)"></button>
    </div>
</template>

<style>

.container{
  margin-right: 400px;
  margin-left: 400px;
}
.release_button {
  width: 50px;
  height: 50px;
  /* 按钮样式 */
  border: none;
  cursor: pointer;
  border-radius: 4px;
  /* 其他样式 */
  background-repeat: no-repeat;
  background-size: cover;
  margin-right: 20px;
  transition: background-color 0.3s ease; /* 添加过渡效果 */
  background-size: 100% 100%; /* 调整背景图像的尺寸 */
}
.spacer {
  height: 10px; /* 设置间隔的高度 */
}
</style>