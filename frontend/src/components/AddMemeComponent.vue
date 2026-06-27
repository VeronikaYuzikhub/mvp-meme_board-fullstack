<script>
import { http, getHttpErrorMessage } from '@/interceptors/http.js'
import useCategories from '@/composables/useCategories.js'

const MAX_FILE_SIZE = 5 * 1024 * 1024
const ALLOWED_TYPES = ['image/png', 'image/jpeg', 'image/gif']
const MAX_TITLE_LENGTH = 100
const MAX_DESCRIPTION_LENGTH = 200

export default {
  mixins: [useCategories],
  data() {
    return {
      MAX_TITLE_LENGTH,
      MAX_DESCRIPTION_LENGTH,
      form: {
        selectedCategoryId: '',
        title: '',
        description: '',
        imageBase64: '',
        imageContentType: '',
      },
      imagePreview: '',
      isSubmitting: false,
      formError: '',
      formSuccess: '',
      editId: null,
      savedImageUrl: '',
    }
  },
  async mounted() {
    await this.editFile()
  },
  methods: {
    clearMessages() {
      this.formError = ''
      this.formSuccess = ''
    },
    openFilePicker() {
      this.$refs.fileInput.click()
    },
    onFileSelected(event) {
      const file = event.target.files?.[0]
      if (file) {
        this.processFile(file)
      }
    },
    onDrop(event) {
      const file = event.dataTransfer.files?.[0]
      if (file) {
        this.processFile(file)
      }
    },
    processFile(file) {
      if (!ALLOWED_TYPES.includes(file.type)) {
        this.formSuccess = ''
        this.formError = 'Only PNG, JPG and GIF images are allowed.'
        return
      }

      if (file.size > MAX_FILE_SIZE) {
        this.formSuccess = ''
        this.formError = 'Image must be 5MB or smaller.'
        return
      }

      const reader = new FileReader()
      reader.onload = () => {
        const [, base64] = reader.result.split(',')
        this.form.imageContentType = file.type
        this.form.imageBase64 = base64
        this.imagePreview = reader.result
        this.formError = ''
      }
      reader.readAsDataURL(file)
    },
    resetForm() {
      this.form.selectedCategoryId = ''
      this.form.title = ''
      this.form.description = ''
      this.form.imageBase64 = ''
      this.form.imageContentType = ''
      this.imagePreview = ''

      if (this.$refs.fileInput) {
        this.$refs.fileInput.value = ''
      }
    },
    async submitPost() {
      if (this.isSubmitting) return

      this.formSuccess = ''

      if (!this.editId && !this.form.imageBase64) {
        this.formError = 'Please upload an image'
        return
      }
      if (!this.form.title.trim()) {
        this.formError = 'Title is required'
        return
      }
      if (this.form.title.trim().length > MAX_TITLE_LENGTH) {
        this.formError = `Title must be ${MAX_TITLE_LENGTH} characters or less`
        return
      }
      if (this.form.description.trim().length > MAX_DESCRIPTION_LENGTH) {
        this.formError = `Description must be ${MAX_DESCRIPTION_LENGTH} characters or less`
        return
      }
      if (!this.form.selectedCategoryId) {
        this.formError = 'Please select a category'
        return
      }

      this.formError = ''
      this.isSubmitting = true

      try {
        if (this.editId) {
          const payload = {
            title: this.form.title.trim(),
            description: this.form.description.trim() || null,
            categoryId: Number(this.form.selectedCategoryId),
            imageUrl: this.savedImageUrl,
          }
          if (this.form.imageBase64) {
            payload.imageBase64 = this.form.imageBase64
            payload.imageContentType = this.form.imageContentType
          }
          await http.put(`/memes/${this.editId}`, payload)
          this.$router.push('/myMemes')
        } else {
          await http.post('/memes', {
          title: this.form.title.trim(),
          description: this.form.description.trim() || null,
          imageBase64: this.form.imageBase64,
          imageContentType: this.form.imageContentType,
          categoryId: Number(this.form.selectedCategoryId),
        })
          this.formSuccess = 'Meme successfully published!'
          this.resetForm()
        }
      } catch (error) {
        this.formError = getHttpErrorMessage(error, 'Failed to send data')
      } finally {
        this.isSubmitting = false
      }
    },
    async editFile() {
      const id = this.$route.query.edit
      if (!id) return

      this.editId = Number(id)
      const {data} = await http.get(`/memes/${this.editId}`) 

      while (this.loading) {
        await new Promise(r => setTimeout(r, 50))
      }

      this.form.title = data.title
      this.form.description = data.description || ''
      this.form.selectedCategoryId = this.categories.find(c => c.name === data.categoryName)?.id || ''

      if (data.imageBase64) {
        this.imagePreview = `data:${data.imageContentType};base64,${data.imageBase64}`
      } else if (data.imageUrl) {
        this.imagePreview = data.imageUrl
      }
      this.savedImageUrl = data.imageUrl || ''
    }
  },
}
</script>

<template>
  <section class="px-5 pt-5 pb-5 text-center">
    <div class="container">
      <div class="mb-4 text-start page-intro">
        <h4 class="page-title">{{ editId ? 'Edit Meme' : 'Create a New Meme' }}</h4>
        <p class="page-subtitle">{{ editId ? 'Update your meme' : 'Share your creativity with the world' }}</p>
      </div>
      <div class="row g-4 align-items-stretch">
        <div class="col-lg-5 d-flex">
          <article class="card add-meme-card p-4 text-start h-100 w-100 d-flex flex-column">
            <h6 class="required-label mb-0">Upload Image</h6>
            <input ref="fileInput" type="file" accept="image/png,image/jpeg,image/gif" class="d-none" @change="onFileSelected" />
            <div class="upload-zone border border-2 border-dashed rounded-3
                     d-flex flex-column align-items-center justify-content-center
                     text-center p-4 mt-3 flex-grow-1" :class="{ 'upload-zone-error': formError === 'Please upload an image' }" role="button" @click="openFilePicker" @dragover.prevent @drop.prevent="onDrop">
              <img v-if="imagePreview" :src="imagePreview"  alt="Selected meme preview"  class="upload-preview mb-3"/>
              <div v-else class="upload-icon-wrap">
                <i class="fa-solid fa-cloud-arrow-up"></i>
              </div>
              <p class="mb-1">
                <span class="fw-bold mb-0 hero-accent">Click to upload</span> or drag and drop
              </p>
              <small class="text-muted">PNG, JPG, GIF up to 5MB</small>
            </div>
          </article>
        </div>
        <div class="col-lg-7 d-flex">
          <article class="card add-meme-card p-4 text-start h-100 w-100 d-flex flex-column">
            <form @submit.prevent="submitPost" class="container py-5">
              <label for="titleInput" class="form-label required-label">Title</label>
              <input v-model="form.title" type="text" class="form-control w-100 mb-1" id="titleInput" placeholder="Enter item title here..." :maxlength="MAX_TITLE_LENGTH" required @input="clearMessages" />
              <small class="text-muted d-block mb-3">{{ form.title.length }}/{{ MAX_TITLE_LENGTH }}</small>

              <label for="descriptionInput" class="form-label">Description</label>
              <textarea v-model="form.description" class="form-control w-100 mb-1" id="descriptionInput" rows="3" placeholder="Add a description (optional)" :maxlength="MAX_DESCRIPTION_LENGTH" @input="clearMessages"></textarea>
              <small class="text-muted d-block mb-3">{{ form.description.length }}/{{ MAX_DESCRIPTION_LENGTH }}</small>

              <label for="categorySelect" class="form-label required-label">Category</label>
              <select id="categorySelect" class="form-select w-100 mb-3" required v-model="form.selectedCategoryId" :disabled="loading" @change="clearMessages">
                <option disabled value="">Select a category</option>
                <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                  {{ cat.name }}
                </option>
              </select>
              <p v-if="formError" class="text-danger small mb-2">{{ formError }}</p>
              <p v-if="formSuccess" class="text-success small mb-2">{{ formSuccess }}</p>
              <button type="submit" class="btn btn-primary btn-meme w-100 mt-auto" :disabled="isSubmitting">
                {{ isSubmitting ? (editId ? 'Saving...' : 'Publication...') : (editId ? 'Save changes' : 'Publish Meme') }}
              </button>
            </form>
          </article>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.page-title {
  margin-bottom: 0.25rem;
}

.page-subtitle {
  margin-bottom: 0;
  color: #6b7280;
}

.add-meme-card {
  border: 1px solid rgba(var(--bs-primary-rgb), 0.1);
  box-shadow: 0 1px 6px rgba(0, 0, 0, 0.04);
}

.upload-zone {
  background: rgba(var(--bs-primary-rgb), 0.07);
  border-color: rgba(var(--bs-primary-rgb), 0.22) !important;
  box-shadow: 0 0 28px rgba(var(--bs-primary-rgb), 0.12);
  min-height: 220px;
  cursor: pointer;
}

.upload-preview {
  max-width: 100%;
  max-height: 180px;
  object-fit: contain;
  border-radius: 0.5rem;
}

.upload-icon-wrap {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  background: rgba(var(--bs-primary-rgb), 0.14);
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--brand-purple);
  margin-bottom: 0.75rem;
  font-size: 1.15rem;
}

.required-label::after {
  content: ' *';
  color: #dc3545;
}

.upload-zone-error {
  border-color: #dc3545 !important;
}
</style>
