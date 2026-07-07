import { apiClient } from "./client";
import type { Transaction } from "../types/Transaction";

export const transactionsApi = {
  getAll: async (): Promise<Transaction[]> => {
    const { data } = await apiClient.get<Transaction[]>("/Transactions");
    return data;
  },
};
