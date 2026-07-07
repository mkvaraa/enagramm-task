import { useQuery } from "@tanstack/react-query";
import { transactionsApi } from "../../api/transactions";

export const transactionsQueryKey = ["transactions"] as const;

export const useTransactions = () => {
  return useQuery({
    queryKey: transactionsQueryKey,
    queryFn: transactionsApi.getAll,
  });
};
