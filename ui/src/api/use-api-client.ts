import { ApplicationApiClient } from '@/api/application-api-client';

let apiClient: ApplicationApiClient | undefined = undefined;

export function useApplicationApiClient() {
  if (apiClient === undefined) {
    apiClient = new ApplicationApiClient();
  }
  return apiClient;
}
