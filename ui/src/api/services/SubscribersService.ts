import type { AxiosInstance, AxiosPromise } from 'axios';
import type {
  GetSubscribersRequestFilterDto,
  SubscriberDto,
} from '@/api/dtos/SubscribersDto';

export class SubscribersService {
  constructor(public readonly httpRequest: AxiosInstance) {}

  public getAll(
    filters: GetSubscribersRequestFilterDto,
  ): AxiosPromise<Array<SubscriberDto>> {
    return this.httpRequest.request({
      method: 'GET',
      url: '/api/Subscribers',
      params: filters,
    });
  }
}
