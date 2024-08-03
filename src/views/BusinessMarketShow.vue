  <template>
    <div class="CommodityShow">
      <div class="SearchContainer">
        <el-input v-model="searchTopic" placeholder="请输入市集主题" style="display: inline-block;"></el-input>
        <el-button type="primary" @click="filterProducts">搜索</el-button>
        <!-- <el-select v-model="value" placeholder="根据折扣要求筛选" style="display: inline-block;">
        <el-option
          v-for="item in options"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        />
       </el-select>
        <el-button type="primary" @click="filterProducts">筛选</el-button> -->

      </div>
      <div class="TableContainer">
      <el-table :data="currentPageData" class="CommodityTable" height="760">
        <el-table-column type="index"  />
        <el-table-column prop="id" label="市集ID"></el-table-column>
        <el-table-column prop="name" label="市集主题" ></el-table-column>
        <el-table-column prop="begin" label="开始时间" ></el-table-column>
        <el-table-column prop="end" label="结束时间" ></el-table-column>
        <!-- <el-table-column prop="request" label="折扣要求" ></el-table-column> -->
        <el-table-column label="操作">
          <template #default="scope">
            <el-button-group>
              <el-button size="mini" type="primary" icon="check" @click="handleCheck(scope.row)">查看</el-button>
              <!-- <el-button size="mini" type="primary" icon="check" >未参与</el-button> -->
              <el-button
               size="mini"
               :type="scope.row.isParticipated ? 'success' : 'primary'"
               :icon="scope.row.isParticipated ? 'check' : 'circle'"
               @click="toggleParticipation(scope.row)"
               >
                {{ scope.row.isParticipated ? '已参与' : '未参与' }}
              </el-button>
              <!-- 全部商品？ -->
            </el-button-group>
          </template>
        </el-table-column>
      </el-table>
    </div>

      <!-- 查看 -->
      <div v-if="dialogVisible" class="SettingPopUp">
      <div v-if="currentProduct" class="SettingContent">
        <span class="close" @click="dialogVisible = false">&times;</span>
        <!-- <p>商品ID: {{ currentProduct.id }}</p>
        <p>商品名称: {{ currentProduct.name }}</p>
        <p>商品分类: {{ currentProduct.category }}</p>
        <p>商品价格: {{ currentProduct.price }}</p>
        <p>是否出售: {{ currentProduct.isOnSale ? '是' : '否' }}</p>
        <p>商品具体描述: {{ currentProduct.description}}</p>
        <p>商品图片:
          <img :src="currentProduct.image" alt="ProductImage">
        </p> -->
        </div>
      </div>  

      <!-- 翻页 -->
    <div class="paginationContainer">
      <el-pagination
        v-model:current-page="currentPage"
        :page-size="pageSize"
        :total="totalProducts"
        layout="total, prev, pager, next, jumper"
        @current-change="handlePageChange">
      </el-pagination>
   </div>
  
    </div>
  </template>
  
  <script >
  import { ref,computed} from 'vue';
  import imageA from '@/assets/setting.svg';
  import { ElSelect, ElOption, ElButton } from 'element-plus';
  import 'element-plus/dist/index.css';
  export default{
    components: {
      ElSelect,
      ElOption, 
      ElButton
    },
    setup() {
      const value = ref('');
      const dialogVisible = ref(false);
      const currentProduct = ref(null);
      const searchTopic = ref('');
    
      const products = ref([
        {
          id: '001',
          name: '市集A',
          begin: '10点',
          end: '12点',
          description: '这是市集A的描述',
          image: imageA
        },
      ]);
  
      function handleCheck(row) {
        currentProduct.value = row;
        dialogVisible.value = true;
      }

      // const isParticipated = ref(false); // 初始状态为未参与
      // 根据状态设置按钮的类型、图标和文本
      // const buttonType = computed(() => (isParticipated.value ? 'success' : 'primary'));
      // const buttonIcon = computed(() => (isParticipated.value ? 'check' : 'circle'));
      // const buttonText = computed(() => (isParticipated.value ? '已参与' : '未参与'));
      // 切换参与状态
      const toggleParticipation = (row) => {
        row.isParticipated = !row.isParticipated; // 切换按钮状态
      };

      // 翻页
        const pageSize = 20;
        const currentPage = ref(1);
        const totalProducts = computed(() => products.value.length);
        const currentPageData = computed(() => {
        const start = (currentPage.value - 1) * pageSize;
        const end = Math.min(start + pageSize, totalProducts.value);
        return products.value.slice(start, end);
        });
        const handlePageChange = (page) => {
          currentPage.value = page;
        };
  
      return {
        pageSize,
        currentPage,
        totalProducts,
        currentPageData,
        handlePageChange,
        value,
        products,
        dialogVisible,
        currentProduct,
        handleCheck,
        // isParticipated,
        toggleParticipation,
        searchTopic
        // buttonType,
        // buttonIcon,
        // buttonText
      };
    }
  }
  </script>
  
<style scoped>
  .CommodityShow {
    width: 86%;
    height: 88.5vh;
    position: fixed;
    top: 10.5vh;
    background-color: rgb(164, 197, 181);
  }
  
  .TableContainer {
    margin: 10px;
    margin-top: 0; 
  }
  
  .SearchContainer {
    display: flex; 
    text-align: end;
    align-items: center;
    height: 60px;
    margin-right: 10px;
    /* background-color: rgb(164, 197, 181); */
  }
  
  .SettingPopUp {
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0,0,0,0.4);
  display: flex;
  justify-content: center;
  align-items: center;
}
.paginationContainer {
  display: inline-block;
}

.SettingContent {
  color: #065f43;
  background-color: #fefefe;
  display: inline-block;
  padding:5vh;
}

.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}
 
.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}
  
  </style>