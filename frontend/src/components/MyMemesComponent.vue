<script>
import SidebarComponent from '@/components/SidebarComponent.vue'
import useMeme from '@/composables/useMeme.js'
import useSearch from '@/composables/useSearch.js'
import { useAuthStore } from '@/stores/auth'
import MemeFeedComponent from '@/components/MemeFeedComponent.vue'

export default {
  mixins: [useMeme, useSearch],
  data() {
    return {
      authStore: useAuthStore(),
    }
  },
  computed: {
    isAuthenticated() {
      return this.authStore.isAuthenticated
    },
  },
  async mounted() {
    if (this.isAuthenticated) await this.getMyMemes()
    else this.loading = false
  },
  methods: {
    async loadMemes() {
      if (this.isAuthenticated) await this.getMyMemes()
    },
    async goEdit(id) {
      this.$router.push({ path: '/addMeme', query: { edit: id } })
    }
  },
  components: { SidebarComponent, MemeFeedComponent },
}
</script>

<template>
  <section class="feed-section px-5 pt-4 pb-5">
    <div class="container-fluid position-relative">
      <div class="row g-4">
        <SidebarComponent />
        <section class="col ps-lg-5">
          <div v-if="!isAuthenticated" class="guest-prompt text-center">
            <p class="text-muted mb-1">
              Well.. you need to log in, what did you think?
              <i class="fa-regular fa-face-grin-beam-sweat" style="color: rgb(177, 151, 252);"></i>
            </p>
            <i class="fa-solid fa-arrow-up-right guest-arrow-icon"></i>
          </div>

          <div v-else class="row g-3 mt-3">
            <h3 class="feed-title d-flex align-items-center gap-2 mb-0 text-dark">
              My memes
              <i class="fa-solid fa-burst" style="color: rgb(177, 151, 252);"></i>
            </h3>
            <MemeFeedComponent
              :memes="memes"
              :loading="loading"
              :show-delete="true"
              :show-edit="true"
              :deleting-id="deletingId"
              :liking-id="likingId"
              @delete="deleteMeme"
              @edit="goEdit"
              @like="toggleLike" />
          </div>
        </section>
      </div>
    </div>
  </section>
</template>

<style scoped>
.feed-title {
  font-size: 1.75rem;
  font-weight: 700;
}

.guest-prompt {
  padding-top: 2rem;
  text-align: center;
}

.guest-arrow-icon {
  display: inline-block;
  font-size: 1.75rem;
  color: var(--brand-purple);
}

.feed-title-icon {
  font-size: 0.95rem;
  color: var(--brand-purple);
}
</style>
