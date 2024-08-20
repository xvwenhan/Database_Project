<template>
  <!-- 这里是商品管理界面的内容区域 -->
  <div class="CommodityShow">
    <!-- 搜索和筛选按钮 --> 
    <div class="SearchContainer">
      <el-input v-model="searchName" placeholder="请输入商品名称(在全部商品中搜索)" style="display: inline-block;"></el-input>
      <el-button type="primary" @click="filterProducts">搜索</el-button>
      <el-input v-model="searchcategoryInit" placeholder="请输入商家分类（在全部商品中搜索）" style="display: inline-block;"></el-input>
      <el-button type="primary" @click="filterProductsTag">筛选</el-button>
    </div>

    <!-- 表格 -->
  <div class="TableContainer">
    <el-table :data="currentPageData" class="CommodityTable" height="760"@selection-change="handleSelectionChange">
      <el-table-column type="selection" />
      <el-table-column prop="id" label="商品ID"></el-table-column>
      <el-table-column prop="name" label="商品名称" ></el-table-column>
      <el-table-column prop="categoryInit" label="商家分类" ></el-table-column>
      <el-table-column prop="price" label="商品价格" width="100"></el-table-column>
      <el-table-column prop="isOnSale" label="是否出售" width="100">
        <template #default="scope">
          <el-tag type="success" v-if="scope.row.isOnSale">是</el-tag>
          <el-tag type="info" v-else>否</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作">
        <template #default="scope">
          <el-button-group>
            <el-button size='small' type="primary" icon="check" @click="handleCheck(scope.row)">查看</el-button>
            <el-button size='small' type="primary" icon="Edit" @click="handleEdit(scope.row)">编辑</el-button>
            <el-button size='small' type="danger" icon="Delete" @click="handleDelete(scope.row)">删除</el-button>
          </el-button-group>
        </template>
      </el-table-column>
    </el-table>
  </div>

  <!-- 查看 -->
  <div v-if="dialogVisible" class="SettingPopUp">
      <div v-if="currentProduct" class="SettingContent">
        <span class="close" @click="dialogVisible = false">&times;</span>
        <p>商品ID: {{ currentProduct.id }}</p>
        <p>商品名称: {{ currentProduct.name }}</p>
        <p>系统分类: {{ currentProduct.categorySys }}</p>
        <p>商家分类: {{ currentProduct.categoryInit }}</p>
        <p>商品价格: {{ currentProduct.price }}</p>
        <p>是否出售: {{ currentProduct.isOnSale ? '是' : '否' }}</p>
        <p>商品具体描述: {{ currentProduct.description}}</p>
        <p>商品图片:
          <img :src="currentProduct.image" alt="ProductImage" style="width: 200px; height: 200px;">
        </p>
      </div>
  </div>   

  <!-- 编辑 -->
  <div v-if="dialogVisibleTwo" class="SettingPopUp">
      <div v-if="preProduct" class="SettingContent" >
        <el-form ref="editForm"  :rules="rules" :model="preProduct">
          <el-form-item label="商品名称" prop="name">
            <el-input v-model="preProduct.name"></el-input>
          </el-form-item>
          <el-form-item label="商品价格" prop="price">
            <el-input v-model.number="preProduct.price"></el-input>
          </el-form-item>
          <el-form-item label="系统分类" prop="categorySys">
              <el-select v-model="preProduct.categorySys">
                <el-option
                  v-for="category in options"
                  :key="category.value"
                  :label="category.value"
                  :value="category.value"
                ></el-option>
              </el-select>
            </el-form-item>
            <el-form-item label="商家分类" prop="categoryInit">
            <el-input v-model.number="preProduct.categoryInit"></el-input>
          </el-form-item>
          <!-- <el-form-item label="是否出售" prop="isOnSale">
            <el-select v-model="preProduct.isOnSale" > -->
              <!-- :placeholder="getPlaceholderText" -->
                <!-- <el-option
                  v-for="choice in OnOrNot"
                  :key="choice.value"
                  :label="choice.value"
                  :value="choice.value"
                ></el-option>
              </el-select>
          </el-form-item> -->
          <el-form-item label="商品具体描述" prop="description">
            <el-input v-model="preProduct.description"></el-input>
          </el-form-item>
          <el-form-item label="商品图片(.jpg)" prop="image">
            <img :src="preProduct.image" alt="当前图片" v-if="preProduct.image" style="width: 200px; height: 200px;" />
            <input type="file" @change="(e) => onFileChange(e, 'preProduct')">
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="onsubmit()">保存</el-button>
            <el-button @click="dialogVisibleTwo = false">取消</el-button>
          </el-form-item>
    </el-form>
  </div>
  </div>   

  <!-- <el-dialog title="商品详情" :visible.sync="dialogVisible">
      <div v-if="currentProduct" > 
        <p>商品ID: {{ currentProduct.id }}</p>
        <p>商品名称: {{ currentProduct.name }}</p>
        <p>商品分类: {{ currentProduct.category }}</p>
        <p>商品价格: {{ currentProduct.price }}</p>
        <p>是否出售: {{ currentProduct.isOnSale ? '是' : '否' }}</p>
      </div>
  </el-dialog>  -->

  <!-- 添加商品和批量删除按钮 -->
  <div id="BottomButton">
    <el-button size='small' type='primary' icon="Edit" @click="showAddDialog()">添加商品</el-button>
    <el-button size='small' type='danger' icon="Delete" @click="confirmBatchDelete()">批量删除</el-button>
  </div>

  <!-- 添加商品对话框 -->
  <div v-if="addDialogVisible" class="SettingPopUp">
    <div class="SettingContent">
      <span class="close" @click="addDialogVisible = false">&times;</span>
      <el-form :model="newProduct" :rules="rules" ref="form"> 
        <el-form-item label="商品名称" prop="name">
          <el-input v-model="newProduct.name"></el-input>
        </el-form-item>
        <el-form-item label="系统分类" prop="categorySys">
          <el-select v-model="newProduct.categorySys">
            <el-option-group
        v-for="group in options"
        :key="group.label"
        :label="group.label"
      >
        <el-option
          v-for="item in group.children"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        ></el-option>
      </el-option-group>
          </el-select>
        </el-form-item>
        <el-form-item label="商家分类" prop="categoryInit">
          <el-input v-model="newProduct.categoryInit"></el-input>
        </el-form-item>
        <el-form-item label="商品价格" prop="price">
          <el-input v-model.number="newProduct.price"></el-input>
        </el-form-item>
        <el-form-item label="商品图片(.jpg)" prop="image">
          <img :src="newProduct.image" alt="当前图片" v-if="newProduct.image" style="width: 200px; height: 200px;" />
          <input type="file" @change="(e) => onFileChange(e, 'newProduct')">
        </el-form-item>
        <el-form-item label="商品描述" prop="description">
          <el-input v-model="newProduct.description"></el-input>
        </el-form-item>

         <!-- 图片和文字的输入区域 -->
         <el-form-item label="图片和文字">
        <div v-for="(item, index) in newProduct.imagesWithText" :key="index" class="image-text-item">
          <img :src="item.image" alt="图片" style="width: 50px; height: 50px;" />
          <p>{{ item.text }}</p>
        </div>
        <input type="file" @change="handleFileChange($event, 'newImage')">
        <el-input v-model="newImageText" placeholder="输入图片描述"></el-input>
        <el-button @click="addImageWithText(newImage, newImageText)">添加图片和文字</el-button>
      </el-form-item>

      </el-form>
      <el-button type="primary" @click="addNewProduct">添加</el-button>
      <el-button type="primary" @click="addDialogVisible = false">取消</el-button>
    </div>
  </div>


  <!-- 确认批量删除对话框 -->
  <div v-if="confirmDialogVisible" class="SettingPopUp">
      <div class="SettingContent">
        <span class="close" @click="confirmDialogVisible = false">&times;</span>
        <span>您确定要删除选中的商品吗？此操作不可撤销。</span>
        <el-button type="primary" @click="deleteSelectedCommodities">确认</el-button>
        <el-button @click="confirmDialogVisible = false">取消</el-button>
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
import { ref, computed ,onMounted } from 'vue';
import axiosInstance from '../components/axios';

import { ElSelect, ElOption,ElMessageBox } from 'element-plus';
import { ElTable, ElTableColumn, ElButton, ElDialog, ElPagination,ElMessage  } from 'element-plus';

import 'element-plus/dist/index.css';

export default{
  components: {
    ElSelect,
    ElOption,
    ElTable, 
    ElTableColumn,
    ElButton, 
    ElDialog,
    ElPagination 
  },
  setup() {
    const searchName=ref('');
    const searchcategoryInit=ref('');
    const value = ref('');
    const dialogVisible = ref(false);
    const dialogVisibleTwo = ref(false);
    const preProduct = ref({});
    const currentProduct = ref({});
    const editForm = ref(null);
    const products=ref([]);
    const pageSize = 20;
    const currentPage = ref(1);
    const totalProducts = ref(0);
    const addDialogVisible = ref(false);
    const form = ref(null);
    const confirmDialogVisible = ref(false);
    const selectedProducts = ref([]);
    const viewType = ref(1);
  
    const fetchProducts = async () => {
  const storeId = localStorage.getItem('userId'); // 替换为实际的 storeId

  try {
  const response = await axiosInstance.get('/StoreViewProduct/GetProductsByStoreIdAndViewType', {
    params: {
      StoreId: storeId,
      ViewType: viewType.value
    },
  });

  console.log('Backend Response:', response.data);

  if (Array.isArray(response.data)) {
    const processedProducts = response.data.map(product => {
      console.log('Original Product Data:', product);

      // 确保字段存在并转换正确
      const image = product.productPic ? `data:image/jpeg;base64,${product.productPic}` : '';

      return {
        id: product.productId || 'N/A',
        name: product.productName || 'Unknown',
        categorySys: product.tag || 'Unknown',
        categoryInit: product.storeTag || 'Unknown',
        price: product.productPrice || 0,
        isOnSale: product.saleOrNot !== undefined ? product.saleOrNot : false,
        description: product.description || 'No description available',
        image: image,
      };
    });

    console.log('Processed Products:', processedProducts);
    products.value = processedProducts; // 确保 products 是 Vue 的响应式对象
    totalProducts.value = processedProducts.length;
  } else {
    console.error('Unexpected response format:', response.data);
  }
} catch (error) {
  console.error('获取商品数据失败:', error);
}
};

const fetchProductByName = async (keyword) => {
  const storeId = localStorage.getItem('userId'); // 替换为实际的 storeId
  try {
    const response = await axiosInstance.get('/StoreViewProduct/search', {
      params: {
        storeId: storeId,
        keyword: keyword || '',
      }
    });

    if (Array.isArray(response.data)) {
      const processedProducts = response.data.map(product => {
        const image = product.productPic ? `data:image/jpeg;base64,${product.productPic}` : '';

        return {
        id: product.productId || 'N/A',
        name: product.productName || 'Unknown',
        categorySys: product.tag || 'Unknown',
        categoryInit: product.storeTag || 'Unknown',
        price: product.productPrice || 0,
        isOnSale: product.saleOrNot !== undefined ? product.saleOrNot : false,
        description: product.description || 'No description available',
        image: image,
        };
      });

      products.value = processedProducts; 
    } else {
      console.error('Unexpected response format:', response.data);
    }
  } catch (error) {
    console.error('通过商品名称获取商品数据失败:', error);
  }
};

const fetchProductByTag = async (storeTag) => {
  const storeId = localStorage.getItem('userId'); // 替换为实际的 storeId
  try {
    const response = await axiosInstance.get('/StoreViewProduct/searchByStoreTag', {
      params: {
        storeId: storeId,
        storeTag: storeTag || '',
      }
    });

    if (Array.isArray(response.data)) {
      const processedProducts = response.data.map(product => {
        const image = product.productPic ? `data:image/jpeg;base64,${product.productPic}` : '';

        return {
        id: product.productId || 'N/A',
        name: product.productName || 'Unknown',
        categorySys: product.tag || 'Unknown',
        categoryInit: product.storeTag || 'Unknown',
        price: product.productPrice || 0,
        isOnSale: product.saleOrNot !== undefined ? product.saleOrNot : false,
        description: product.description || 'No description available',
        image: image,
        };
      });

      products.value = processedProducts; 
    } else {
      console.error('Unexpected response format:', response.data);
    }
  } catch (error) {
    console.error('通过商品名称获取商品数据失败:', error);
  }
};

    const filterProducts = () => {
      if (searchName.value.trim() !== '') {
        fetchProductByName(searchName.value.trim());
      } else {
        fetchProducts();
      }
    };

    const filterProductsTag = () => {
      if (searchcategoryInit.value.trim() !== '') {
        fetchProductByTag(searchcategoryInit.value.trim());
      } else {
        fetchProducts();
      }
    };

    const handleChange = (viewTypeValue) => {
      viewType.value = viewTypeValue;
      fetchProducts();
    };

    onMounted(() => {
      fetchProducts();
    });

    const options = [
  {
    label: '服装',
    children: [
      { value: '001', label: '汉族传统服饰' },
      { value: '002', label: '少数民族服饰' },
      { value: '003', label: '地方特色服装' }
    ]
  },
  {
    label: '首饰',
    children: [
      { value: '004', label: '银饰' },
      { value: '005', label: '玉饰' },
      { value: '006', label: '宝石首饰' },
      { value: '007', label: '民族特色首饰' }
    ]
  },
  {
    label: '家具',
    children: [
      { value: '008', label: '床榻类' },
      { value: '009', label: '桌案类' },
      { value: '010', label: '椅凳类' },
      { value: '011', label: '柜架类' },
      { value: '012', label: '屏风类' }
    ]
  },
  {
    label: '工艺品',
    children: [
      { value: '013', label: '陶瓷' },
      { value: '014', label: '漆器' },
      { value: '015', label: '刺绣' },
      { value: '016', label: '景泰蓝' }
    ]
  },
  {
    label: '小物件',
    children: [
      { value: '017', label: '文房四宝' },
      { value: '018', label: '剪纸艺术' },
      { value: '019', label: '竹编' }
    ]
  }
];
    const OnOrNot = [
      {value: true, label: '是'},
      {value: false, label: '否'}
    ] 

    // 查看
    const handleCheck = (item) => {
      // console.log("Clicked item:", item);
      currentProduct.value = item;
      // console.log(currentProduct.value);
      dialogVisible.value = true;
      // console.log("Dialog visible:", dialogVisible.value);
    };

    const rules = {
      name: [{ required: true, message: '请输入商品名称', trigger: 'blur' }],
      categorySys: [{ required: true, message: '请输入系统分类', trigger: 'blur' }],
      categoryInit: [{ required: true, message: '请输入商家分类', trigger: 'blur' }],
      price: [{ required: true, message: '请输入商品价格', trigger: 'blur' } ,
      { type: 'number', message: '价格必须为数字', trigger: 'blur' },
      { validator: (rule, value, callback) => {
            if (value === null || value === undefined || value < 0) {
              callback(new Error('价格必须大于或等于0'));
            } else {
              callback();
            }
          }, trigger: 'blur' }],
      isOnSale: [{ required: true, message: '请选择是否出售', trigger: 'change' }],
      description: [{ required: true, message: '请输入商品描述', trigger: 'blur' }],
      image: [{ required: true, message: '请上传商品图片', trigger: 'change' }]
    };

    const onFileChange = (event, target) => {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          const base64Image = `data:image/jpeg;base64,${e.target.result.split(',')[1]}`;
          if (target === 'newProduct') { 
            newProduct.value.image = base64Image;
          } else if (target === 'preProduct') {
            preProduct.value.image = base64Image;
          }
        };
        reader.readAsDataURL(file);
      }
    };
   
    // 编辑
    const handleEdit = (item) => {
      if (row.isOnSale) {
        ElMessage({
          message: '该商品已经出售，无法编辑',
          type: 'warning'
        });
        return;
      }
      currentProduct.value = item;
      preProduct.value = {
        id: item.id,
        name: item.name,
        categorySys: item.categorySys,
        categoryInit: item.categoryInit,
        price: item.price,
        // isOnSale: item.isOnSale,
        description: item.description,
        image:item.image 
      };
      dialogVisibleTwo.value = true;
    };

    const onsubmit = async () => {
    editForm.value.validate(async (valid) => {
        if (valid) {
          console.log('preProduct.image:', preProduct.value.image);
            // 确保 productPic 是有效的 Base64 字符串或处理空情况
            const productPic = preProduct.value.image ? preProduct.value.image.split(',')[1] : '';

            const updatedProduct = {
                productId: preProduct.value.id,
                productName: preProduct.value.name,
                productPrice: preProduct.value.price,
                storeTag: preProduct.value.categoryInit,
                tag: preProduct.value.categorySys,
                description: preProduct.value.description,
                productPic: productPic
            };
            const userId = localStorage.getItem('userId'); 
            try {
                // 发送请求到后端，storeId 作为查询参数传递
                const response = await axiosInstance.put('/StoreViewProduct/editProduct', updatedProduct, {
                    params: {
                        storeId: userId // 替换为实际的 storeId
                    }
                });

                // 处理响应
                if (response.status === 200) {
                dialogVisibleTwo.value = false;
                currentProduct.value=preProduct.value;
                fetchProducts();
                ElMessage({
                    message: '商品信息已更新',
                    type: 'success'
                });
            } else {
                ElMessage({
                    message: '更新商品信息失败',
                    type: 'error'
                });
            }
            } catch (error) {
                console.error('更新商品信息失败:', error.response ? error.response.data : error.message);
                ElMessage({
                    message: '更新商品信息失败',
                    type: 'error'
                });
            }
        } else {
            ElMessage({
                message: '请填写正确',
                type: 'warning'
            });
        }
    });
};
    // 翻页
    const currentPageData = computed(() => {
    const start = (currentPage.value - 1) * pageSize;
    const end = Math.min(start + pageSize, totalProducts.value);
    return products.value.slice(start, end);
    });
    const handlePageChange = (page) => {
      currentPage.value = page;
    };

    //删除单个
    const handleDelete = async (row) => {
  if (row.isOnSale) {
    ElMessage({
      message: '该商品已经出售，无法删除',
      type: 'warning'
    });
    return;
  }

  try {
    await ElMessageBox.confirm('此操作将永久删除该商品, 是否继续?', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    });

    const storeId = localStorage.getItem('userId');; // 替换为实际的storeId
    const productId = row.id;

    // 发送请求到后端删除单个商品
    const response = await axiosInstance.delete('/StoreViewProduct/deleteProducts', {
      params: {
        storeId: storeId
      },
      data: [productId] // 传递请求体为数组
    });

    if (response.status === 200) {
      // 删除本地商品列表中的商品
      const index = products.value.indexOf(row);
      if (index !== -1) {
        products.value.splice(index, 1); // 删除指定索引的数据
        handlePageChange(currentPage.value);
        if (currentPage.value > 1 && currentPageData.value.length === 0) {
          currentPage.value--;
        }
      }

      ElMessage({
        message: '商品已删除',
        type: 'success'
      });
    } else {
      ElMessage({
        message: '删除商品失败',
        type: 'error'
      });
    }
  } catch (error) {
    if (error !== 'cancel') {
      console.error('删除商品失败:', error.response ? error.response.data : error.message);
      ElMessage({
        message: '删除商品失败: ' + error.message,
        type: 'error'
      });
    }
  }
};


    // 添加商品
    const newProduct = ref({
      name: '',
      categorySys: '',
      categoryInit: '',
      price: null,
      description: '',
      image: '',
      imagesWithText: []
    });
    const newImage = ref('');
const newImageText = ref('');
    const showAddDialog = () => {
      newProduct.value = {
        name: '',
        categorySys: '',
        categoryInit: '',
        price: null,
        description: '',
        image: ''
      };
      addDialogVisible.value = true;
    };
    const handleFileChange = (event) => {
  const file = event.target.files[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = (e) => {
      newImage.value = e.target.result; // 获取完整的 Base64 字符串
    };
    reader.readAsDataURL(file);
  }
};

const addImageWithText = () => {
  console.log('newImage:', newImage.value);
  console.log('newImageText:', newImageText.value);
  console.log('imagesWithText:', newProduct.value.imagesWithText);

  if (newImage.value && newImageText.value) {
    if (!Array.isArray(newProduct.value.imagesWithText)) {
      newProduct.value.imagesWithText = [];
    }
    newProduct.value.imagesWithText.push({ image: newImage.value, text: newImageText.value });
    newImage.value = ''; // 清空 newImage
    newImageText.value = ''; // 清空 newImageText
  }
};
    const addNewProduct = async () => {
  form.value.validate(async (valid) => {
    if (valid) {
      // 确保 productPic 是有效的 Base64 字符串或处理空情况
      const productPic = newProduct.value.image ? newProduct.value.image.split(',')[1] : '';
      // const newProductData = {
      //   productName: newProduct.value.name,
      //   productPrice: newProduct.value.price,
      //   Tag: newProduct.value.categoryInit,
      //   Description: newProduct.value.description,
      //   StoreTag: newProduct.value.categorySys,
      //   ProductImages: productPic
      // };
       // 使用 FormData 处理文件上传
      // 使用 URLSearchParams 处理数据
      const params = new URLSearchParams();
      params.append('ProductName', newProduct.value.name || '');
      params.append('ProductPrice', newProduct.value.price || '');
      params.append('Tag', newProduct.value.categoryInit || '');
      params.append('Description', newProduct.value.description || '');
      params.append('StoreTag', newProduct.value.categorySys || '');
      params.append('ProductImages', productPic ? `data:image/jpeg;base64,${productPic}` : '');
      
      // 使用 Promise.all 处理所有文件
      const promises = newProduct.value.imagesWithText.map((item, index) => {
        return new Promise((resolve, reject) => {
          // 确保 item.image 是 File 对象
          console.log(item.image); // 确认 item.image 的实际内容
          console.log(item.image instanceof File); // 确认 item.image 是否是 File 对象
          if (item.image && item.image instanceof File) {
            const reader = new FileReader();
            reader.onloadend = function () {
              const base64String = reader.result.split(',')[1]; // 获取 Base64 字符串部分
              params.append(`PicDes[${index}].DetailPic`, base64String);
              params.append(`PicDes[${index}].Description`, item.text);
              resolve(); // 处理完成
            };
            reader.onerror = function () {
              reject(new Error('Failed to read file'));
            };
            reader.readAsDataURL(item.image); // 读取文件并触发 onloadend
          } else {
            // 处理不符合预期的 item.image
            reject(new Error('item.image is not a valid File object'));
          }
        });
      });

      // 检查 URLSearchParams 内容
      for (const [key, value] of params.entries()) {
        console.log(`${key}: ${value}`);
      }

      const storeId = localStorage.getItem('userId');// 替换为实际的storeId
      if (!storeId) {
        ElMessage({ message: '未找到有效的 storeId', type: 'error' });
        return;
      }
      console.log('请求 URL:', `/StoreViewProduct/addProduct?storeId=${storeId}`);

      try {
        // 发送请求到后端，storeId 作为查询参数传递
        // const response = await axiosInstance.post(`/StoreViewProduct/addProduct`, newProductData, {
        //   params: {
        //     storeId: storeId
        //   }
        // });

        const response = await axiosInstance.post(`/StoreViewProduct/addProduct?storeId=${storeId}`, params, {
          headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
          params: { storeId: storeId } // 将 storeId 作为查询参数传递
        });

        // const response = await axiosInstance.post(`/StoreViewProduct/addProduct`, params, {
        //   headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        //   params: { storeId: storeId } // 将 storeId 作为查询参数传递
        // });

        if (response.status === 200) {
          fetchProducts();
          ElMessage({
            message: '商品已添加',
            type: 'success'
          });
          Object.assign(newProduct.value, {
            name: '',
            categorySys: '',
            categoryInit: '',
            price: null,
            isOnSale: null,
            description: '',
            image: '',
            imagesWithText:[]
          });
          addDialogVisible.value = false;
        } else {
          ElMessage({
            message: '添加商品失败',
            type: 'error'
          });
        }
      } catch (error) {
        console.error('添加商品失败:', error.response ? error.response.data : error.message);
        ElMessage({
          message: '添加商品失败: ' + error.message,
          type: 'error'
        });
      }
    } else {
      ElMessage({
        message: '请填写完整',
        type: 'warning'
      });
    }
  });
};

//删除多个
const handleSelectionChange = (selection) => {
  selectedProducts.value = selection;
};
const confirmBatchDelete = () => {
  if (selectedProducts.value.length > 0) {
    const notAllowedToDelete = selectedProducts.value.some(product => product.isOnSale);
    if (notAllowedToDelete) {
      ElMessage({
        message: '所选商品中包含已经出售的商品，无法删除',
        type: 'warning'
      });
    } else {
      confirmDialogVisible.value = true;
    }
  } else {
    ElMessage({
      message: '请选择要删除的商品',
      type: 'warning'
    });
  }
};
const deleteSelectedCommodities = async () => {
  if (selectedProducts.value.length > 0) {
    try {
      // 确认对话框
      // await ElMessageBox.confirm('此操作将永久删除所选商品, 是否继续?', '提示', {
      //   confirmButtonText: '确定',
      //   cancelButtonText: '取消',
      //   type: 'warning'
      // });

      const storeId = localStorage.getItem('userId'); // 替换为实际的storeId
      const productIds = selectedProducts.value.map(product => product.id);

      // 发送请求到后端批量删除商品
      const response = await axiosInstance.delete('/StoreViewProduct/deleteProducts', {
        params: {
          storeId: storeId
        },
        data: productIds // 传递请求体
      });

      if (response.status === 200) {
        // 从本地商品列表中删除选中的商品
        products.value = products.value.filter(product => !selectedProducts.value.includes(product));
        selectedProducts.value = [];
        confirmDialogVisible.value = false;

        ElMessage({
          message: '选中的商品已删除',
          type: 'success'
        });
      } else {
        ElMessage({
          message: '删除商品失败',
          type: 'error'
        });
      }
    } catch (error) {
      console.error('删除商品失败:', error.response ? error.response.data : error.message);
      ElMessage({
        message: '删除商品失败: ' + error.message,
        type: 'error'
      });
    }
  } else {
    ElMessage({
      message: '请选择要删除的商品',
      type: 'warning'
    });
  }
};

    return {
      value,
      options,
      products,
      dialogVisible,
      dialogVisibleTwo,
      currentProduct,
      totalProducts,
      currentPageData,
      // totalPageCount,
      pageSize,
      OnOrNot,
      preProduct,
      newProduct,
      newImage,
      newImageText,
      addDialogVisible,
      confirmDialogVisible,
      selectedProducts,
      handleCheck,
      handleDelete,
      handlePageChange,
      handleEdit,
      onsubmit,
      showAddDialog,
      addNewProduct,
      handleSelectionChange,
      confirmBatchDelete,
      deleteSelectedCommodities,
      rules,
      form,
      editForm,
      searchName,
      searchcategoryInit,
      fetchProducts,
      viewType,
      handleChange,
      filterProducts,
      filterProductsTag,
      onFileChange,
      addImageWithText,
      handleFileChange
    };
  }
}
</script>

<style scoped>
.CommodityShow {
  position: fixed;
  top: 10vh;
  left: 150px; 
  right: 0;
  bottom: 0;
  background-color: #DFCDC7  ;
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
}

#BottomButton {
  display: inline-block;
  margin-right: 20px;
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

.el-button--primary {
    background-color: #a13232;
    border-color: #a13232;
}

.el-button--primary:hover {
    background-color: #8b2b2b;
    border-color: #8b2b2b;
}
.el-button--danger {
    background-color: #a13232;
    border-color: #a13232;
}

.el-button--danger:hover {
    background-color: #8b2b2b;
    border-color: #8b2b2b;
}
</style>