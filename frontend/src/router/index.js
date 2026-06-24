import { createRouter, createWebHistory } from 'vue-router'

import HeroView from '@/views/HeroView.vue'
import HomeView from '@/views/HomeView.vue'
import AboutView from '@/views/AboutView.vue'
import MyMemesView from '@/views/MyMemes.vue'
import AddMemeView from '@/views/AddMeme.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'hero',
      component: HeroView,
    },
    {
      path: '/home',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/about',
      name: 'about',
      component: AboutView,
    },
    {
      path: '/myMemes',
      name: 'myMemes',
      component: MyMemesView,
    },
    {
      path: '/addMeme',
      name: 'addMeme',
      component: AddMemeView,
    }
  ],
})

export default router
