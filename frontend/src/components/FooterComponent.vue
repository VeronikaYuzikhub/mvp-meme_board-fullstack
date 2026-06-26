<template>
  <footer class="bg-dark slime-footer">
    <svg class="slime-svg flip-x" viewBox="0 0 1024 60" preserveAspectRatio="none">
      <path
        d="M0,0 C170,40 340,40 512,15 C684,-10 854,40 1024,0 L1024,60 L0,60 Z"
        fill="#a78bfa"
        class="flip-y" />
    </svg>
    <div class="footer-content">
      <p class="m-0">© 2026 MemeBoard. All rights reserved.</p>
    </div>
  </footer>
</template>

<style scoped>
.slime-footer {
  position: relative;
  width: 100%;
  padding-top: 40px;
  flex-grow: 0.5;
  display: flex;
  flex-direction: column;
}

.slime-svg {
  position: absolute;
  top: -15px;
  left: 0;
  width: 100%;
  height: 40px;
  display: block;
}

.footer-content {
  flex-grow: 1;
  text-align: center;
  color: #cbd5e1;
  padding: 1.5rem 0 2.5rem;
}

.flip-y {
  transform: scaleX(-1);
  transform-origin: center;
}

.flip-x {
  transform: scaleY(-1);
  transform-origin: center;
}
</style>
