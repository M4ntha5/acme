import type { UseAsyncStateOptions, UseAsyncStateReturn } from '@vueuse/core';
import { refDebounced, useAsyncState } from '@vueuse/core';
import type { UnwrapNestedRefs } from 'vue';
import type { ParsedError } from '@/lib/parse-error';
import { parseError } from '@/lib/parse-error';
import { reactive, ref, watch } from 'vue';

type UseAsyncApiStateReturn<
  Data,
  Params extends unknown[] = [],
  Shallow extends boolean = true,
> = UnwrapNestedRefs<
  Omit<
    UseAsyncStateReturn<Data, Params, Shallow>,
    'error' | 'execute' | 'state'
  >
> & {
  error?: ParsedError;
  execute: (...args: Params) => Promise<Data>;
  state: Data;
  reset: () => void;
};

export function useAsyncApiState<
  Data,
  Params extends unknown[] = [],
  Shallow extends boolean = true,
>(
  promise: Promise<Data> | ((...args: Params) => Promise<Data>),
  initialState: Data,
  options?: UseAsyncStateOptions<Shallow, Data>,
): UseAsyncApiStateReturn<Data, Params, Shallow>;
export function useAsyncApiState<
  Data,
  Params extends unknown[] = [],
  Shallow extends boolean = true,
>(
  promise: Promise<Data> | ((...args: Params) => Promise<Data>),
  initialState?: Data,
  options?: UseAsyncStateOptions<Shallow, Data>,
): UseAsyncApiStateReturn<Data, Params, Shallow> & {
  state: Data | undefined;
};
export function useAsyncApiState<
  Data,
  Params extends unknown[] = [],
  Shallow extends boolean = true,
>(
  promise: Promise<Data> | ((...args: Params) => Promise<Data>),
  initialState: Data,
  options?: UseAsyncStateOptions<Shallow, Data>,
): Omit<UseAsyncApiStateReturn<Data, Params, Shallow>, 'state'> {
  const defaultOptions: UseAsyncStateOptions<Shallow, Data> = {
    immediate: false,
    throwError: true,
    resetOnExecute: false,
  };
  const asyncState = useAsyncState(promise, initialState, {
    ...defaultOptions,
    ...options,
  });

  const execute = (...args: Params) => asyncState.execute(0, ...args);

  const error = ref<ParsedError | undefined>();
  watch(
    () => asyncState.error.value,
    newError => {
      error.value = newError ? parseError(newError) : undefined;
    },
  );

  const reset = () => {
    asyncState.error.value = undefined;
    asyncState.state.value = initialState;
  };

  const isLoading = refDebounced(asyncState.isLoading, 0);

  return reactive({ ...asyncState, error, execute, isLoading, reset });
}
