<template>
    <div>
      <div ref="chart" style="width: 100%; height: 400px;"></div>
    </div>
  </template>
  
  <script>
  import { ref, onMounted } from 'vue';
  import * as echarts from 'echarts';
  
  export default {
    name: 'OrderStatisticsChart',
    props: {
      data: {
        type: Array,
        required: true
      },
      labels: {
        type: Array,
        required: true
      }
    },
    setup(props) {
      const chart = ref(null);
  
      onMounted(() => {
        const myChart = echarts.init(chart.value);
  
        const option = {
          title: {
            text: '订单数目'
          },
          tooltip: {
            trigger: 'axis'
          },
          xAxis: {
            type: 'category',
            data: props.labels
          },
          yAxis: {
            type: 'value'
          },
          series: [
            {
              name: '订单数目',
              type: 'line',
              data: props.data
            }
          ]
        };
  
        myChart.setOption(option);
      });
  
      return {
        chart
      };
    }
  };
  </script>
  
  <style scoped>
  </style>