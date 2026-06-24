<script>
import useCategories from '@/composables/useCategories.js'

export default {
  mixins: [useCategories],
  data() {
    return {
      selectedCategoryId: '',
    }
  },
}
</script>

<template>
    <section class="px-5 pt-5 pb-5 text-center">
        <div class="container">
            <div class="mb-4 text-start page-intro">
                <h4 class="page-title">Create a New Meme</h4>
                <p class="page-subtitle">Share your creativity with the world</p>
            </div>
            <div class="row g-4 align-items-stretch">
                <div class="col-lg-5 d-flex">
                    <article class="card add-meme-card p-4 text-start h-100 w-100 d-flex flex-column">
                        <h6>Upload Image</h6>

                        <div class="upload-zone border border-2 border-dashed rounded-3
                                    d-flex flex-column align-items-center justify-content-center
                                    text-center p-4 mt-3 flex-grow-1">
                            <div class="upload-icon-wrap">
                                <i class="fa-solid fa-cloud-arrow-up"></i>
                            </div>
                            <p class="mb-1"><span class="fw-bold mb-0 hero-accent">Click to upload</span> or drag and drop</p>
                            <small class="text-muted">PNG, JPG, GIF up to 5MB</small>
                        </div>
                    </article>
                </div>
                <div class="col-lg-7 d-flex">
                    <article class="card add-meme-card p-4 text-start h-100 w-100 d-flex flex-column">
                        <label for="titleInput" class="form-label required-label">Title</label>
                        <input type="text" class="form-control w-100 mb-3" id="titleInput" placeholder="Enter item title here..." required>

                        <label for="descriptionInput" class="form-label">Description</label>
                        <textarea class="form-control w-100 mb-3" id="descriptionInput" rows="3" placeholder="Add a description (optional)"></textarea>

                        <label for="categorySelect" class="form-label required-label">Category</label>
                        <select
                            id = "categorySelect"
                            class = "form-select w-100 mb-3"
                            required
                            v-model="selectedCategoryId"
                            :disabled="loading">

                            <option disabled value="">Select a category</option>
                            <option
                                v-for="cat in categories"
                                :key="cat.id"
                                :value="cat.id">
                                {{cat.name}}
                            </option>
                        </select>
                        <button type="button" class="btn btn-meme w-100 mt-auto">Publish Meme</button>
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
</style>
