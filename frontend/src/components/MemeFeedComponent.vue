<script>
export default {
  props: {
    memes: Array,
    loading: Boolean,
    showDelete: { type: Boolean, default: false },
    deletingId: { type: Number, default: null },
    likingId: { type: Number, default: null },
  },
  emits: ['delete', 'like'],
  methods: {
    categoryStyle(name) {
      const hue = (name.length * 45 + name.charCodeAt(0) * 7) % 360
      return {
        backgroundColor: `hsl(${hue}, 75%, 85%)`,
        color: `hsl(${hue}, 60%, 25%)`,
      }
    },
  },
}
</script>

<template>
  <div class="row g-3 mt-3">
    <p v-if="loading" class="text-muted small">Loading...</p>
    <div v-else v-for="meme in memes" :key="meme.id" class="col-xl-3 col-12 col-sm-6 col-lg-4">
      <article class="rounded-3 app-mockup shadow meme-card">
        <div class="mockup-card_img rounded-2" :style="meme.imageBase64 ? {
                                                                    backgroundImage: `url(data:${meme.imageContentType};base64,${meme.imageBase64})`,
                                                                    backgroundSize: 'cover',
                                                                    backgroundPosition: 'center',} : {}">
          <button v-if="showDelete" type="button" class="meme-delete-btn" aria-label="Delete meme"
            :disabled="deletingId === meme.id"
            @click="$emit('delete', meme.id)">
            <i class="fa-regular fa-trash-can"></i>
          </button>
          <p class="mockup-card_caption">{{ meme.title }}</p>
        </div>
        <span class="mockup-tag mb-2" :class="meme.categoryName" :style="categoryStyle(meme.categoryName)">{{ meme.categoryName }}</span>
        <div class="mockup-card_footer d-flex justify-content-between align-items-center">
          <span class="small text-muted ms-1 mb-2" @click="$emit('like', meme)">
            <i :class="meme.isLikedByMe ? 'fa-solid' : 'fa-regular'" class="fa-heart me-1"></i>
                {{ meme.likeCount }}
          </span>
          <i class="fa-solid fa-ellipsis-vertical small text-muted"></i>
        </div>
      </article>
    </div>
  </div>
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

.app-mockup {
  background: var(--hero-bg);
  border: 1px solid rgba(var(--bs-primary-rgb), 0.14);
  overflow: hidden;
}

.app-mockup_dots {
  display: flex;
  gap: 6px;
  padding: 14px 18px 0;
}

.window-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
}

.app-mockup_header {
  padding: 10px 18px 14px;
  border-color: rgba(var(--bs-primary-rgb), 0.12) !important;
}

.mockup-logo {
  color: var(--brand-purple);
  font-size: 1.1rem;
}

.mockup-search {
  background: rgb(248, 248, 250);
  border-color: rgb(238, 238, 240);
  border-radius: 7px;
  padding: 7px 14px;
  font-size: 0.72rem;
  white-space: nowrap;
}

.app-mockup_body {
  padding: 14px 18px 20px;
}

.mockup-card_img {
  position: relative;
  height: 150px;
  weight: 75px;
  margin-bottom: 8px;
  display: flex;
  align-items: flex-end;
  justify-content: center;
  padding: 6px;
  overflow: hidden;
}

.mockup-card_caption {
  margin: 0;
  font-size: 0.45rem;
  font-weight: 800;
  line-height: 1.2;
  text-align: center;
  color: #fff;
  text-shadow: 0 1px 2px rgba(0, 0, 0, 0.6);
  text-transform: uppercase;
}

.mockup-tag {
  display: inline-block;
  font-size: 0.62rem;
  padding: 2px 8px;
  margin: 2px 8px;
  border-radius: 7px;
  font-weight: 600;
}

.mockup-card_footer {
  margin-top: 8px;
  padding: 0 2px;
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
