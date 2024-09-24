import { useToast } from "primevue/usetoast";
import type { ToastMessageOptions } from "primevue/toast";

type AppToastOptions = Omit<
  ToastMessageOptions,
  "severity" | "summary" | "detail" | "group"
>;

export const createAppToast = (group?: string) => {
  const toast = useToast();
  const defaultToastLife = 5000;

  const success = (
    header: string,
    details?: string,
    options?: AppToastOptions,
  ) => {
    toast.add({
      severity: "success",
      summary: header,
      detail: details,
      life: defaultToastLife,
      group,
      ...options,
    });
  };

  const info = (
    header: string,
    details?: string,
    options?: AppToastOptions,
  ) => {
    toast.add({
      severity: "info",
      summary: header,
      detail: details,
      life: defaultToastLife,
      group,
      ...options,
    });
  };

  const warn = (
    header: string,
    details?: string,
    options?: AppToastOptions,
  ) => {
    toast.add({
      severity: "warn",
      summary: header,
      detail: details,
      life: defaultToastLife,
      group,
      ...options,
    });
  };

  const error = (
    header: string,
    details?: string,
    options?: AppToastOptions,
  ) => {
    toast.add({
      severity: "error",
      summary: header,
      detail: details,
      life: defaultToastLife,
      group,
      ...options,
    });
  };

  return {
    success,
    info,
    warn,
    error,
  };
};

const appToastInstance = ref<ReturnType<typeof createAppToast>>();
export const useAppToast = () => {
  if (!appToastInstance.value) {
    appToastInstance.value = createAppToast();
  }
  return appToastInstance.value!;
};
