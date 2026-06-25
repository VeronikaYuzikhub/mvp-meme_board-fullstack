<template>
  <div class="d-flex align-items-center gap-3">
    <div class="header-search-wrap">
      <input
        type="search"
        class="form-control header-search-input"
        placeholder="Search memes..."
        aria-label="Search"
      />
      <i class="fa-solid fa-magnifying-glass header-search-icon" aria-hidden="true"></i>
    </div>
    <router-link class="btn btn-meme d-none d-lg-inline-flex align-items-center flex-shrink-0" aria-current="page" to="/addMeme"><i class="fa-solid fa-plus me-2"></i>Add Meme</router-link>
  </div>
</template>
