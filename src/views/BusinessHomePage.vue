<!-- 商家首页 -->
<template>
  <div class="Background">
    <div class="BlockOne">
      <span class="BlockThree">
        <div style="background-color: seagreen;">待办订单</div>
        <br>
        <el-card class="box-card" >
          <span class="Number">
            <el-statistic :value="stats.waitingForShipment" />
          </span>
        </el-card>
        <el-card class="box-card-two">
          <span class="Number">
            <el-statistic :value="stats.waitingForReturn" />
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
            <el-statistic :value="stats.inCount" />
          </span>
        </el-card>
        <el-card class="box-card-two">
          <span class="Number">
            <el-statistic :value="stats.outCount" />
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
            <el-statistic :value="stats.orderCount" />
          </span>
        </el-card>
        <el-card class="box-card-two">
          <span class="Number">
            <el-statistic :value="stats.totalRevenue" />
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
    </div>
</template>

<script>
import OrderStatisticsChart from './OverviewView.vue';
import axiosInstance from '../components/axios';

export default {
  components: {
    OrderStatisticsChart,
  },
  data() {
    return {
      orderData: [],
      lastSevenDays: this.getLastSevenDays(),
      stats: {
        waitingForShipment: 0,
        waitingForReturn: 0,
        inCount: 0,
        outCount: 0,
        orderCount: 0,
        totalRevenue: 0,
      },
    };
  },
  methods: {
    async fetchOrderStats() {
      const storeId = 'S1234567'; // 替换为实际的 storeId
      const today = this.formatDate(new Date());

      try {
        const response = await axiosInstance.get('/StoreFront/GetOrderStats', {
          params: {
            storeId: storeId,
            date: today,
          },
        });

        // 更新组件数据
        this.stats = response.data;
      } catch (error) {
        console.error('Error fetching order stats:', error);
      }
    },
    getLastWeekDate() {
    const date = new Date();
    date.setDate(date.getDate() - 7); // 设置为今天之前的第七天
    return this.formatDate(date); // 格式化日期
    },
    async fetchWeeklyOrderCount() {
      const storeId = 'S1234567'; // 替换为实际的 storeId
      const lastWeekDate = this.getLastWeekDate();

      try {
        const response = await axiosInstance.get('/StoreFront/GetWeeklyOrderCount', {
          params: {
            storeId: storeId,
            date: lastWeekDate,
          },
        });

        console.log('Weekly order count response:', response.data);


        // 更新订单数据
        this.orderData = response.data.map(item => item.count);
        this.lastSevenDays = this.getLastSevenDays(); // 更新日期标签
      } catch (error) {
        console.error('Error fetching weekly order count:', error);
      }
    },
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
  },
  mounted() {
    this.fetchOrderStats(); // 组件挂载后请求数据
    this.fetchWeeklyOrderCount(); // 请求一周订单数据
  },
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
  width: 1500px;
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