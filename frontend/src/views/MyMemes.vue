<script setup>
import FooterComponent from '@/components/FooterComponent.vue';
import HeaderComponent from '@/components/HeaderComponent.vue';
import MyMemesComponent from '@/components/MyMemesComponent.vue';
import NavComponent from '@/components/NavComponent.vue';

</script>

<template>
    <NavComponent />
    <HeaderComponent />
    <main class="flex-grow-1">
        <MyMemesComponent />
    </main>
    <FooterComponent />
</template>

<style scoped>

</style>
