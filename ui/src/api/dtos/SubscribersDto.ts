export type SubscriberDto = {
  id: string;
  email: string;
  expirationDate: Date;
};

export type FileImportResponseDto = {
  errors: Array<string>;
  expiredSubscribers: Array<SubscriberDto>;
};

export type GetSubscribersRequestFilterDto = {
  email?: string;
  expirationDateFrom?: Date;
  expirationDateTo?: Date;
};
