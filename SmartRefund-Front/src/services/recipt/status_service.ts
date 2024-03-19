import {
  ChangeStatusResponseType,
  ChangeStatusType,
} from "@/types/refund/ChangeStatusType";
import { AxiosPromise } from "axios";
import { api } from "../api";

export const StatusServices = {
  changeStatus(
    changeStatusForm: ChangeStatusType,
  ): AxiosPromise<ChangeStatusResponseType> {
    return api.patch(`/receipt/status`, changeStatusForm);
  },
};
