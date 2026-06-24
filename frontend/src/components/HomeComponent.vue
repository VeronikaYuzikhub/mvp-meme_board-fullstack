<script setup>
import SidebarComponent from '@/components/SidebarComponent.vue'
const memeCards = [
  {
    id: 1,
    caption: 'WHEN YOUR CODE WORKS ON THE FIRST TRY',
    tag: 'programming',
    tagClass: 'tag-purple',
    likes: '1.2K',
    imageClass: 'meme-img-1',
  },
  {
    id: 2,
    caption: 'ME PRETENDING TO UNDERSTAND',
    tag: 'relatable',
    tagClass: 'tag-green',
    likes: '856',
    imageClass: 'meme-img-2',
  },
  {
    id: 3,
    caption: 'ME AT 3AM FIXING A BUG',
    tag: 'funny',
    tagClass: 'tag-pink',
    likes: '2.1K',
    imageClass: 'meme-img-3',
  },
  {
    id: 4,
    caption: 'ME AT 3AM FIXING A BUG',
    tag: 'funny',
    tagClass: 'tag-pink',
    likes: '2.1K',
    imageClass: 'meme-img-3',
  },
]
</script>

<template>
    <section class="p-5">
        <div class="container-fluid position-relative">
        <div class="row g-4">
            <SidebarComponent />
            <section class="col ps-lg-5">
                <h3 class="feed-title d-flex align-items-center gap-2 mb-0 text-dark">
                  All memes
                  <i class="fa-solid fa-burst" style="color: rgb(177, 151, 252);"></i>
                </h3>
                <div class="row g-3 mt-3">
                    <div v-for="meme in memeCards" :key="meme.id" class="col-xl-3 col-12 col-sm-6 col-lg-4">
                        <article class="rounded-3 app-mockup shadow">
                            <div class="mockup-card_img rounded-2" :class="meme.imageClass">
                                <p class="mockup-card_caption">{{meme.caption}}</p>
                            </div>
                            <span class="mockup-tag mb-2 " 
                                :class="meme.tagClass" 
                                :style="{backgroundColor: `hsl(${(meme.tag.length * 45 + meme.tag.charCodeAt(0) * 7) % 360}, 75%, 85%)`,
                                color: `hsl(${(meme.tag.length * 45 + meme.tag.charCodeAt(0) * 7) % 360}, 60%, 25%)` 
                            }">{{ meme.tag }}</span>
                            <div class="mockup-card_footer d-flex justify-content-between align-items-center">
                                <span class="small text-muted ms-1 mb-2">
                                    <i class="fa-regular fa-heart me-1"></i>{{ meme.likes }}
                                </span>
                                    <i class="fa-solid fa-ellipsis-vertical small text-muted"></i>
                            </div>
                        </article>
                    </div>
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
  background:rgb(248, 248, 250);
  border-color:rgb(238, 238, 240);
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
</style>
