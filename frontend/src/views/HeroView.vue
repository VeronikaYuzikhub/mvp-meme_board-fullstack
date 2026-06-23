<script setup>
import FooterComponent from '@/components/FooterComponent.vue'
import HeroComponent from '@/components/HeroComponent.vue'
import NavComponent from '@/components/NavComponent.vue'
</script>

<template>
  <NavComponent />
  <main class="flex-grow-1 min-w-0">
    <HeroComponent />
  </main>
  <FooterComponent />
</template>
