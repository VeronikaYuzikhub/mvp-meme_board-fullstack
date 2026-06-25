<script>
import NavComponent from '@/components/NavComponent.vue'
import FooterComponent from '@/components/FooterComponent.vue'
import LoginComponent from '@/components/LoginComponent.vue'

export default {
  name: 'LoginView',
  components: {
    NavComponent,
    FooterComponent,
    LoginComponent,
  },
}
</script>

<template>
  <NavComponent />
  <main class="flex-grow-1">
    <LoginComponent />
  </main>
  <FooterComponent />
</template>
