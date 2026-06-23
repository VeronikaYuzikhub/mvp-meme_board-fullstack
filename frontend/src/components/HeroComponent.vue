<script setup>
import MockupComponent from '@/components/MockupComponent.vue'

const blobs = ['blob-left', 'blob-main', 'blob-right', 'blob-top', 'blob-bottom']
const dots = Array.from({ length: 16 }, (_, i) => i)
</script>

<template>
  <section class="hero-section">
    <div v-for="name in blobs" :key="name" class="blob" :class="name"></div>
    <div class="dot-grid" aria-hidden="true"><span v-for="dot in dots" :key="dot" class="dot"></span></div>
    <div class="container position-relative hero-content pt-5 mt-5" style="padding-top: 6rem !important;">
      <div class="row align-items-center">
        <div class="col-lg-6">
            <h1 class="display-3 hero-title fw-bold mb-0">Share memes,</h1>
            <h1 class="display-3 hero-title fw-bold mb-0 hero-accent">simply.</h1>
            <p class="text-muted lead mt-3 mb-4">
              Create, discover, and share memes with your friends and the world.
            </p>
            <div class="d-flex flex-wrap gap-3">
              <button type="button" class="btn btn-meme btn-lg d-inline-flex align-items-center">
                <i class="fa-solid fa-rocket me-2"></i>Explore memes
              </button>
              <button type="button" class="btn btn-lg hero-btn-outline d-inline-flex align-items-center">
                <svg class="hero-info-icon me-2" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" aria-hidden="true">
                  <circle cx="12" cy="12" r="9" stroke="currentColor" stroke-width="1.75"/>
                  <circle cx="12" cy="8" r="0.75" fill="currentColor"/>
                  <path d="M12 11v5" stroke="currentColor" stroke-width="1.75" stroke-linecap="round"/>
                </svg>
                Learn more
              </button>
            </div>
        </div>

        <div class="col-lg-6 mt-5 mt-lg-0">
          <MockupComponent />
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.hero-section {
  position: relative;
  min-height: 650px;
  background: var(--hero-bg);
  overflow: hidden;
}

.hero-content {
  position: relative;
  z-index: 2;
  padding-top: 7rem;
}

.hero-title {
  line-height: 1.15;
}

.hero-btn-outline {
  background: var(--hero-bg);
  border: 2px solid var(--brand-purple);
  color: var(--brand-purple);
  border-radius: 8px;
  padding: 0.6rem 1.25rem;
  font-weight: 500;
}

.hero-btn-outline:hover {
  background-color: var(--brand-purple);
  border-color: var(--brand-purple);
  color: var(--hero-bg);
}

.hero-info-icon {
  width: 1.1rem;
  height: 1.1rem;
  flex-shrink: 0;
}

.blob {
  position: absolute;
  border-radius: 50%;
  pointer-events: none;
  z-index: 0;
}

.blob-left {
  width: 620px;
  height: 620px;
  top: 10%;
  left: -12%;
  background: radial-gradient(
    circle,
    rgba(var(--bs-primary-rgb), 0.1) 0%,
    rgba(var(--bs-primary-rgb), 0.04) 50%,
    rgba(var(--bs-primary-rgb), 0) 72%
  );
}

.blob-main {
  width: 720px;
  height: 580px;
  top: 8%;
  right: 6%;
  border-radius: 48% 52% 50% 50%;
  background: radial-gradient(
    ellipse at center,
    rgba(var(--bs-primary-rgb), 0.16) 0%,
    rgba(var(--bs-primary-rgb), 0.07) 48%,
    rgba(var(--bs-primary-rgb), 0) 74%
  );
}

.blob-right {
  width: 360px;
  height: 360px;
  top: 38%;
  right: -14%;
  background: radial-gradient(
    circle,
    rgba(var(--bs-primary-rgb), 0.12) 0%,
    rgba(var(--bs-primary-rgb), 0.04) 55%,
    rgba(var(--bs-primary-rgb), 0) 75%
  );
}

.blob-top {
  width: 200px;
  height: 200px;
  top: 6%;
  left: 46%;
  background: radial-gradient(
    circle,
    rgba(var(--bs-primary-rgb), 0.1) 0%,
    rgba(var(--bs-primary-rgb), 0) 70%
  );
}

.blob-bottom {
  width: 420px;
  height: 420px;
  bottom: 14%;
  right: 4%;
  background: radial-gradient(
    circle,
    rgba(var(--bs-primary-rgb), 0.09) 0%,
    rgba(var(--bs-primary-rgb), 0.03) 50%,
    rgba(var(--bs-primary-rgb), 0) 72%
  );
}

.dot-grid {
  position: absolute;
  right: 140px;
  bottom: 80px;
  z-index: 1;
  display: grid;
  grid-template-columns: repeat(4, 6px);
  grid-template-rows: repeat(4, 6px);
  gap: 11px;
  pointer-events: none;
}

.dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: rgba(var(--bs-primary-rgb), var(--hero-dot-opacity));
}
</style>
