<script setup>
import FooterComponent from '@/components/FooterComponent.vue'
import HomeComponent from '@/components/HomeComponent.vue'
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
    <HomeComponent />
  </main>
  <FooterComponent />
</template>
