import type { AxiosInstance, AxiosPromise } from "axios";
import type { SubscriberDto } from "@/api/dtos/SubscribersDto";

export class SubscribersService {
  constructor(public readonly httpRequest: AxiosInstance) {}

  public getAll({
    email,
    expirationDateFrom,
    expirationDateTo,
  }: {
    email?: string;
    expirationDateFrom?: string;
    expirationDateTo?: string;
  }): AxiosPromise<Array<SubscriberDto>> {
    return this.httpRequest.request({
      method: "GET",
      url: "/api/Subscribers",
      params: {
        email: email,
        expirationDateFrom: expirationDateFrom,
        expirationDateTo: expirationDateTo,
      },
    });
  }
}
