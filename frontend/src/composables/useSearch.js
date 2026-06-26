import { useSearchStore } from '@/stores/search'
import { useCategoryStore } from '@/stores/category'

export default {
  computed: {
    searchStore() {
      return useSearchStore()
    },
    categoryStore() {
      return useCategoryStore()
    },
  },
  watch: {
    'searchStore.searchQuery'() {
      this.getMemes()
    },
    'categoryStore.categoryQuery'() {
      this.getMemes()
    },
  },
}
