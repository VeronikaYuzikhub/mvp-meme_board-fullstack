<script setup>
import { ref } from 'vue'
import catImg from '@/assets/images/about-cat.png'

const currentStep = ref(1)

const steps = [
  {
    id: 1,
    label: 'Browse the feed',
    text: 'Open Home and scroll through memes. No complicated layout — just a grid and categories on the side.',
    statTitle: 'Memes on feed',
    statDesc: 'Grid on Home — grows when API is connected.',
    icon: 'fa-images',
  },
  {
    id: 2,
    label: 'Pick a category',
    text: 'Categories load from the API. Click one in the sidebar to filter what you want to see.',
    statTitle: 'Categories from API',
    statDesc: 'Sidebar loads real categories from the backend.',
    icon: 'fa-folder',
  },
  {
    id: 3,
    label: 'Add your meme',
    text: 'Go to Add Meme: upload an image, write a title, choose a category, and publish.',
    statTitle: 'Add meme in 3 steps',
    statDesc: 'Upload, title, category — then publish.',
    icon: 'fa-bolt',
  },
]

function goToStep(step) {
  currentStep.value = step
}

function nextStep() {
  if (currentStep.value < steps.length) currentStep.value += 1
}

function prevStep() {
  if (currentStep.value > 1) currentStep.value -= 1
}

function resetSteps() {
  currentStep.value = 1
}
</script>

<template>
  <div>
    <section class="container py-5">
      <div class="col-lg-8" data-aos="fade-right">
        <h1 class="display-4 fw-bold text-dark mb-3">
          Built for meme lovers,<br />
          <span class="hero-accent">not for the algorithm.</span>
        </h1>
        <p class="text-muted lead mb-2">
          Meme Board is a fullstack practice project built to explore web dev, share laughs, and keep meme culture alive.
        </p>
        <svg class="about-squiggle" viewBox="0 0 220 24" aria-hidden="true">
          <path
            d="M4 14 C 40 4, 80 22, 120 12 S 200 8, 216 16"
            fill="none"
            stroke="var(--brand-purple)"
            stroke-width="3"
            stroke-linecap="round" />
        </svg>
      </div>
      <p class="text-muted small mt-5 mb-0">Scroll to explore ↓</p>
    </section>

    <section class="container py-5">
      <div class="row align-items-center g-4">
        <div class="col-lg-6" data-aos="fade-right">
          <h2 class="fw-bold mb-3">Make meme sharing simple again</h2>
          <p class="text-muted mb-0">
            Meme Board strips away the noise. No feeds you don't need, no trends we force.
            Just browse, post, laugh, repeat. Because memes hit different when it's real.
          </p>
        </div>
        <div class="col-lg-6 position-relative text-center text-lg-end">
          <img
            :src="catImg"
            alt="Cat with sunglasses"
            class="about-cat-img img-fluid"
            data-aos="zoom-in-up"
            data-aos-duration="1000"
            data-aos-easing="ease-out-cubic" />
          <div
            class="about-speech-bubble"
            data-aos="fade-left"
            data-aos-delay="300"
            data-aos-duration="800">
            keep it funny
          </div>
        </div>
      </div>
    </section>

    <section class="container pb-5">
      <div class="card add-meme-card border-0 p-4" data-aos="fade-up" data-aos-delay="100">
        <div class="about-stepper d-flex align-items-center mb-4">
          <template v-for="(step, index) in steps" :key="step.id">
            <div class="d-flex align-items-center gap-2">
              <button
                type="button"
                class="about-step-btn"
                :class="{ active: currentStep === step.id }"
                @click="goToStep(step.id)">
                {{ step.id }}
              </button>
              <span class="small" :class="currentStep === step.id ? 'fw-semibold text-dark' : 'text-muted'">
                {{ step.label }}
              </span>
            </div>
            <div v-if="index < steps.length - 1" class="about-step-line flex-grow-1 mx-3"></div>
          </template>
        </div>

        <p class="text-muted mb-4">
          {{ steps.find((s) => s.id === currentStep)?.text }}
        </p>

        <div class="d-flex justify-content-between align-items-center">
          <div class="d-flex gap-3">
            <button type="button" class="btn btn-link btn-sm text-muted text-decoration-none p-0" @click="resetSteps">
              RESET
            </button>
            <button
              type="button"
              class="btn btn-link btn-sm text-muted text-decoration-none p-0"
              :disabled="currentStep === 1"
              @click="prevStep">
              PREVIOUS
            </button>
          </div>
          <button
            type="button"
            class="btn btn-link btn-sm hero-accent text-decoration-none p-0 fw-semibold"
            :disabled="currentStep === steps.length"
            @click="nextStep">
            NEXT <i class="fa-solid fa-chevron-right ms-1"></i>
          </button>
        </div>
      </div>
    </section>

    <section class="container py-5">
      <div class="row align-items-center justify-content-center g-6 about-duo-row mx-auto">
        <div class="col-lg-5" data-aos="fade-right">
          <p class="text-uppercase small hero-accent fw-semibold mb-2">Not like the others</p>
          <h2 class="fw-bold mb-3">
            No trends.<br />
            No pressure.<br />
            <span class="hero-accent">Just memes.</span>
          </h2>
          <ul class="text-muted mb-0 mb-lg-4">
            <li>No chasing virality</li>
            <li>Simple feed and categories</li>
            <li>Upload, laugh, repeat</li>
          </ul>
        </div>
        <div class="col-lg-4">
          <div class="about-author-card add-meme-card p-4" data-aos="fade-up">
            <div class="d-flex justify-content-center">
              <div class="d-flex flex-column align-items-center text-center about-author">
                <div class="about-avatar">V</div>
                <div class="about-author-info">
                  <h3 class="h5 fw-bold mb-2">Veronika</h3>
                  <p class="text-muted mb-3">Author this project</p>
                </div>
                <a
                  class="btn btn-meme"
                  href="https://www.linkedin.com/in/veronika-yuzik-9292b735b"
                  target="_blank"
                  rel="noopener noreferrer">
                  LinkedIn
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.about-squiggle {
  width: 200px;
  height: 24px;
}

.about-cat-img {
  max-width: 420px;
}

.about-speech-bubble {
  position: absolute;
  top: 0;
  right: 0;
  padding: 0.4rem 0.85rem;
  border: 1px solid rgba(var(--bs-primary-rgb), 0.22);
  border-radius: 12px;
  box-shadow: 0 4px 16px rgba(var(--bs-primary-rgb), 0.14);
  font-size: 0.82rem;
  font-weight: 600;
  color: var(--brand-purple);
  background: #fff;
}

.about-bubble {
  display: inline-block;
  padding: 0.5rem 1rem;
  border: 1px solid rgba(var(--bs-primary-rgb), 0.2);
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(var(--bs-primary-rgb), 0.1);
  font-size: 0.85rem;
  color: #4b5563;
  background: #fff;
}

.about-stats {
  background: rgba(var(--bs-primary-rgb), 0.07);
}

.about-stat-btn {
  border: none;
  background: transparent;
}

.about-stat-btn.active {
  background: rgba(var(--bs-primary-rgb), 0.12);
  border-radius: 8px;
}

.about-stepper {
  flex-wrap: wrap;
  gap: 0.5rem;
}

.about-step-line {
  height: 2px;
  background: #d1d5db;
  min-width: 24px;
}

.about-step-btn {
  width: 2rem;
  height: 2rem;
  border-radius: 50%;
  border: none;
  background: #d1d5db;
  color: #fff;
  font-weight: 600;
  font-size: 0.85rem;
  flex-shrink: 0;
}

.about-step-btn.active {
  background: var(--brand-purple);
}

.about-bubble-2 {
  margin-left: 2rem;
}

.about-avatar {
  width: 72px;
  height: 72px;
  border-radius: 50%;
  background: rgba(var(--bs-primary-rgb), 0.15);
  color: var(--brand-purple);
  font-weight: 700;
  font-size: 1.5rem;
  line-height: 72px;
  text-align: center;
  flex-shrink: 0;
}

.about-author-info {
  margin-top: 1rem;
  padding-top: 0.25rem;
}

.about-duo-row {
  max-width: 980px;
}

.add-meme-card {
  border: 1px solid rgba(var(--bs-primary-rgb), 0.1);
  box-shadow: 0 1px 6px rgba(0, 0, 0, 0.04);
}
</style>
