import api from '@/api/axios.js'
import { http, getHttpErrorMessage } from '@/interceptors/http.js'
import { useSearchStore } from '@/stores/search'
import { useCategoryStore } from '@/stores/category'

export default {
  data() {
    return {
      memes: [],
      loading: true,
      deletingId: null,
    }
  },
  async mounted() {
    await this.getMemes()
  },
  methods: {
    async getMemes() {
      this.loading = true

      const title = useSearchStore().searchQuery
      const category = useCategoryStore().categoryQuery

      try {
        const params = {}
        if (title.trim()) params.Title = title.trim()
        if (category.trim()) params.category = category.trim()

        const response = await api.get('/memes', { params })
        this.memes = response.data
      } finally {
        this.loading = false
      }
    },
    async postMemes() {
      const response = await api.post('/memes')
      this.memes = response.data
      this.loading = false
    },
    async deleteMeme(id) {
      if (this.deletingId) return

      this.deletingId = id

      try {
        await http.delete(`/memes/${id}`)
        this.memes = this.memes.filter((m) => m.id !== id)
      } catch (error) {
        console.error('Failed to delete meme:', error)
        alert(getHttpErrorMessage(error, 'Failed to delete meme'))
      } finally {
        this.deletingId = null
      }
    },
    async getMyMemes() {
      this.loading = true
      try {
        const response = await http.get('/memes', { params: { mine: true } })
        this.memes = response.data
      } finally {
        this.loading = false
      }
    },
  },
}
