<script setup>
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

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
  <div class="d-flex align-items-center gap-3">
    <div class="header-search-wrap">
      <input
        type="search"
        class="form-control header-search-input"
        placeholder="Search memes..."
        aria-label="Search"
      />
      <i class="fa-solid fa-magnifying-glass header-search-icon" aria-hidden="true"></i>
    </div>
    <router-link v-if="!isAuthenticated" class="btn btn-meme flex-shrink-0 d-none d-lg-inline-flex align-items-center" :to="loginLink">
      Log in
    </router-link>
    <div v-else class="d-flex align-items-center gap-2 flex-shrink-0">
      <router-link class="btn btn-meme d-none d-lg-inline-flex align-items-center" to="/addMeme">
        <i class="fa-solid fa-plus me-2"></i>Add Meme
      </router-link>
      <button type="button" class="btn btn-meme d-none d-lg-inline-flex align-items-center" @click="logout">
        <i class="fa-solid fa-arrow-right-from-bracket me-2"></i>Log out
      </button>
    </div>
  </div>
</template>
