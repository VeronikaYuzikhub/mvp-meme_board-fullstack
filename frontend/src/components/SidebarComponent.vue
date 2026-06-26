<script>
import useCategories from '@/composables/useCategories.js'
import { useCategoryStore } from '@/stores/category'

export default {
  mixins: [useCategories],
  data() {
    return { categoryStore: useCategoryStore() }
  },
  methods: {
    selectAll() {
      this.categoryStore.categoryQuery = ''
    },
    selectCategory(name) {
      this.categoryStore.categoryQuery = name
    },
  },
}
</script>

<template>
    <aside class="col-lg-2">
        <h6 class="mb-3 fw-semibold">Categories</h6>
        <p v-if="loading" class="text-muted small">Loading...</p>
        <div v-else class="list-group list-group-flush rounded-3 border shadow-sm">
            <button
                type="button"
                class="list-group-item list-group-item-action border-0 py-2 px- small category-btn d-flex align-items-center"
                :class="{ active: !categoryStore.categoryQuery }"
                @click="selectAll">
                <i class="fa-solid fa-border-all category-icon me-3"></i>
                <span>All</span>
            </button>
            <button
                v-for="cat in categories"
                :key="cat.id"
                type="button"
                @click="selectCategory(cat.name)"
                class="list-group-item list-group-item-action border-0 py-2 px- small category-btn d-flex align-items-center"
                :class="{ active: categoryStore.categoryQuery === cat.name }">
                <i v-if="cat.icon" class="fa-regular category-icon me-3" :class="cat.icon"></i>
                <span>{{ cat.name }}</span>
            </button>
        </div>
    </aside>
</template>

<style scoped>
.category-btn {
  padding: 0.35rem 0.55rem;
  font-size: 0.8rem;
}

.list-group-item.active {
  background-color: rgba(var(--bs-primary-rgb), 0.12);
  color: var(--brand-purple);
  border-color: transparent;
}

.list-group-item.active .category-icon {
  color: var(--brand-purple);
}
</style>
