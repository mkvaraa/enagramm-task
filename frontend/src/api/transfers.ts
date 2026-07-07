import { AxiosError } from "axios";
import { apiClient } from "./client";
import type {
  TransferRequest,
  TransferSuccessResponse,
  TransferErrorResponse,
} from "../types/Transfer";

export const transfersApi = {
  create: async (
    request: TransferRequest,
  ): Promise<TransferSuccessResponse> => {
    try {
      const { data } = await apiClient.post<TransferSuccessResponse>(
        "/Transfers",
        request,
      );
      return data;
    } catch (err) {
      if (err instanceof AxiosError && err.response) {
        throw err.response.data as TransferErrorResponse;
      }
      throw err;
    }
  },
};
