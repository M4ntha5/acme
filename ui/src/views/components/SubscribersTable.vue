<script setup lang="ts">
import type { SubscriberDto } from "@/api/dtos/SubscribersDto";
defineProps<{
  isLoading?: boolean;
  subscribers?: Array<SubscriberDto>;
}>();
</script>

<template>
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
