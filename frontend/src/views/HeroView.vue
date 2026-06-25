<script setup>
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import FooterComponent from '@/components/FooterComponent.vue'
import HeroComponent from '@/components/HeroComponent.vue'
import NavComponent from '@/components/NavComponent.vue'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const isAuthenticated = computed(() => authStore.isAuthenticated)
const loginLink = computed(() => ({
  path: '/login',
  query: { redirect: route.fullPath },
}))

function logout() {
  authStore.logout()
  router.push('/home')
}
</script>

<template>
  <NavComponent>
    <template #action>
      <router-link v-if="!isAuthenticated" class="btn btn-meme flex-shrink-0" :to="loginLink">
        Log in
      </router-link>
      <div v-else class="d-none d-lg-flex align-items-center gap-2 flex-shrink-0">
        <router-link class="btn btn-meme d-inline-flex align-items-center" to="/addMeme">
          <i class="fa-solid fa-plus me-2"></i>Add Meme
        </router-link>
        <button type="button" class="btn btn-meme d-inline-flex align-items-center" @click="logout">
          <i class="fa-solid fa-arrow-right-from-bracket me-2"></i>Log out
        </button>
      </div>
    </template>
  </NavComponent>
  <main class="flex-grow-1 min-w-0">
    <HeroComponent />
  </main>
  <FooterComponent />
</template>
