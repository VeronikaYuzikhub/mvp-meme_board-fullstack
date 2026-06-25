import api from '@/api/axios.js'
import { http, getHttpErrorMessage } from '@/interceptors/http.js'

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
      const response = await api.get('/memes')
      this.memes = response.data
      this.loading = false
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
        this.memes = this.memes.filter(m => m.id !== id)
      } catch (error) {
        console.error('Failed to delete meme:', error)
        alert(getHttpErrorMessage(error, 'Failed to delete meme'))
      } finally {
        this.deletingId = null
      }
    },
  },
}