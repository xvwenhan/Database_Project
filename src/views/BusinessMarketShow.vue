<template>
  <div class="CommodityShow">
    <div class="SearchContainer">
      <el-input v-model="searchTopic" placeholder="请输入市集主题" style="display: inline-block;"></el-input>
      <el-button type="primary" @click="filterMarkets">搜索</el-button>
    </div>
    <div class="TableContainer">
      <el-table :data="currentPageData" class="CommodityTable" height="760">
        <el-table-column type="index" />
        <el-table-column prop="marketId" label="市集ID"></el-table-column>
        <el-table-column prop="theme" label="市集主题"></el-table-column>
        <el-table-column prop="startTime" label="开始时间"></el-table-column>
        <el-table-column prop="endTime" label="结束时间"></el-table-column>
        <el-table-column label="操作">
          <template #default="scope">
            <el-button-group>
              <el-button size="mini" type="primary" icon="check" @click="handleCheck(scope.row)">查看</el-button>
              <el-button
                size="mini"
                :type="scope.row.isStoreParticipating ? 'success' : 'primary'"
                :icon="scope.row.isStoreParticipating ? 'check' : 'circle'"
                @click="toggleParticipation(scope.row)"
              >
                {{ scope.row.isStoreParticipating ? '已参与' : '未参与' }}
              </el-button>
            </el-button-group>
          </template>
        </el-table-column>
      </el-table>
    </div>

    <div v-if="dialogVisible" class="SettingPopUp">
      <div v-if="currentMarket" class="SettingContent">
        <span class="close" @click="dialogVisible = false">&times;</span>
        <p>市集ID: {{ currentMarket.marketId }}</p>
        <p>市集主题: {{ currentMarket.theme }}</p>
        <p>开始时间: {{ currentMarket.startTime }}</p>
        <p>结束时间: {{ currentMarket.endTime }}</p>
        <p>详细信息: {{ currentMarket.detail }}</p>
        <p>海报图片:
          <img :src="currentMarket.posterImg" alt="MarketPoster">
        </p>
      </div>
    </div>  

    <div class="paginationContainer">
      <el-pagination
        v-model:current-page="currentPage"
        :page-size="pageSize"
        :total="totalMarkets"
        layout="total, prev, pager, next, jumper"
        @current-change="handlePageChange"
      ></el-pagination>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue';
import axiosInstance from '../components/axios';
import imageA from '@/assets/setting.svg';
import { ElSelect, ElOption, ElButton ,ElMessage} from 'element-plus';
import 'element-plus/dist/index.css';

export default {
  components: {
    ElSelect,
    ElOption, 
    ElButton
  },
  setup() {
    const value = ref('');
    const dialogVisible = ref(false);
    const currentMarket = ref(null);
    const searchTopic = ref('');

    const markets = ref([]);
    const pageSize = 20;
    const currentPage = ref(1);
    const totalMarkets = computed(() => markets.value.length);

    const currentPageData = computed(() => {
      const start = (currentPage.value - 1) * pageSize;
      const end = Math.min(start + pageSize, totalMarkets.value);
      return markets.value.slice(start, end);
    });

    const handleCheck = (row) => {
      currentMarket.value = row;
      dialogVisible.value = true;
    };

    
    const toggleParticipation = async (row) => {
      const storeId = localStorage.getItem('userId'); // 替换为实际的 storeId
      console.log('Stored User ID:', localStorage.getItem('userId'));
      const inOrOut = !row.isStoreParticipating; // 反转参与状态

       // 检查当前时间是否在市集的开始时间和结束时间范围内
        const currentTime = new Date();
        const startTime = new Date(row.startTime);
        const endTime = new Date(row.endTime);
     
        if (currentTime < startTime || currentTime > endTime) {
          ElMessage({
            message: '市集不在有效时间范围内，无法改变参与状态。',
            type: 'warning'
          });
          return;
        }


      try {
        await axiosInstance.put('/StoreViewMarket/UpdateMarketStoreParticipation', {
          marketId: row.marketId,
          storeId: storeId,
          inOrOut: inOrOut,
        });

        // 更新本地状态
        row.isStoreParticipating = inOrOut;
      } catch (error) {
        console.error('更新市场参与状态失败:', error);
      }
      };

    const handlePageChange = (page) => {
      currentPage.value = page;
    };

    const fetchMarkets = async () => {
      const storeId =localStorage.getItem('userId'); // 替换为实际的 storeId

      try {
        const response = await axiosInstance.get('/StoreViewMarket/GetMarketsByStoreId', {
          params: {
            storeId: storeId,
          },
        });

     
    console.log('Backend Response:', response.data);
            

    if (Array.isArray(response.data)) {
          const processedMarkets = response.data.map(market => {
            console.log('Original Market Data:', market);

            // 确保字段存在并转换正确
            const startTime = market.startTime ? new Date(market.startTime).toLocaleString() : 'N/A';
            const endTime = market.endTime ? new Date(market.endTime).toLocaleString() : 'N/A';
            const posterImg = market.posterImg ? `data:image/png;base64,${market.posterImg}` : imageA;

            return {
              marketId: market.marketId || 'N/A',
              theme: market.theme || 'Unknown',
              startTime: startTime,
              endTime: endTime,
              detail: market.detail || 'No details available',
              posterImg: posterImg,
              isStoreParticipating: market.isStoreParticipating !== undefined ? market.isStoreParticipating : false,
            };
          });

          console.log('Processed Markets:', processedMarkets);
          markets.value = processedMarkets; // 确保 markets 是 Vue 的响应式对象
        } else {
          console.error('Unexpected response format:', response.data);
        }
          } catch (error) {
            console.error('Error fetching markets:', error);
          }
        };


        const fetchMarketByTheme = async (theme) => {
  const storeId = localStorage.getItem('userId');// 替换为实际的 storeId
  try {
    const response = await axiosInstance.get('/StoreViewMarket/searchMarket', {
      params: {
        storeId: storeId,
        theme: theme || '', // 允许 theme 参数为空
      }
    });

    if (Array.isArray(response.data)) {
      // 处理返回的市场数据
      const processedMarkets = response.data.map(market => {
        const startTime = market.startTime ? new Date(market.startTime).toLocaleString() : 'N/A';
        const endTime = market.endTime ? new Date(market.endTime).toLocaleString() : 'N/A';
        const posterImg = market.posterImg ? `data:image/png;base64,${market.posterImg}` : imageA;

        return {
          marketId: market.marketId || 'N/A',
          theme: market.theme || 'Unknown',
          startTime: startTime,
          endTime: endTime,
          detail: market.detail || 'No details available',
          posterImg: posterImg,
          isStoreParticipating: market.isStoreParticipating !== undefined ? market.isStoreParticipating : false,
        };
      });

      markets.value = processedMarkets; 
    } else {
      console.error('Unexpected response format:', response.data);
    }
  } catch (error) {
    console.error('通过市集主题获取市集数据失败:', error);
  }
};

const filterMarkets = () => {
  if (searchTopic.value.trim() !== '') {
    fetchMarketByTheme(searchTopic.value.trim());
  } else {
    fetchMarkets();
  }
};

    onMounted(() => {
      fetchMarkets();
    });

    return {
      value,
      dialogVisible,
      currentMarket,
      searchTopic,
      markets,
      pageSize,
      currentPage,
      totalMarkets,
      currentPageData,
      handleCheck,
      toggleParticipation,
      handlePageChange,
      filterMarkets,
    };
  },
};
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