<template>
  <div class="Background">
    <div class="BlockOne">
      <span class="OrderTOFinish">
        <span>待办订单</span>
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
        <span class="Frame">待发货</span>
        <span>待售后</span>
      </span>
      <span class="Market">
        <span>相关市集</span>
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
        <span class="Frame">已参与</span>
        <span>可报名</span>
      </span>
      <span class="Today">
        <span>今日总览</span>
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
        <span>订单统计</span>
        <OrderStatisticsChart :data="orderData" :labels="days" />
      </span>
      <router-view/>
    </div>
    </div>
</template>
<script>

import OrderStatisticsChart from './OverviewView.vue';

export default {
  components: {
    OrderStatisticsChart
  },
  data() {
    return {
      days: ['1日', '2日', '3日', '4日', '5日', '6日', '7日'],
      orderData: [10, 12, 21, 54, 260, 830, 710],
      number:12234
    };
  }
};
</script>

<style scoped>
.BlockOne {
  display: flex;
  padding-left: 60px;
  padding-top: 60px;
}

.OrderTOFinish {
  width:300px;
  background-color: rgb(226, 233, 233);
  margin-right: 100px;
  padding:15px;
}

.Market {
  width:300px;
  background-color: rgb(238, 238, 238);
  margin-right: 100px;
  padding:15px;
}

.Today {
  width:300px;
  background-color: rgb(233, 233, 244);
  padding:15px;
}

.Frame {
    padding-left: 60px;
    padding-right: 65px;
  }
  
  .FrameTwo {
    padding-left: 40px;
    padding-right: 45px;
  }

.BlockTwo {
  display: flex;
  padding-left: 60px;
  padding-top: 40px;
}

.OrderStatistics {
  width:1200px;
  height: 450px;
  padding:15px;
  background-color: rgb(235, 236, 226);
}

.Background {
  padding-right: 130px;
  padding-bottom: 45px;
  background-color: rgb(84, 138, 87);
}

.box-card {
  width: 90px;
  height: 160px;
  margin-top: 10px;
  margin-left:40px;
  background-color: rgb(110, 216, 142);
  display: inline-block;
}

.box-card-two {
  width: 90px;
  height: 160px;
  margin-top: 10px;
  margin-left:20px;
  background-color: rgb(110, 216, 142);
  display: inline-block;
}

.Number {
  display: flex;
  justify-content: center;
  align-items: center; 
  height: 160px;
}
</style>