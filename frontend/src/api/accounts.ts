import { apiClient } from "./client";
import type { Account } from "@/types/Account";

export const accountsApi = {
  getAll: async (): Promise<Account[]> => {
    const { data } = await apiClient.get<Account[]>("/Accounts");
    return data;
  },
  getById: async (id: string): Promise<Account> => {
    const { data } = await apiClient.get<Account>(`/Accounts/${id}`);
    return data;
  },
};
