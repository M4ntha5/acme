<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useApplicationApiClient } from '@/api/use-api-client';
import { useAsyncApiState } from '@/composables/use-async-api-state';
import SubscribersImportDialog from '@/views/components/SubscribersImportDialog.vue';
import { useSessionStorage, watchDebounced } from '@vueuse/core';
import type { GetSubscribersRequestFilterDto } from '@/api/dtos/SubscribersDto';
import SubscribersTable from '@/views/components/SubscribersTable.vue';

const api = useApplicationApiClient();
const showImportDialog = ref<boolean>(false);

const filters = useSessionStorage<GetSubscribersRequestFilterDto>(
  'subscriber-list-filters',
  {},
);

onMounted(async () => {
  await subscribers.execute();
});

const subscribers = useAsyncApiState(async () => {
  const response = await api.subscribers.getAll(filters.value);
  return response.data;
});

watchDebounced(
  () => filters.value,
  async () => {
    await subscribers.execute();
  },
  { debounce: 500, deep: true },
);
</script>

<template>
  <div class="flex flex-column gap-3 p-8" v-bind="$attrs">
    <div class="text-right">
      <PrimeButton label="Import" @click="showImportDialog = true" />
    </div>

    <h2 class="text-center">Subscribers</h2>

    <template v-if="!subscribers.state">
      <div class="flex justify-content-center">No records yet</div>
    </template>
    <SubscribersTable
      v-else
      v-model:filters="filters"
      :subscribers="subscribers.state"
      :is-loading="subscribers.isLoading"
      show-filters
      @clear-filters-clicked="filters = {}"
    />
  </div>

  <SubscribersImportDialog v-model:visible="showImportDialog" />
</template>
