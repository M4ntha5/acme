<script setup lang="ts">
import type {
  GetSubscribersRequestFilterDto,
  SubscriberDto,
} from "@/api/dtos/SubscribersDto";
import { watchDebounced } from "@vueuse/core";

const props = defineProps<{
  showFilters?: boolean;
  isLoading?: boolean;
  subscribers: Array<SubscriberDto>;
  filters?: GetSubscribersRequestFilterDto;
}>();

const emit = defineEmits<{
  clearFiltersClicked: [];
  filtersChanged: [GetSubscribersRequestFilterDto];
}>();

const tableFilters = ref<GetSubscribersRequestFilterDto>({});

watch(
  () => props.filters,
  () => {
    tableFilters.value = { ...props.filters };
  },
);

watchDebounced(
  () => tableFilters.value,
  () => {
    emit("filtersChanged", tableFilters.value);
  },
  { debounce: 500, deep: true },
);
</script>

<template>
  <div v-if="showFilters" class="flex gap-3">
    <PrimeInputText v-model="tableFilters.email" placeholder="Email" />
    <PrimeCalendar
      v-model="tableFilters.expirationDateFrom"
      placeholder="Date From"
    />
    <PrimeCalendar
      v-model="tableFilters.expirationDateTo"
      placeholder="Date To"
    />
    <PrimeButton
      :disabled="
        !tableFilters?.expirationDateFrom &&
        !tableFilters?.expirationDateTo &&
        !tableFilters?.email
      "
      severity="help"
      label="Clear Filters"
      @click="$emit('clearFiltersClicked')"
    />
  </div>

  <PrimeDataTable
    :value="subscribers"
    :loading="isLoading"
    :rows="10"
    paginator
  >
    <PrimeColumn field="email" header="Email" sortable />
    <PrimeColumn
      header="Expiration Date"
      sortable
      data-type="date"
      field="expirationDate"
    />
  </PrimeDataTable>
</template>
