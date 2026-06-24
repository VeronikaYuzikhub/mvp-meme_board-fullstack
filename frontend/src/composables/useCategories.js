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
      const response = await api.get('/categories')
      this.categories = response.data
      this.loading = false
    },
  },
}