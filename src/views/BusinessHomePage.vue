<!-- 商家首页 -->
<template>
  <div class="Background">
    <div class="BlockOne">
      <span class="BlockThree">
        <div style="background-color: seagreen;">待办订单</div>
        <br>
        <el-card class="box-card" >
          <span class="Number">
            <el-statistic :value="number" />
          </span>
        </el-card>
        <el-card class="box-card-two">
          <span class="Number">
            <el-statistic :value="number" />
          </span>
        </el-card>
        <br>
        <span class="Frame">待发货</span>
        <span>待售后</span>
      </span>
      <span class="BlockThree">
        <div style="background-color: seagreen;">相关市集</div>
        <br>
        <el-card class="box-card" >
          <span class="Number">
            <el-statistic :value="number" />
          </span>
        </el-card>
        <el-card class="box-card-two">
          <span class="Number">
            <el-statistic :value="number" />
          </span>
        </el-card>
        <br>
        <span class="Frame">已参与</span>
        <span>可报名</span>
      </span>
      <span class="BlockThree">
        <div style="background-color: seagreen;">今日总览</div>
        <br>
        <el-card class="box-card" >
          <span class="Number">
            <el-statistic :value="number" />
          </span>
        </el-card>
        <el-card class="box-card-two">
          <span class="Number">
            <el-statistic :value="number" />
          </span>
        </el-card>
        <br>
        <span class="FrameTwo">今日新订单</span>
        <span>今日营业额</span>
      </span>
    </div>
    <div class="BlockTwo">
      <span class="OrderStatistics">
        <div style="background-color: seagreen;">订单统计</div>
        <OrderStatisticsChart :data="orderData" :labels="lastSevenDays" />
      </span>
    </div>
    <div class="StoreScore">
      <div v-if="loading">Loading...</div>
      <div v-if="error">{{ error }}</div>
      <div v-if="storeScore !== null">Store Score: {{ storeScore }}</div>
    </div>
    </div>
</template>

<script>
import OrderStatisticsChart from './OverviewView.vue';
import axios from 'axios';

export default {
  components: {
    OrderStatisticsChart,
  },
  data() {
    return {
      orderData: [10, 12, 21, 54, 260, 830, 710],
      lastSevenDays: this.getLastSevenDays(),
      storeScore: null,
      loading: false,
      error: null
    };
  },
  created() {
    this.fetchStoreScore();  // 在组件创建时调用方法获取店铺评分
  },
  methods: {
    getLastSevenDays() {
      const days = [];
      const today = new Date();

      for (let i = 0; i < 7; i++) {
        const date = new Date();
        date.setDate(today.getDate() - i);
        const formattedDate = this.formatDate(date);
        days.push(formattedDate);
      }

      return days.reverse(); // 反转数组使日期按顺序排列
    },
    formatDate(date) {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const day = String(date.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    },
    async fetchStoreScore() {
      const storeid = 'yourStoreId';  // 这里替换为实际的 storeid
      this.loading = true;
      this.error = null;
      this.storeScore = null;

      try {
        const response = await axios.get('/api/StoreFront/UpdateStoreScore', {
          params: { storeid }
        });
        this.storeScore = response.data;
        console.log('Store score:', response.data); // 控制台输出
      } catch (error) {
        this.error = 'Failed to fetch store score';
        console.error('Error fetching store score:', error);
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style scoped>
.Background {
  background-color: rgb(84, 138, 87);
  position: fixed;
  top: 6vh;
  left: 23vh; 
  right: 0;
  bottom: 0;
}

.BlockOne {
  display: flex;
  padding-left: 100px;
  padding-top: 60px;
}

.BlockTwo {
  display: flex;
  padding-left: 100px;
  padding-top: 70px;
}

.BlockThree {
  color: black;
  width:300px;
  background-color: rgb(226, 233, 233);
  margin-right: 100px;
  padding:15px;
}

.Frame {
  padding-right: 80px;
}
  
.FrameTwo {
  padding-right: 50px;
}

.OrderStatistics {
  width:90%;
  height: 450px;
  padding:15px;
  color: black;
  background-color: rgb(235, 236, 226);
}

.box-card {
  width: 90px;
  height: 160px;
  background-color: rgb(110, 216, 142);
  display: inline-block;
}

.box-card-two {
  width: 90px;
  height: 160px;
  margin-left:30px;
  background-color: rgb(110, 216, 142);
  display: inline-block;
}

.Number {
  display: flex;
  justify-content: center;
  align-items: center; 
  height: 120px;
}
</style>