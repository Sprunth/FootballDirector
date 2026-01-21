import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  build: {
    outDir: '../FootballDirector.Server/wwwroot',
    emptyDirBeforeWrite: true
  },
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7153',
        changeOrigin: true,
        secure: false
      }
    }
  }
})
