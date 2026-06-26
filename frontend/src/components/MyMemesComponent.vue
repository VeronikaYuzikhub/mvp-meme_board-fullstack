<script>
import SidebarComponent from '@/components/SidebarComponent.vue'
import useMeme from '@/composables/useMeme.js'
import useSearch from '@/composables/useSearch.js'
import { useAuthStore } from '@/stores/auth'
import MemeFeedComponent from '@/components/MemeFeedComponent.vue'

export default {
  mixins: [useMeme],
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
  components: { SidebarComponent, MemeFeedComponent },
}
</script>

<template>
  <section class="p-5">
    <div class="container-fluid position-relative">
      <div class="row g-4">
        <SidebarComponent />
        <section class="col ps-lg-5">
          <div v-if="!isAuthenticated" class="text-center mt-5">
            <p class="text-muted mb-2">
              Well.. you need to log in, what did you think?
              <i class="fa-regular fa-face-grin-beam-sweat" style="color: rgb(177, 151, 252);"></i>
            </p>
            <i class="fa-solid fa-arrow-up-right fs-4" style="color: var(--brand-purple);"></i>
          </div>

          <div v-else class="row g-3 mt-3">
            <h3 class="feed-title d-flex align-items-center gap-2 mb-0 text-dark">
              My memes
              <i class="fa-solid fa-burst" style="color: rgb(177, 151, 252);"></i>
            </h3>
            <MemeFeedComponent :memes="memes" :loading="loading" />
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

.feed-title-icon {
  font-size: 0.95rem;
  color: var(--brand-purple);
}

.meme-card:hover .meme-delete-btn {
  opacity: 1;
  pointer-events: auto;
}

.meme-delete-btn {
  position: absolute;
  top: 8px;
  right: 8px;
  z-index: 2;
  width: 32px;
  height: 32px;
  padding: 0;
  border: none;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(255, 255, 255, 0.92);
  color: #6b7280;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.12);
  opacity: 0;
  pointer-events: none;
  transition: opacity 0.2s ease, background-color 0.2s ease, color 0.2s ease;
}

.meme-delete-btn:hover:not(:disabled) {
  background: #fff;
  color: #dc3545;
}

.meme-delete-btn:disabled {
  opacity: 0.7;
  pointer-events: none;
}
</style>
