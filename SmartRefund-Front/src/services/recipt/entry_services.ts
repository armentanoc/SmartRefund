import {
  ReceipEntryResponseType,
  ReceipEntryType,
} from "@/types/refund/ReciptEntryType";
import { AxiosPromise } from "axios";
import { api } from "../api";

export const EntryServices = {
  sendReceipt(
    receipEntryForm: ReceipEntryType,
  ): AxiosPromise<ReceipEntryResponseType> {
    const headers = {
      "Content-Type": "multipart/form-data",
    };

    return api.post(`/receipt`, receipEntryForm, { headers });
  },
};
