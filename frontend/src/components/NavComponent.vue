<script setup>

</script>

<template>
  <div class="container">
    <nav class="navbar navbar-expand-lg d-flex align-items-center justify-content-between py-2">
      <div class="d-flex align-items-center gap-4">
        <router-link class="navbar-brand d-flex align-items-center gap-2 mb-0 text-decoration-none text-dark flex-shrink-0" to="/">
          <i class="fa-regular fa-face-smile fs-4" style="color: var(--brand-purple);"></i>
          <span class="font-monospace fw-bold text-nowrap">Meme Board</span>
        </router-link>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarSupportedContent">
          <ul class="navbar-nav mb-2 mb-lg-0">
            <li class="nav-item">
              <router-link class="nav-link" aria-current="page" to="/home"><i class="fa-solid me-1"></i>Home</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" aria-current="page" to="/myMemes"><i class="fa-solid me-1"></i>MyMemes</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" aria-current="page" to="/about">About</router-link>
            </li>
          </ul>
        </div>
      </div>
      <div class="d-flex align-items-center flex-shrink-0">
        <slot name="action" />
      </div>
    </nav>
    <hr class="app-line m-0" />
  </div>
</template>
