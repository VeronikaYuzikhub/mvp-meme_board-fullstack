<script>
import SidebarComponent from '@/components/SidebarComponent.vue'
import MemeFeedComponent from '@/components/MemeFeedComponent.vue'
import useMeme from '@/composables/useMeme.js'
import useSearch from '@/composables/useSearch.js'

export default {
  mixins: [useMeme, useSearch],
  components: {SidebarComponent, MemeFeedComponent},
  async mounted() {
    await this.getMemes()
  },
}
</script>

<template>
    <section class="feed-section px-5 pt-4 pb-5">
        <div class="container-fluid position-relative">
          <div class="row g-4">
              <SidebarComponent />
              <section class="col ps-lg-5">
                  <h3 class="feed-title d-flex align-items-center gap-2 mb-0 text-dark">
                    All memes
                    <i class="fa-solid fa-burst" style="color: rgb(177, 151, 252);"></i>
                  </h3>
                  <MemeFeedComponent :memes="memes" :loading="loading" :liking-id="likingId" @like="toggleLike"/>
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
</style scoped>