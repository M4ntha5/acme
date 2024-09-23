<script setup lang="ts">
import { useApplicationApiClient } from '@/api/use-api-client';
import { useAppToast } from '@/composables/use-app-toast';
import { useAsyncApiState } from '@/composables/use-async-api-state';
import SubscribersTable from '@/views/components/SubscribersTable.vue';

const visible = defineModel<boolean>('visible');
const api = useApplicationApiClient();
const fileUploader = ref();
const toast = useAppToast();

const importSubscribers = useAsyncApiState(async () => {
  const formData = new FormData();
  selectedFiles.value?.forEach((f: File) => {
    formData.append('image', f);
  });

  const response = await api.subscribersImport.importSubscribers(formData);
  return response.data;
});

const onImportClick = async () => {
  const response = await importSubscribers.execute();

  if (response.errors.length) {
    toast.error('Error importing subscribers');
    return;
  } else if (response.expiredSubscribers.length) {
    toast.warn('Only valid subscribers imported');
    return;
  } else if (!response.errors.length && !response.expiredSubscribers.length) {
    toast.success('Subscribers successfully imported without errors');
    visible.value = false;
  }
};

const selectedFiles = computed(() => {
  const files = fileUploader.value?.files;

  if (Array.isArray(files) && files.every(item => item instanceof File)) {
    return files;
  }

  return undefined;
});
</script>

<template>
  <PrimeDialog v-model:visible="visible" modal header="Import Files">
    <div class="flex flex-column gap-4">
      <div v-if="!importSubscribers.state">
        <PrimeFileUpload
          ref="fileUploader"
          accept="text/csv"
          multiple
          custom-upload
          :show-cancel-button="false"
          :show-upload-button="false"
        >
          <template #empty>
            <p>Drag and drop files to here to upload.</p>
          </template>
        </PrimeFileUpload>
      </div>

      <div v-if="importSubscribers.state?.errors.length">
        <span class="font-bold mb-3">Errors</span>
        <div v-for="error in importSubscribers.state.errors" :key="error">
          <span class="text-red-400">{{ error }}</span>
        </div>
      </div>

      <div
        v-if="importSubscribers.state?.expiredSubscribers.length"
        class="flex flex-column gap-1"
      >
        <span class="font-bold">Expired Subscribers</span>
        <SubscribersTable
          :is-loading="importSubscribers.isLoading"
          :subscribers="importSubscribers.state.expiredSubscribers"
        />
      </div>
    </div>
    <template #footer>
      <PrimeButton
        v-if="!importSubscribers.state"
        severity="primary"
        label="Import"
        :loading="importSubscribers.isLoading"
        :disabled="selectedFiles?.length === 0"
        data-test="buttonClose"
        @click="onImportClick"
      />
      <PrimeButton
        severity="danger"
        label="Close"
        data-test="buttonClose"
        @click="visible = false"
      />
    </template>
  </PrimeDialog>
</template>
