/* eslint-disable @typescript-eslint/ban-ts-comment */
import { resolve } from 'path';
import { defineConfig, loadEnv } from 'vite';
import vue from '@vitejs/plugin-vue';
import vueJsx from '@vitejs/plugin-vue-jsx';
import eslint from 'vite-plugin-eslint';
import UnpluginVueComponents from 'unplugin-vue-components/vite';
import {
  HeadlessUiResolver,
  PrimeVueResolver,
} from 'unplugin-vue-components/resolvers';
import UnpluginAutoImport from 'unplugin-auto-import/vite';
import { createHtmlPlugin } from 'vite-plugin-html';

export default ({ mode }: { mode: string }) => {
  process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };
  return defineConfig({
    plugins: [
      vue({
        script: {
          defineModel: true,
        },
      }),
      createHtmlPlugin({
        minify:
          process.env.VITE_ENV !== 'development'
            ? {
                minifyJS: true,
                collapseWhitespace: true,
                keepClosingSlash: true,
                removeComments: true,
                removeRedundantAttributes: true,
                removeScriptTypeAttributes: true,
                removeStyleLinkTypeAttributes: true,
                useShortDoctype: true,
                minifyCSS: true,
              }
            : false,
      }),
      vueJsx(),
      eslint(),
      UnpluginVueComponents({
        dts: resolve(process.cwd(), `./src/typings/generated-component.d.ts`),
        resolvers: [
          PrimeVueResolver({
            prefix: 'Prime',
          }),
          HeadlessUiResolver({
            prefix: 'Headless',
          }),
        ],
      }),
      UnpluginAutoImport({
        imports: ['vue', 'vue-router'],
        dts: resolve(process.cwd(), `./src/typings/auto-imports.d.ts`),
      }),
    ],
    resolve: {
      alias: {
        '@': resolve(process.cwd(), './src'),
      },
    },
    root: `src`,
    server: {
      port: 5090,
    },
    build: {
      outDir: resolve(process.cwd(), `dist`),
      emptyOutDir: true,
      sourcemap: process.env.VITE_ENV === 'development' ? 'inline' : 'hidden',
    },
  });
};
