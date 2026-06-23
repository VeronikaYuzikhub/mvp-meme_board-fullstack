<script setup>
import HeaderComponent from '@/components/HeaderComponent.vue';
import FooterComponent from '@/components/FooterComponent.vue';
import NavComponent from '@/components/NavComponent.vue';

</script>

<template>
  <NavComponent />
  <HeaderComponent />
  <main class="flex-grow-1">
    <section class="p-5 text-center">
      <div class="container">
        <h1>My Memes</h1>
      </div>
    </section>
  </main>
  <FooterComponent />
</template>

<style scoped>
</style>
