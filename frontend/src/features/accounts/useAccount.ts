import { useQuery } from "@tanstack/react-query";
import { accountsApi } from "@/api/accounts";

export const accountQueryKey = (id: string) => ["accounts", id] as const;

export const useAccount = (id: string) => {
  return useQuery({
    queryKey: accountQueryKey(id),
    queryFn: () => accountsApi.getById(id),
    enabled: !!id,
  });
};
