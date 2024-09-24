import type { AxiosInstance, AxiosPromise } from "axios";
import type { FileImportResponseDto } from "@/api/dtos/SubscribersDto";

export class SubscribersImportService {
  constructor(public readonly httpRequest: AxiosInstance) {}

  public importSubscribers(
    formData: FormData,
  ): AxiosPromise<FileImportResponseDto> {
    return this.httpRequest.request({
      method: "POST",
      url: "/api/SubscribersImport",
      data: formData,
    });
  }
}
