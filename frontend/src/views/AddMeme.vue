<script setup>
import FooterComponent from '@/components/FooterComponent.vue'
import AddMemeComponent from '@/components/AddMemeComponent.vue'
import NavComponent from '@/components/NavComponent.vue'
</script>

<template>
  <NavComponent />
  <main class="flex-grow-1 min-w-0">
    <AddMemeComponent />
  </main>
  <FooterComponent />
</template>
