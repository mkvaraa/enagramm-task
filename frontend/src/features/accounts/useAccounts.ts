import { useQuery } from "@tanstack/react-query";
import { accountsApi } from "../../api/accounts";

export const accountsQueryKey = ["accounts"] as const;

export const useAccounts = () => {
  return useQuery({
    queryKey: accountsQueryKey,
    queryFn: accountsApi.getAll,
  });
};
