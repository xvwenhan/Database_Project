<!-- 商家页面组件 -->
  <template>
    <div class="CommodityShow">
      <div class="SearchContainer">
        <el-input v-model="searchName" placeholder="请输入订单ID" style="display: inline-block;"></el-input>
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
  
        <!-- :row-style="{ borderBottom: '1px solid #303a4f' }" :cell-style="{ borderRight: '1px solid #303a4f' }" -->
      </div>
      <div class="TableContainer">
      <el-table :data="products" class="CommodityTable" height="720">
        <el-table-column type="index"  />
        <el-table-column prop="id" label="订单ID"></el-table-column>
        <el-table-column prop="name" label="创建时间" ></el-table-column>
        <el-table-column prop="category" label="订单状态" ></el-table-column>
        <el-table-column prop="price" label="订单金额" ></el-table-column>
        <el-table-column prop="price" label="买家实付金额" ></el-table-column>
        <el-table-column label="操作">
          <template #default="scope">
            <el-button-group>
              <el-button size="mini" type="primary" icon="check" @click="handleCheck(scope.row)">查看</el-button>
            </el-button-group>
          </template>
        </el-table-column>
      </el-table>
    </div>
    
    <el-dialog title="商品详情" :visible.sync="dialogVisible">
        <div v-if="currentProduct">
          <p>商品ID: {{ currentProduct.id }}</p>
          <p>商品名称: {{ currentProduct.name }}</p>
          <p>商品分类: {{ currentProduct.category }}</p>
          <p>商品价格: {{ currentProduct.price }}</p>
          <p>是否出售: {{ currentProduct.isOnSale ? '是' : '否' }}</p>
          <!-- <p>是否出售: {{ currentProduct.description  }}</p>
          <p>是否出售: {{ currentProduct.image }}</p> -->
        </div>
        <template #footer>
          <el-button @click="dialogVisible = false">关闭</el-button>
        </template>
      </el-dialog>
  
      <!-- <div id="BottomButton">
        <el-button size="mini" type="primary" icon="Edit" @click="handleEdit(row)">添加商品</el-button>
        <el-button size="mini" type="danger" icon="Delete" @click="handleDelete(row)">批量删除</el-button>
      </div> -->
    </div>
  </template>
  
  <script >
  import { ref } from 'vue';
  import imageA from '@/assets/setting.svg';
  import { ElSelect, ElOption } from 'element-plus';
  import 'element-plus/dist/index.css';
  export default{
    components: {
      ElSelect,
      ElOption,
    },
    setup() {
      const value = ref('');
      
      const options = [
        { value: 'Option1', label: 'Option1' },
        { value: 'Option2', label: 'Option2' },
        { value: 'Option3', label: 'Option3' },
        { value: 'Option4', label: 'Option4' },
        { value: 'Option5', label: 'Option5' }
      ];
  
      const customIcon = 'el-icon-arrow-up';
  
      const products = ref([
        {
          id: '001',
          name: '商品A',
          category: '分类1',
          price: 100,
          isOnSale: true,
          description: '这是商品A的描述',
          image: imageA
        },
        {
          id: '002',
          name: '商品A',
          category: '分类1',
          price: 100,
          isOnSale: true,
          description: '这是商品A的描述',
          image: imageA
        },
        {
          id: '003',
          name: '商品A',
          category: '分类1',
          price: 100,
          isOnSale: true,
          description: '这是商品A的描述',
          image: imageA
        },
        {
          id: '004',
          name: '商品B',
          category: '分类2',
          price: 200,
          isOnSale: false,
          description: '这是商品B的描述',
          image: imageA
        },
        {
          id: '005',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '006',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '007',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '008',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '009',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '010',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '011',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '012',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '013',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '014',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '015',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '016',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '016',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '017',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '018',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '019',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
        {
          id: '020',
          name: '商品C',
          category: '分类3',
          price: 300,
          isOnSale: true,
          description: '这是商品C的描述',
          image: imageA
        },
      ]);
  
      const dialogVisible = ref(false);
      const currentProduct = ref(null);
  
      function handleCheck(row) {
        currentProduct.value = row;
        dialogVisible.value = true;
      }
  
      return {
        value,
        options,
        products,
        dialogVisible,
        currentProduct,
        handleCheck
      };
    }
  }
  </script>
  
  <style scoped>
  .CommodityShow {
    width: 86%;
    height: 88vh;
    position: fixed;
    top: 10.5vh;
    background-color: rgb(164, 197, 181);
  }
  
  .TableContainer {
    /* border: 1px solid #303a4f; */
    margin: 10px;
  }
  
  /* .CommodityTable {
    background-color: rgb(85, 124, 105);
    border: 1px solid #303a4f;
  } */
  
  .SearchContainer {
    margin-left: 990px;
    display: flex; 
    align-items: center;
    height: 50px;
    margin-right: 10px;
    /* background-color: rgb(164, 197, 181); */
  }
  
  #BottomButton {
    margin-left: 10px;
  }
  
  </style>