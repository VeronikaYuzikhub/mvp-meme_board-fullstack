<script setup>
import FooterComponent from '@/components/FooterComponent.vue'
import MyMemesComponent from '@/components/MyMemesComponent.vue'
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
    <MyMemesComponent />
  </main>
  <FooterComponent />
</template>
