<script setup>
import FooterComponent from '@/components/FooterComponent.vue'
import HomeComponent from '@/components/HomeComponent.vue'
import NavComponent from '@/components/NavComponent.vue'
</script>

<template>
  <NavComponent />
  <main class="flex-grow-1">
    <HomeComponent />
  </main>
  <FooterComponent />
</template>

<style scoped>
</style>
