import { createRouter, createWebHistory } from 'vue-router';

export enum RouteNames {
  Index = 'Index',
}

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: () => import('@/views/SubscriberListView.vue'),
      name: RouteNames.Index,
    },
  ],
});

declare module 'vue-router' {
  // interface RouteMeta {}
}
