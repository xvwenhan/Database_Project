<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, watch,computed ,reactive} from 'vue';


// const router = createRouter({
//   history: createWebHistory(import.meta.env.BASE_URL),
//   routes: [
//     {
//       path: '/',
//       name: 'Release',
//       component: Release
//     }
//   ]
// })

const inputTitle = ref('');
const inputContent = ref('');
const inputHeight = ref(30);

const adjustHeightContent = () => {
  const textarea = document.querySelector('.input_content');
  textarea.style.height = `${textarea.scrollHeight}px`;
  inputHeight.value = textarea.scrollHeight;
};
const saveTitle = () => {
  // 保存逻辑
};

const saveContent = () => {
  // 保存内容逻辑
};

const inputTitleCounter = computed(() => {
  return `${inputTitle.value.length}/30`;
});

const inputContentCounter = computed(() => {
  return `${inputContent.value.length}`;
});

watch(inputContent, (newVal) => {
  if (newVal === '') {
    const textarea = document.querySelector('.input_content');
    textarea.style.height = '300px'; // 将高度重置为最小高度
    inputHeight.value = 300;
  }
});

const buttons = reactive([
  { id: 1, text: 'Button 1', background: 'src/assets/picture+.svg', backgroundColor: 'transparent' },
  { id: 2, text: 'Button 2', background: 'src/assets/picture+.svg', backgroundColor: 'transparent' },
  { id: 3, text: 'Button 3', background: 'src/assets/picture+.svg', backgroundColor: 'transparent' },
  { id: 4, text: 'Button 4', background: 'src/assets/picture+.svg', backgroundColor: 'transparent' },
  { id: 5, text: 'Button 5', background: 'src/assets/picture+.svg', backgroundColor: 'transparent' },
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
  <Navbar />
    <div class="title_container">
     <textarea v-model="inputTitle" placeholder="请输入标题"  class="input_title"  @blur="saveTitle" maxlength="30"></textarea>
    </div>
    <span class="input_title_counter">{{ inputTitle.length }}/30</span>
    <div>
      <textarea v-model="inputContent" placeholder="请输入内容" :style="{ height: inputContent + 'px' }" class="input_content" @input="adjustHeightContent" @blur="saveContent"></textarea>
    </div>
    <span class="input_content_counter">{{ inputContent.length }}</span>
    <div>
      <button v-for="button in buttons" :key="button.id" :style="{ backgroundImage: `url(${button.background})`, 
        backgroundColor: button.backgroundColor }" @click="buttonClick(button)" class="release_button" 
        @mouseover="changeButtonColor(button, true)" @mouseout="changeButtonColor(button, false)"></button>
    </div>
</template>

<style scoped>

.input_title{
  width: 800px;       /* 设置输入框宽度 */
  height: 30px;   /* 设置输入框最小高度 */
  border: 1px solid #413f3f;
  padding: 5px;
  margin-top: 10px;
  font-size: 22px;
  overflow: hidden; 
  resize: none; 
}
.input_title_counter {
    right: 10px;
    bottom: 10px;
    font-size: 12px;
    color: #494a4a;
    text-align: right;
    pointer-events: none;
    margin-right: 425px;
   }

.input_content{
  width: 800px;       /* 设置输入框宽度 */
  min-height: 300px;   /* 设置输入框最小高度 */
  border: 1px solid #413f3f;
  padding: 5px;
  resize: none;   /* 允许垂直调整输入框的大小 */
  font-size: 20px;
  overflow: hidden; 
}
.input_content_counter {
    right: 10px;
    bottom: 10px;
    font-size: 12px;
    color: #777777;
    text-align: right;
    pointer-events: none;
    margin-right: 425px;
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

</style>