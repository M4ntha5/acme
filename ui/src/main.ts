import App from "@/App.vue";
import { createApp } from "vue";
import PrimeVue from "primevue/config";
import ToastService from "primevue/toastservice";
import { createPinia } from "pinia";
import { parseError } from "@/lib/parse-error";
import { createHead } from "@unhead/vue";
import DialogService from "primevue/dialogservice";
import { router } from "@/router";
import "primevue/resources/themes/aura-light-green/theme.css";

const pinia = createPinia();
const app = createApp(App);
const head = createHead();

app.use(head);
app.use(router);
app.use(PrimeVue, {
  ripple: true,
});
app.use(ToastService);
app.use(DialogService);
app.use(pinia);

app.mount("#app");

app.config.errorHandler = (e: unknown) => {
  // eslint-disable-next-line no-console
  console.error(e);

  const parsedError = parseError(e, "\n");
  app.config.globalProperties.$toast.add({
    summary: parsedError?.name,
    detail: parsedError?.message,
    severity: "error",
    life: 5000,
  });
};
