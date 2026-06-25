<script>
import NavComponent from '@/components/NavComponent.vue'
import FooterComponent from '@/components/FooterComponent.vue'
import RegisterComponent from '@/components/RegisterComponent.vue'

export default {
  name: 'RegisterView',
  components: {
    NavComponent,
    FooterComponent,
    RegisterComponent,
  },
}
</script>

<template>
  <NavComponent />
  <main class="flex-grow-1">
    <RegisterComponent />
  </main>
  <FooterComponent />
</template>
