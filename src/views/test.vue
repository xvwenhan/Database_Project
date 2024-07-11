<template>
    <div class="container">
      <AdminSidebarMenu />
      <div class="main-content">
        <AdminHeaderSec />
        <div class="content">
            <MerchantCertification />
          
        </div>
      </div>
    </div>
    
  </template>
    
  <script setup>
  import AdminSidebarMenu from '../components/AdminSidebarMenu.vue'
  import AdminHeaderSec from '../components/AdminHeaderSec.vue'
  import MerchantCertification from '../views/MerchantCertification.vue'
  
  
  
  </script>
  
  <style scoped>
  .container {
    display: flex;
    height: 100vh;
  }
  
  .main-content {
    flex: 1;
    display: flex;
    flex-direction: column;
  }
  
  .content {
    flex: 1;
    padding: 0 15px;
  }
  
  </style>
    