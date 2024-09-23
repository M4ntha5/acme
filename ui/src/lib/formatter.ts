import type { App } from 'vue';
import { format } from 'date-fns';
import { nl } from 'date-fns/locale';

export class Formatter {
  private static _instance: Formatter | undefined;
  public static get instance() {
    if (!this._instance) {
      this._instance = new Formatter();
    }
    return this._instance;
  }

  private constructor() {
    // singleton only
  }

  date(input: Date | string, dateFormat: string): string;
  date(
    input?: Date | string | null | undefined,
    dateFormat?: string | null | undefined,
  ): string | undefined;
  date(
    input?: Date | string | null | undefined,
    dateFormat?: string | null | undefined,
  ): string | undefined {
    if (!input) return;
    const date = typeof input === 'string' ? new Date(input) : input;
    return format(date, dateFormat ?? 'yyyy-M-dd', {
      locale: nl,
    });
  }

  static install = (app: App) => {
    app.config.globalProperties.$format = Formatter.instance;
  };
}

export function useFormat() {
  return Formatter.instance;
}

declare module '@vue/runtime-core' {
  export interface ComponentCustomProperties {
    $format: Formatter;
  }
}
