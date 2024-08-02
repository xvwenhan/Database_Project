
  <template>
    <div class="CommodityShow">
      <div class="SearchContainer">
        <el-input v-model="searchOrder" placeholder="请输入订单ID" style="display: inline-block;"></el-input>
        <el-button type="primary" @click="filterProducts">搜索</el-button>
        <el-select v-model="value" placeholder="根据创建时间筛选" style="display: inline-block;">
        <el-option
          v-for="item in options"
          :key="item.value"
          :label="item.label" 
          :value="item.value"
        />
       </el-select>
        <el-button type="primary" @click="filterProducts">筛选</el-button>
      </div>
      
      <!-- 表格 -->
      <div class="TableContainer">
      <el-table :data="currentPageData" class="CommodityTable" height="760">
        <el-table-column type="index"  />
        <el-table-column prop="id" label="订单ID"width="150"></el-table-column>
        <el-table-column prop="creationtime" label="创建时间" width="150"></el-table-column>
        <el-table-column prop="orderstatus" label="订单状态" width="150"></el-table-column>
        <el-table-column prop="orderprice" label="订单金额"width="150"></el-table-column>
        <el-table-column prop="practicalprice" label="买家实付金额" width="150"></el-table-column>
        <el-table-column label="买家是否申请退货">
          <template #default="scope">
            <span>{{ scope.row.returnRequested ? '是' : '否' }}</span>
            <span class="space"></span>
            <el-button
              size="mini"
              :type="scope.row.returnStatus === '待同意' ? 'success' : 'primary'"
              :disabled="scope.row.returnRequested === false"
              @click="handleReturnRequest(scope.row)"
            >
              {{ scope.row.returnStatus || '待同意' }}
            </el-button>
          </template>
        </el-table-column>
        <el-table-column label="操作">
          <template #default="scope">
            <el-button-group>
              <el-button size="mini" type="primary" icon="check" @click="handleCheck(scope.row)">买家评价</el-button>
              <el-button size="mini" type="primary" icon="check" @click="handleCheckTwo(scope.row)">快递</el-button>
            </el-button-group>
          </template>
        </el-table-column>
      </el-table>
      </div>

     <!-- 买家评价 -->
    <div v-if="dialogVisible" class="SettingPopUp">
      <div v-if="currentProduct" class="SettingContent">
        <span class="close" @click="dialogVisible = false">&times;</span>
        <p>买家评分</p>
        <p>评价内容</p>
      </div>
    </div>   
    <!-- 快递单号 -->
    <div v-if="dialogVisibleTwo" class="SettingPopUp">
      <div v-if="currentProduct" class="SettingContent">
        <span class="close" @click="dialogVisibleTwo = false">&times;</span>
        <p>快递单号</p>
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
  
  <script>
  import { ref,computed } from 'vue';
  import { ElSelect, ElOption } from 'element-plus';
  import 'element-plus/dist/index.css';
  export default{
    components: {
      ElSelect,
      ElOption,
    },
    setup() {
      const value = ref('');
      const dialogVisible = ref(false);
      const dialogVisibleTwo = ref(false);
      const currentProduct = ref(null);

//       const now = new Date(); // 当前日期和时间
// const specificDate = new Date('2024-07-25T10:00:00'); // 指定日期和时间
      
      const options = [
        { value: 'Option1', label: 'Option1' },
        { value: 'Option2', label: 'Option2' },
        { value: 'Option3', label: 'Option3' },
        { value: 'Option4', label: 'Option4' },
        { value: 'Option5', label: 'Option5' }
      ];
  
      const products = ref([
        {
          id: '001',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '002',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '003',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:false,
        },
        {
          id: '004',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '005',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '006',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '007',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '008',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '009',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '010',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '011',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '012',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '013',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '014',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '015',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '016',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '017',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '018',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '019',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '020',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
        {
          id: '021',
          creationtime: '10点',
          orderstatus: '待发货',
          orderprice: 100,
          practicalprice: 100,
          returnRequested:true,
        },
      ]);
  
      function handleCheck(row) {
        currentProduct.value = row;
        dialogVisible.value = true;
      }
      function handleCheckTwo(row) {
        currentProduct.value = row;
        dialogVisibleTwo.value = true;
      }
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

         // 根据 returnRequested 自动设置 returnStatus
        const updatedData = computed(() => {
          return currentPageData.value.map(item => ({
            ...item,
            returnStatus: item.returnRequested ? '待同意' : ''
          }));
        });
          // 处理退货申请按钮点击事件
          const handleReturnRequest = (row) => {
            if (row.returnRequested && row.returnStatus === '待同意') {
              row.returnStatus = '已同意';
            }
          };

          const searchOrder=ref('');
  
      return {
        currentPageData: updatedData,
        handleReturnRequest,
        pageSize,
        currentPage,
        totalProducts,
        handlePageChange,
        value,
        options,
        products,
        currentProduct,
        dialogVisible,
        dialogVisibleTwo,
        handleCheck,
        handleCheckTwo,
        searchOrder
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
    margin-left: 990px;
    display: flex; 
    align-items: center;
    height: 60px;
    margin-right: 10px;
    /* background-color: rgb(164, 197, 181); */
  }

  .paginationContainer {
  display: inline-block;
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

.space {
  margin-left: 20px;
}
  </style>