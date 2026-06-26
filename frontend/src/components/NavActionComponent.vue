<script>
import { useAuthStore } from '@/stores/auth'
import { useSearchStore } from '@/stores/search.js'

export default {
  name: 'NavActionComponent',
  data() {
    return {
      authStore: useAuthStore(),
      searchStore: useSearchStore(),
    }
  },
  computed: {
    isAuthenticated() {
      return this.authStore.isAuthenticated
    },
    loginLink() {
      return { path: '/login', query: { redirect: this.$route.fullPath } }
    },
    searchQuery: {
      get() {
        return this.searchStore.searchQuery
      },
      set(value) {
        this.searchStore.searchQuery = value
      },
    },
  },
  methods: {
    logout() {
      this.authStore.logout()
      this.$router.push('/home')
    },
  },
}
</script>

<template>
  <div class="d-flex align-items-center gap-3">
    <div class="header-search-wrap">
      <input v-model="searchQuery" type="search" class="form-control header-search-input" placeholder="Search memes..." aria-label="Search" />
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
