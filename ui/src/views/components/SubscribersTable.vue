<script setup lang="ts">
import type {
  GetSubscribersRequestFilterDto,
  SubscriberDto,
} from '@/api/dtos/SubscribersDto';

defineEmits<{
  clearFiltersClicked: [];
}>();

defineProps<{
  showFilters?: boolean;
  isLoading?: boolean;
  subscribers: Array<SubscriberDto>;
}>();

//TODO fix filters
const filters = defineModel<GetSubscribersRequestFilterDto>('filters');
</script>

<template>
  <div v-if="showFilters" class="flex gap-3">
    <PrimeInputText v-model="filters?.email" placeholder="Email" />
    <PrimeCalendar
      v-model="filters?.expirationDateFrom"
      placeholder="Date From"
    />
    <PrimeCalendar v-model="filters?.expirationDateTo" placeholder="Date To" />
    <PrimeButton
      :disabled="
        !filters?.expirationDateFrom &&
        !filters?.expirationDateTo &&
        !filters?.email
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
