import axios from "axios";
import type { AxiosInstance } from "axios";
import qs from "qs";
import { SubscribersImportService } from "@/api/services/SubscribersImportService";
import { Environment } from "@/lib/environment";

export class SubscribersImportApiClient {
  public readonly subscribersImport: SubscribersImportService;
  public readonly axiosInstance: AxiosInstance;

  constructor() {
    const apiProtocol =
      Environment.apiImporterProtocol ?? window.location.protocol;
    const apiHostName =
      Environment.apiImporterHostName ?? window.location.hostname;
    const apiPort = Environment.apiImporterPort
      ? `:${Environment.apiImporterPort}`
      : "";
    const apiBaseUrl = `${apiProtocol}//${apiHostName}${apiPort}`;

    this.axiosInstance = axios.create({
      baseURL: apiBaseUrl,
      paramsSerializer: {
        serialize: (params) => qs.stringify(params, { indices: false }),
      },
    });

    this.subscribersImport = new SubscribersImportService(this.axiosInstance);
  }
}
