<script setup lang="ts">
import type { GetSubscribersRequestFilterDto } from "@/api/dtos/SubscribersDto";

const props = defineProps<{
  filters?: GetSubscribersRequestFilterDto;
}>();

const emit = defineEmits<{
  clearFiltersClicked: [];
  filtersChanged: [GetSubscribersRequestFilterDto];
}>();

const tableFilters = ref<GetSubscribersRequestFilterDto>({
  email: undefined,
  expirationDateTo: undefined,
  expirationDateFrom: undefined,
});

watch(
  () => props.filters,
  (newFilters) => {
    tableFilters.value = { ...newFilters };
  },
  { immediate: true },
);

const filtersEmpty = computed(() => {
  return (
    !tableFilters.value?.expirationDateFrom &&
    !tableFilters.value?.expirationDateTo &&
    !tableFilters.value?.email
  );
});
</script>

<template>
  <div class="flex gap-3">
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
      severity="success"
      label="Filter"
      @click="$emit('filtersChanged', tableFilters)"
    />
    <PrimeButton
      :disabled="filtersEmpty"
      severity="help"
      label="Clear Filters"
      @click="$emit('clearFiltersClicked')"
    />
  </div>
</template>
