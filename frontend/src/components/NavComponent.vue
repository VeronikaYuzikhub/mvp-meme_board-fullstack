<script setup>

</script>

<template>
  <div class="container">
    <nav class="navbar navbar-expand-lg py-2 pt-3 pt-lg-2">
      <router-link class="navbar-brand d-flex align-items-center gap-2 mb-0 text-decoration-none text-dark flex-shrink-0" to="/">
        <i class="fa-regular fa-face-smile fs-4" style="color: var(--brand-purple);"></i>
        <span class="font-monospace fw-bold text-nowrap">Meme Board</span>
      </router-link>

      <div class="d-flex align-items-center gap-2 ms-auto d-lg-none">
        <slot name="action" />
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
      </div>

      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <ul class="navbar-nav mx-auto text-center align-items-center w-100 mb-2 mb-lg-0 me-lg-auto mx-lg-0 ms-lg-3 text-lg-start w-lg-auto">
          <li class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/home">Home</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/myMemes">MyMemes</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/about">About</router-link>
          </li>
        </ul>
        <div class="d-lg-none d-flex flex-column align-items-center gap-2 pb-3">
          <router-link class="btn btn-meme burger-btn" aria-current="page" to="/addMeme"><i class="fa-solid fa-plus me-2"></i>Add Meme</router-link>
          <button type="button" class="btn btn-meme burger-btn">Log in</button>
        </div>
      </div>
      <div class="d-flex align-items-center flex-shrink-0 d-none d-lg-flex">
        <slot name="action" />
      </div>
    </nav>
    <hr class="app-line m-0" />
  </div>
</template>

<style scoped>
.burger-btn {
  min-width: 11rem;
}
</style>
