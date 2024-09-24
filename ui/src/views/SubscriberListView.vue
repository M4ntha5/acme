<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useApplicationApiClient } from "@/api/use-api-client";
import { useAsyncApiState } from "@/composables/use-async-api-state";
import SubscribersImportDialog from "@/views/components/SubscribersImportDialog.vue";
import { useSessionStorage } from "@vueuse/core";
import type { GetSubscribersRequestFilterDto } from "@/api/dtos/SubscribersDto";
import SubscribersTable from "@/views/components/SubscribersTable.vue";
import { useFormat } from "@/lib/formatter";
import SubscriberFilters from "@/views/components/SubscriberFilters.vue";

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

const clearFilters = async () => {
  filters.value = {};
  await subscribers.execute();
};
</script>

<template>
  <div class="flex flex-column gap-3 p-8" v-bind="$attrs">
    <div class="text-right">
      <PrimeButton label="Import" @click="showImportDialog = true" />
    </div>

    <h2 class="text-center">Subscribers</h2>

    <template v-if="subscribers.state">
      <SubscriberFilters
        :filters="filters"
        @clear-filters-clicked="clearFilters"
        @filters-changed="onFiltersChanged($event)"
      />
      <SubscribersTable
        show-filters
        :subscribers="subscribers.state"
        :is-loading="subscribers.isLoading"
      />
    </template>
  </div>

  <SubscribersImportDialog v-model:visible="showImportDialog" />
</template>
