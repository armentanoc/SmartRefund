import {
  AllReceiptDataType,
  ReceiptDataType,
} from "@/types/refund/EventSourceType";
import { FetchReceiptsDataOptions } from "@/types/refund/ReciptValidationType";
import { AxiosPromise } from "axios";
import { api } from "../api";

export const EventSourceServices = {
  getAllReceipts(
    options?: FetchReceiptsDataOptions,
  ): AxiosPromise<AllReceiptDataType> {
    return api.get(`/events/front`, {
      params: options,
      paramsSerializer: { indexes: null },
    });
  },

  getReceiptByHash(hash: string): AxiosPromise<ReceiptDataType> {
    return api.get(`events/${hash}/front`);
  },
};
