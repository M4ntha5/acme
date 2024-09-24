import { SubscribersImportApiClient } from "@/api/subscribers-import-api-client";

let apiClient: SubscribersImportApiClient | undefined = undefined;

export function useApplicationApiImporterClient() {
  if (apiClient === undefined) {
    apiClient = new SubscribersImportApiClient();
  }
  return apiClient;
}
