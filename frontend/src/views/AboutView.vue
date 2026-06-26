<script setup>
import AboutComponent from '@/components/AboutComponent.vue'
import FooterComponent from '@/components/FooterComponent.vue'
import NavActionComponent from '@/components/NavActionComponent.vue'
import NavComponent from '@/components/NavComponent.vue'
</script>

<template>
  <NavComponent>
    <template #action>
      <NavActionComponent />
    </template>
  </NavComponent>
  <main class="flex-grow-1">
    <AboutComponent />
  </main>
  <FooterComponent />
</template>
<style scoped>
</style>
