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
      <div v-if="currentProduct">
        <div class="container-block">
          <img src="@/assets/mmy/blue_background.jpg">
          <div class="inner-block">
            <div class="slider-top-right">
              <div class="picture-and-text">
                <transition class="animate__animated animate__fadeInDown">
                  <img class="picture":src="currentProduct.images" alt="ProductImage" >
                </transition>
                <transition class="animate__animated animate__fadeInUp">
                  <div class="text">
                    <span class="close" @click="dialogVisible = false">&times;</span>
                    <h2>商品名称: {{ currentProduct.name }}</h2>
                    <br>
                    <p>系统分类: {{ getCategoryLabel(currentProduct.categorySys) }}</p>
                    <p>商家分类: {{ currentProduct.categoryInit }}</p>
                    <p>商品价格: {{ currentProduct.price }}</p>
                    <p>是否出售: {{ currentProduct.isOnSale ? '是' : '否' }}</p>
                    <p>商品具体描述: {{ currentProduct.description}}</p>
                  </div>
                </transition>
              </div>
            </div>
          </div>
        </div> 
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
            <el-input v-model.number="preProduct.categoryInit"></el-input>
          </el-form-item>
          <el-form-item label="商品具体描述" prop="description">
            <el-input v-model="preProduct.description"></el-input>
          </el-form-item>
          <!-- <el-form-item label="商品图片(.jpg)" prop="image">
            <img :src="preProduct.image" alt="当前图片" v-if="preProduct.image" style="width: 200px; height: 200px;" />
            <input type="file" @change="(e) => onFileChange(e, 'preProduct')">
          </el-form-item> -->
          <el-form-item label="商品图片">

    

   <!-- 显示已存在的图片 -->
   <div v-if="preProduct.images && preProduct.images.length" style="display: flex; flex-wrap: wrap;">
      <div v-for="(image, index) in preProduct.images" :key="image.imageId" style="position: relative; margin-right: 10px;">
        <img :src="image.imageUrl" alt="商品图片" style="margin-top:10px;width: 150px; height: 150px; object-fit: cover;border-radius: 8px;" />
        <span class="close" style="position: absolute; margin-top:10px; top: 0; right: 5px; color: #82111f;" @click="removeImage(index)">&times;</span>
      </div>
    </div>
<!-- 显示选择的图片 -->
<div v-if="selectedImages.length" style="display: flex; flex-wrap: wrap;">
  <div v-for="(image, index) in selectedImages" :key="index" style="position: relative; margin-right: 10px;">
    <img :src="image.url" alt="选择的图片" style="margin-top:10px;width: 150px; height: 150px; object-fit: cover;border-radius: 8px;" />
    <span class="close" style="position: absolute; margin-top:10px; top: 0; right: 5px; color: #82111f;" @click="removeSelectedImage(index)">&times;</span>
  </div>
</div>
        <!-- 上传新图片 -->
<el-form-item label="">
  <input type="file" @change="handleFile" multiple />
</el-form-item>
      </el-form-item>

      <!-- <el-form-item label="瑕疵图片文字描述" prop="defectDescription">
  <el-input v-model="preProduct.defectDescription"></el-input>
</el-form-item> -->
<el-form-item label="瑕疵图片及描述">
  <div v-if="preProduct.defectImages && preProduct.defectImages.length" style="display: flex; flex-wrap: wrap;">
    <div v-for="(defect, index) in preProduct.defectImages" :key="defect.imageId" style="position: relative; margin-right: 10px;">
      <img :src="defect.image" alt="瑕疵图片" style="margin-top:10px;width: 150px; height: 150px; object-fit: cover; border-radius: 8px;" />
      <el-input v-model="defect.description" placeholder="输入瑕疵描述" style="width: 150px; margin-top: 5px;"></el-input>
      <span class="close" style="position: absolute; margin-top: 10px; top: 0; right: 5px; color: #82111f;" @click="removeDefectImage(index)">&times;</span>
    </div>
  </div>
</el-form-item>


<!-- 显示选择的瑕疵图片 -->
<!-- <div v-if="selectedDefectImages.length" style="display: flex; flex-wrap: wrap;">
  <div v-for="(image, index) in selectedDefectImages" :key="index" style="position: relative; margin-right: 10px;">
    <img :src="image.url" alt="选择的瑕疵图片" style="margin-top:10px;width: 150px; height: 150px; object-fit: cover;border-radius: 8px;" />
    <span class="close" style="position: absolute; margin-top:10px; top: 0; right: 5px; color: #82111f;" @click="removeSelectedDefectImage(index)">&times;</span>
  </div>
</div> -->


<!-- 上传瑕疵图片 -->
<!-- <el-form-item label="">
  <input type="file" @change="handleDefectFile" multiple />
</el-form-item> -->
<!-- <el-form-item label="">
  <input type="file" @change="handleDefectFile" multiple />
  <div v-for="(image, index) in selectedDefectImages" :key="index" class="defect-image-item">
    <img :src="image.url" alt="Defect Image" style="width: 100px; height: 100px;"/>
    <input type="text" v-model="image.description" placeholder="输入图片描述" />
    <el-button @click="removeSelectedDefectImage(index)">删除</el-button>
  </div>
</el-form-item> -->

<!-- 显示选择的瑕疵图片和描述输入框 -->
<div v-if="selectedDefectImages.length" style="display: flex; flex-wrap: wrap;">
  <div v-for="(image, index) in selectedDefectImages" :key="index" style="position: relative; margin-right: 10px;">
    <img :src="image.url" alt="选择的瑕疵图片" style="margin-top:10px;width: 150px; height: 150px; object-fit: cover;border-radius: 8px;" />
    <input type="text" v-model="image.description" placeholder="输入图片描述" style="width: 150px; margin-top: 5px;" />
    <span class="close" style="position: absolute; top: 0; right: 5px; color: #82111f; cursor: pointer;" @click="removeSelectedDefectImage(index)">&times;</span>
  </div>
</div> 

<!-- 上传瑕疵图片 -->
<el-form-item label="">
  <input type="file" @change="handleDefectFile" multiple />
</el-form-item>

          <div style="display: flex;justify-content: center;margin-top: 30px">
            <el-button type="primary" @click="onsubmit()">保存</el-button>
            <el-button  type="primary"  @click="handleCancel">取消</el-button>
          </div>
    </el-form>
  </div>
  </div>   


  <!-- 添加商品和批量删除按钮 -->
  <div id="BottomButton">
    <el-button size='small' type='primary' icon="Edit" @click="showAddDialog()">添加商品</el-button>
    <el-button size='small' type='danger' icon="Delete" @click="confirmBatchDelete()">批量删除</el-button>
  </div>

  <!-- 添加商品对话框 -->
  <div v-if="addDialogVisible" class="SettingPopUp">
    <div class="SettingContent">
      <!-- <span class="close" @click="addDialogVisible = false">&times;</span> -->
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
        <el-form-item label="商品描述" prop="description">
          <el-input v-model="newProduct.description"></el-input>
        </el-form-item>
        <el-form-item label="商品图片" prop="image">
          <div v-if="newProduct.images.length > 0" class="image-preview">
    <div v-for="(image, index) in newProduct.images" :key="index" class="image-item">
      <img :src="image" alt="图片" style="width: 100px; height: 100px;display:flex" />
    </div>
  </div>
  <input type="file" multiple @change="(e) => onFileChange(e, 'newProduct')">
        </el-form-item>

        <el-form-item label="瑕疵图文描述（图文一一对应）" prop="imagesWithText">
        <div v-for="(item, index) in newProduct.imagesWithText" :key="index" class="image-text-item">
          <img :src="item.image" alt="图片" style="width: 50px; height: 50px; margin-right: 5px" />
          <p>{{ item.text }}</p>
        </div>
        <input type="file" @change="handleFileChange($event, 'newImage')">
        <el-input   type="textarea"  :rows="4"  v-model="newImageText" placeholder="输入瑕疵文字描述"></el-input>
       
        <el-button   @click="addImageWithText(newImage, newImageText)">添加瑕疵图文描述</el-button>
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
import { useRouter } from 'vue-router';

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
    ElPagination ,
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
    const router = useRouter();
    const selectedImages = ref([]); 
    const deletedImages= ref([]); 
    const selectedDefectImages = ref([]);
    const deletedDefectImages= ref([]);  
  
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

// 获取标签的方法
const getCategoryLabel = (value) => {
      for (const category of options) {
        const found = category.children.find(child => child.value === value);
        if (found) {
          return found.label;
        }
      }
      return '未知分类';
    };

    const OnOrNot = [
      {value: true, label: '是'},
      {value: false, label: '否'}
    ] 

    // 查看
    const handleCheck = (item) => {
      currentProduct.value = item;
      // dialogVisible.value = true;
      // console.log(currentProduct.value);
      localStorage.setItem('productIdOfDetail', item.id);
      router.push('/productdetail');
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
      // image: [{ required: true, message: '请上传商品图片', trigger: 'change' }],
      images: [
    { validator: (rule, value, callback) => {
      if (value.length === 0) {
        callback(new Error('请上传至少一张商品图片'));
      } else {
        callback();
      }
    }, trigger: 'change' }
  ],
      imagesWithText: [
    { validator: (rule, value, callback) => {
        if (!Array.isArray(value) || value.length === 0) {
          callback(new Error('请添加至少一组瑕疵描述'));
        } else {
          callback();
        }
      }, trigger: 'change'
    }
  ]
    };

    const onFileChange = (event, target) => {
  const files = event.target.files;
  if (files && target === 'newProduct') {
    Array.from(files).forEach(file => {
      const reader = new FileReader();
      reader.onload = (e) => {
        const base64Image = `data:image/jpeg;base64,${e.target.result.split(',')[1]}`;
        newProduct.value.images.push(base64Image); // 将 Base64 数据添加到 images 数组中
      };
      reader.readAsDataURL(file);
    });
  }
  else if(target === 'preProduct'){
    preProduct.value.image = base64Image;
  }
};
   
    // 编辑
    const removeSelectedImage = (index) => {
  selectedImages.value.splice(index, 1);
};
const removeSelectedDefectImage = (index) => {
  selectedDefectImages.value.splice(index, 1);
};
    const removeImage = (index) => {
      if (preProduct.value.images.length > 1) {
    // 从显示的图片数组中移除
    const removedImage = preProduct.value.images.splice(index, 1)[0]; // 从 preProduct.images 中移除图片，并获取该图片对象

    // 将已删除的图片对象添加到 deletedImages 数组中
    if (removedImage) {
      deletedImages.value.push(removedImage);
    }
  } else {
    ElMessage({
      message: '至少需要保留一张商品图片',
      type: 'warning'
    });
  }
    };
    const removeDefectImage = (index) => {
    // 确保至少保留一张瑕疵图片
    if (preProduct.value.defectImages.length > 1) {
    // 从显示的瑕疵图片数组中移除
    const removedImage = preProduct.value.defectImages.splice(index, 1)[0];
    
    // 将已删除的瑕疵图片对象添加到 deletedDefectImages 数组中
    if (removedImage) {
      deletedDefectImages.value.push(removedImage);
    }
  } else {
    ElMessage({
      message: '至少需要保留一张瑕疵图片',
      type: 'warning'
    });
  }
};
    const handleCancel = () => {
  // 清空选择的图片
  selectedImages.value = [];
    // 清空 deletedImages 数组
  deletedImages.value = [];
  // 关闭对话框
  dialogVisibleTwo.value = false;
};
    const handleFile = (event) => {
      const files = event.target.files;
  if (files.length > 0) {
    const newImages = Array.from(files).map(file => ({
      url: URL.createObjectURL(file),
      file: file,
    }));
    selectedImages.value = [...selectedImages.value, ...newImages];
  }
    };
    const handleDefectFile = (event) => {
  const files = event.target.files;
  if (files.length > 0) {
    const newImages = Array.from(files).map(file => ({
      url: URL.createObjectURL(file),
      file: file,
       description: '无'
    }));
    selectedDefectImages.value = [...selectedDefectImages.value, ...newImages];
  }
};
   
    const handleEdit = async (item) => {
      if (item.isOnSale) {
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
        description: item.description,
        images: [],
        defectImages:[],
        // defectDescription: '',  
      };

       // 获取商品图片
  try {
    const response = await axiosInstance.get(`/StoreViewProduct/getProductImages/${preProduct.value.id}`);
    if (response.status === 200) {
      preProduct.value.images = response.data;  // 将图片数据赋值给 preProduct.images
    } else {
      ElMessage({
        message: '获取商品图片失败else',
        type: 'error'
      });
    }
  } catch (error) {
    
    ElMessage({
      message: '获取商品图片失败，本商品可能不存在图片',
      type: 'error'
    });
  }
  try {
  const defectImagesResponse = await axiosInstance.get(`/StoreViewProduct/getDetailImages/${preProduct.value.id}`);
  if (defectImagesResponse.status === 200) {
    preProduct.value.defectImages = defectImagesResponse.data;

    // 遍历每个瑕疵图片，获取其对应的描述
    await Promise.all(preProduct.value.defectImages.map(async (defectImage) => {
      try {
        const descriptionResponse = await axiosInstance.get(`/StoreViewProduct/getProductDescription`, {
          params: { imageId: defectImage.imageId }
        });
        if (descriptionResponse.status === 200) {
          defectImage.description = descriptionResponse.data.length > 0 ? descriptionResponse.data[0].description : '';
          console.log('Defect Images:', preProduct.value.defectImages);
        } else {
          defectImage.description = '描述获取失败';
        }
      } catch (error) {
        defectImage.description = '描述获取失败';
      }
    }));
  } else {
    ElMessage({
      message: '获取瑕疵图片失败',
      type: 'error'
    });
  }
} catch (error) {
  ElMessage({
    message: '获取瑕疵图片失败，本商品可能不存在瑕疵图片',
    type: 'error'
  });
}
  
      
      dialogVisibleTwo.value = true;
    };
    const updateDefectDescription = async () => {
  try {
    // 遍历 preProduct.defectImages，准备更新描述
    const updateRequests = preProduct.value.defectImages.map(defectImage => {
      return axiosInstance.post('/StoreViewProduct/updateProductDescription', {
        imageId: defectImage.imageId,
        description: defectImage.description || '无' // 如果没有描述，则默认为“无”
      });
    });

    // 等待所有更新请求完成
    const responses = await Promise.all(updateRequests);
    
    // 检查所有响应状态
    const allSuccessful = responses.every(response => response.status === 200);
    if (allSuccessful) {
      ElMessage({
        message: '瑕疵图片文字描述已更新',
        type: 'success'
      });
    } else {
      ElMessage({
        message: '更新瑕疵图片文字描述失败',
        type: 'error'
      });
    }
  } catch (error) {
    console.error('更新瑕疵图片文字描述时发生错误:', error.response ? error.response.data : error.message);
    ElMessage({
      message: '更新瑕疵图片文字描述失败',
      type: 'error'
    });
  }
};
const submitUpload = async () => {
  if (selectedImages.value.length > 0) { // 使用 selectedImages.value 而不是 fileList.value
    const formData = new FormData();
    
    selectedImages.value.forEach(image => {
      formData.append('images', image.file); // 从 selectedImages.value 中添加文件
    });
    console.log(preProduct.value.id);
    const uploadUrl = `/StoreViewProduct/addProductImage/${preProduct.value.id}`;
    try {
      const response = await axiosInstance.post(uploadUrl, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });
      if (response.status === 200) {
        ElMessage({
          message: '图片上传成功',
          type: 'success'
        });
        // 清空选择的图片列表
        selectedImages.value = [];
      } else {
        ElMessage({
          message: '图片上传失败1',
          type: 'error'
        });
      }
    } catch (error) {
      console.error('上传图片时发生错误:', error.response ? error.response.data : error.message);
      ElMessage({
        message: '图片上传失败2',
        type: 'error'
      });
    }
  }
};
const submitDefectUpload = async () => {
  if (selectedDefectImages.value.length > 0) {
    const formData = new FormData();

    // 添加文件到 FormData
    selectedDefectImages.value.forEach(image => {
      formData.append('images', image.file);
    });

    // 添加描述信息到 FormData
    const descriptions = selectedDefectImages.value.map(image => image.description || '无');
    formData.append('descriptions', JSON.stringify(descriptions)); // 将描述信息作为 JSON 字符串添加
    
    const uploadUrl = `/StoreViewProduct/addDetailImage/${preProduct.value.id}`;
    try {
      const response = await axiosInstance.post(uploadUrl, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });
      if (response.status === 200) {
        ElMessage({
          message: '瑕疵图片上传成功',
          type: 'success'
        });
        selectedDefectImages.value = [];
      } else {
        ElMessage({
          message: '瑕疵图片上传失败',
          type: 'error'
        });
      }
    } catch (error) {
      console.error('上传瑕疵图片时发生错误:', error.response ? error.response.data : error.message);
      ElMessage({
        message: '瑕疵图片上传失败',
        type: 'error'
      });
    }
  }
};
    const onsubmit = async () => {
    editForm.value.validate(async (valid) => {
        if (valid) {
            const formData = new FormData();
            
            // 将数据添加到 FormData 对象
            formData.append('productId', preProduct.value.id);
            formData.append('productName', preProduct.value.name);
            formData.append('productPrice', preProduct.value.price);
            formData.append('storeTag', preProduct.value.categoryInit);
            formData.append('tag', preProduct.value.categorySys);
            formData.append('description', preProduct.value.description);
            const userId = localStorage.getItem('userId'); 
            try {
                const response = await axiosInstance.put('/StoreViewProduct/editProduct', formData, {
                    params: {
                        storeId: userId // 替换为实际的 storeId
                    },
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
                  // 上传图片
          if (selectedImages.value.length > 0) {
            await submitUpload();
          }
          //删除商品图片
          try {
            for (const image of deletedImages.value) {
              const response = await axiosInstance.delete(`/StoreViewProduct/deleteProductImage/${preProduct.value.id}/${image.imageId}`);
              if (response.status === 401) {
                ElMessage({
                  message: '所有图片已被删除',
                  type: 'warning'
                });
              }
            }
          } catch (error) {
            if (error.response && error.response.status === 401) {
    ElMessage({
      message: '所有图片已被删除',
      type: 'warning'
    });
  } else {
    // 处理其他非401错误
    ElMessage({
      message: '删除图片时发生错误',
      type: 'error'
    });
  }
          }
          // 清空 deletedImages 数组
          deletedImages.value = [];
          //上传瑕疵图片
          await submitDefectUpload();
            // 删除瑕疵图片
            try {
                        for (const defectImage of deletedDefectImages.value) {
                            const response = await axiosInstance.delete(`/StoreViewProduct/deleteDetailImage/${preProduct.value.id}/${defectImage.imageId}`);
                            if (response.status === 401) {
                                ElMessage({
                                    message: '所有瑕疵图片已被删除',
                                    type: 'warning'
                                });
                            }
                        }
                    } catch (error) {
                        if (error.response && error.response.status === 401) {
                            ElMessage({
                                message: '所有瑕疵图片已被删除',
                                type: 'warning'
                            });
                        } else {
                            ElMessage({
                                message: '删除瑕疵图片时发生错误',
                                type: 'error'
                            });
                        }
                    }
                    
                    // 清空 deletedDefectImages 数组
                    deletedDefectImages.value = [];
          //更新瑕疵图文描述
          await updateDefectDescription();
            } else {
                ElMessage({
                    message: '更新商品信息失败?',
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
      images: [], // 存储多个商品图片
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
        images: [], // 存储多个商品图片
        imagesWithText: []
      };
      addDialogVisible.value = true;
    };
    const handleFileChange = (event) => {
  const file = event.target.files[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = (e) => {
      const base64Image = `data:image/jpeg;base64,${e.target.result.split(',')[1]}`;// 获取完整的 Base64 字符串
      newImage.value = base64Image; // 更新 newImage 的值
    };
    reader.readAsDataURL(file);
  }
};
const dataURLToBlob = (dataURL) => {
  const [header, data] = dataURL.split(',');
  const mime = header.match(/:(.*?);/)[1];
  const binary = atob(data);
  const array = [];
  for (let i = 0; i < binary.length; i++) {
    array.push(binary.charCodeAt(i));
  }
  return new Blob([new Uint8Array(array)], { type: mime });
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
     
      const formData = new FormData();
      formData.append('ProductName', newProduct.value.name || '');
      formData.append('ProductPrice', newProduct.value.price || '');
      formData.append('StoreTag', newProduct.value.categoryInit || '');
      formData.append('Description', newProduct.value.description || '');
      formData.append('Tag', newProduct.value.categorySys || '');

      // 主图
      // if (newProduct.value.image) {
      //   const productPicBlob = dataURLToBlob(newProduct.value.image);
      //   formData.append('ProductImages', productPicBlob, 'product-image.jpg');
      // }
      // 上传多个图片
      newProduct.value.images.forEach((image, index) => {
        const imageBlob = dataURLToBlob(image);
        formData.append(`ProductImages[${index}]`, imageBlob, `product-image${index}.jpg`);
      });

      // 图片和描述
      newProduct.value.imagesWithText.forEach((item, index) => {
        const detailPicBlob = dataURLToBlob(item.image);
        formData.append(`PicDes[${index}].detailPic`, detailPicBlob, `detail-image${index}.jpg`);
        formData.append(`PicDes[${index}].description`, item.text);
      });


      const storeId = localStorage.getItem('userId');// 替换为实际的storeId
      console.log(storeId);
      if (!storeId) {
        ElMessage({ message: '未找到有效的 storeId', type: 'error' });
        return;
      }
      console.log('请求 URL:', `/StoreViewProduct/addProduct?storeId=${storeId}`);
      // console.log(newProductData);
      console.log(newProduct.value.categorySys);

      try {
        const response = await axiosInstance.post(`/StoreViewProduct/addProduct?storeId=${storeId}`, formData, {
          headers: { 'Content-Type': 'multipart/form-data' },
          params: { storeId: storeId }
        });

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
            images: [], // 存储多个商品图片
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
      handleFileChange,
      getCategoryLabel,
      removeImage,
      handleFile,
      removeSelectedImage,
      selectedImages,
      handleCancel,
      deletedImages,
      handleDefectFile,
      removeSelectedDefectImage,
      submitDefectUpload,
      removeDefectImage,
      selectedDefectImages,
      deletedDefectImages
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
  background-color: #fefefe;
  color: black;
  display: inline-block;
  padding: 5vh;
  border-radius: 20px;
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

.animate__animated.animate__fadeInUp{
	--animate-duration: 1.5s;
}
.animate__animated.animate__fadeInDown{
	--animate-duration: 1.5s;
}
.container-block {
    overflow: hidden;
    border-radius: 15px;
	color: #fff;
	display: inline-block;
	margin: 2rem auto;
	max-width: 60%;
	position: relative;	
	&::before {
		/* 特别修改1 */
        /* 企图用覆盖层的颜色改变原本背景色,并保留原本图案*/
		/* background-color: rgba(59, 69, 109,0.3); */
        background-color: rgba(250, 13, 40, 0.3);
        /* background-color:#82111f; */
		bottom: 0;
		content: '';
		display: block;
		position: absolute;
		top: 0;
		width: 100%;
	}
	&:hover {
		.inner-block:before,
		.slider-top-right:after {
			height: 100%;
		}
		.inner-block:after,
		.slider-top-right:before {
			width: 100%;
		}
	}
	img {
		display: block;
		max-width: 100%;
	}
}

.slider-top-right:before,
.inner-block:after {
	height: 2px;
	transition: width .75s ease;
	width: 0%;
}

.slider-top-right:after,
.inner-block:before {
	height: 0%;
	transition: height .75s ease;
	width: 2px;
}

.inner-block:before,
.inner-block:after,
.slider-top-right:before,
.slider-top-right:after {
	background-color: #fff;
	content: '';
	display: block;
	position: absolute;
}

.inner-block {
	font-size: 2em;
	width: 90%;
	height: 90%;
	position: absolute;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
	margin: auto;
	&:before {
		bottom: 0;
		left: 0;
	}
	&:after {
		bottom: 0;
		right: 0;
	}
}

.slider-top-right {
	position: relative;
	width: 100%;
	height: 100%;
	display: flex; /* 使用 Flexbox */
	justify-content: center; /* 水平居中 */
	align-items: center; /* 垂直居中 */
	&:before {
		top: 0;
		left: 0;
	}
	&:after {
		top: 0;
		right: 0;
	}
}
.picture-and-text{
	width: 90%;
	height:90%;
	display: flex; 
	flex-direction: row;
}
.picture{
	width:50%;
	border-radius: 20px;
}
.text{
	/* 华文宋 */
	width: 50%;
	font-family: 'Noto Serif SC', serif;
	font-size: 20px;
	padding: 10px 10px 20px 20px;
	overflow-y: auto; 
	max-height: 100%; /* 限制最大高度为父容器高度 */
	text-align: left;
	
}
/* 特别修改2 */
/* 自定义滚动条样式 */
.text::-webkit-scrollbar {
  width: 5px;
}

.text::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 10px;
}

.text::-webkit-scrollbar-thumb {
  background: #5f697a;
  border-radius: 10px;
}
h2 {
    font-size: 1.5em;
    /* margin-block-start: 0.83em;
    margin-block-end: 0.83em;
    margin-inline-start: 0px;
    margin-inline-end: 0px; */
    font-weight: bold;
    unicode-bidi: isolate;
    color: #ecdada;
    text-align: center;
  }

  .image-preview {
  display: flex;
  flex-wrap: wrap; /* 使图片在容器宽度不足时自动换行 */
  gap: 10px; /* 图片之间的间距 */
}

.image-item {
  display: flex;
  align-items: center; /* 垂直居中对齐图片 */
}

.image-item img {
  max-width: 100px; /* 控制图片最大宽度 */
  max-height: 100px; /* 控制图片最大高度 */
  object-fit: cover; /* 保持图片比例，并填充容器 */
}


.custom-upload .el-upload-list__item {
  width: 100%;
  height: 100%;
}

.custom-upload .el-upload-list__item-thumbnail {
  width: 100%;
  height: 100%;
  object-fit: cover; /* 确保图片充满整个卡片 */
}

.custom-upload .el-upload-list__item-status {
  display: none; /* 如果需要隐藏其他状态图标 */
}
</style>