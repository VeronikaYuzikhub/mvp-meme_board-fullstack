<script setup>
const memeCards = [
  {
    caption: 'WHEN YOUR CODE WORKS ON THE FIRST TRY',
    tag: 'programming',
    tagClass: 'tag-purple',
    likes: '1.2K',
    imageClass: 'meme-img-1',
  },
  {
    caption: 'ME PRETENDING TO UNDERSTAND',
    tag: 'relatable',
    tagClass: 'tag-green',
    likes: '856',
    imageClass: 'meme-img-2',
  },
  {
    caption: 'ME AT 3AM FIXING A BUG',
    tag: 'funny',
    tagClass: 'tag-pink',
    likes: '2.1K',
    imageClass: 'meme-img-3',
  },
]
</script>

<template>
  <div class="app-mockup rounded-4 shadow" data-aos="flip-left"
     data-aos-easing="ease-out-cubic"
     data-aos-duration="2000">
    <div class="app-mockup_dots">
      <span class="window-dot window-dot-red"></span>
      <span class="window-dot window-dot-yellow"></span>
      <span class="window-dot window-dot-green"></span>
    </div>

    <div class="app-mockup_header border-bottom d-flex align-items-center justify-content-between gap-2">
      <div class="d-flex align-items-center gap-2">
        <i class="fa-regular fa-face-smile mockup-logo"></i>
        <span class="fw-bold small mb-0">MemeBoard</span>
      </div>
      <div class="mockup-search text-muted">
        <span class="me-5">Search memes...</span><i class="fa-solid fa-magnifying-glass me-1"></i>
      </div>
    </div>

    <div class="app-mockup_body">
      <div class="row g-2">
        <div v-for="(card, index) in memeCards" :key="index" class="col-4">
          <article class="mockup-card rounded-3 app-mockup shadow">
            <div class="mockup-card_img rounded-2" :class="card.imageClass">
              <p class="mockup-card_caption">{{ card.caption }}</p>
            </div>
            <span class="mockup-tag mb-2" :class="card.tagClass">{{ card.tag }}</span>
            <div class="mockup-card_footer d-flex justify-content-between align-items-center">
              <span class="small text-muted ms-1 mb-2">
                <i class="fa-regular fa-heart me-1"></i>{{ card.likes }}
              </span>
              <i class="fa-solid fa-ellipsis-vertical small text-muted"></i>
            </div>
          </article>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
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

.window-dot-red {
  background: #ff5f57;
}
.window-dot-yellow {
  background: #febc2e;
}
.window-dot-green {
  background: #28c840;
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

.mockup-card {
  background: var(--hero-bg);
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

.meme-img-1 {
  background: linear-gradient(160deg, #6b7280, #9ca3af);
}

.meme-img-2 {
  background: linear-gradient(160deg, #78716c, #a8a29e);
}

.meme-img-3 {
  background: linear-gradient(160deg, #57534e, #78716c);
}

.mockup-tag {
  display: inline-block;
  font-size: 0.62rem;
  padding: 2px 8px;
  margin: 2px 8px;
  border-radius: 7px;
  font-weight: 600;
}

.tag-purple {
  background: rgba(var(--bs-primary-rgb), 0.15);
  color: var(--brand-purple);
}

.tag-green {
  background: #d1fae5;
  color: #059669;
}

.tag-pink {
  background: #fce7f3;
  color: #db2777;
}

.mockup-card_footer {
  margin-top: 8px;
  padding: 0 2px;
}
</style>
