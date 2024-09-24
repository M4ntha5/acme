<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useApplicationApiClient } from "@/api/use-api-client";
import { useAsyncApiState } from "@/composables/use-async-api-state";
import SubscribersImportDialog from "@/views/components/SubscribersImportDialog.vue";
import { useSessionStorage } from "@vueuse/core";
import type { GetSubscribersRequestFilterDto } from "@/api/dtos/SubscribersDto";
import SubscribersTable from "@/views/components/SubscribersTable.vue";
import { useFormat } from "@/lib/formatter";

const api = useApplicationApiClient();
const showImportDialog = ref<boolean>(false);
const format = useFormat();

const filters = useSessionStorage<GetSubscribersRequestFilterDto>(
  "subscriber-list-filters",
  {},
);

onMounted(async () => {
  await subscribers.execute();
});

// TODO use mapper to map to filters
const subscribers = useAsyncApiState(async () => {
  const response = await api.subscribers.getAll({
    email: filters.value?.email,
    expirationDateFrom: format.toLocaleDateString(
      filters.value?.expirationDateFrom,
    ),
    expirationDateTo: format.toLocaleDateString(
      filters.value?.expirationDateTo,
    ),
  });
  return response.data;
});

const onFiltersChanged = async (newFilters: GetSubscribersRequestFilterDto) => {
  filters.value = newFilters;
  await subscribers.execute();
};

const clearFilters = () => {
  filters.value = {
    email: undefined,
    expirationDateFrom: undefined,
    expirationDateTo: undefined,
  };
};
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
      show-filters
      :subscribers="subscribers.state"
      :is-loading="subscribers.isLoading"
      :filters="filters"
      @clear-filters-clicked="clearFilters"
      @filters-changed="onFiltersChanged"
    />
  </div>

  <SubscribersImportDialog v-model:visible="showImportDialog" />
</template>
