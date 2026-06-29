import api from '@/api/axios.js'

export default {
  data() {
    return {
      categories: [],
      loading: true,
    }
  },
  async mounted() {
    await this.loadCategories()
  },
  methods: {
    async loadCategories() {
      try {
        const response = await api.get('/categories')
        this.categories = response.data
      } catch (error) {
        console.error('Failed to load categories:', error)
      } finally {
        this.loading = false
      }
    },
  },
}
