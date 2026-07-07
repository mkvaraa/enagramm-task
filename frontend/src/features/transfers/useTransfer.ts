import { useMutation, useQueryClient } from "@tanstack/react-query";
import { transfersApi } from "../../api/transfers";
import { accountsQueryKey } from "../accounts/useAccounts";
import { transactionsQueryKey } from "../transactions/useTransactions";
import type {
  TransferErrorResponse,
  TransferRequest,
  TransferSuccessResponse,
} from "../../types/Transfer";

export const useTransfer = () => {
  const queryClient = useQueryClient();

  return useMutation<TransferSuccessResponse, TransferErrorResponse, TransferRequest>({
    mutationFn: transfersApi.create,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: accountsQueryKey });
      queryClient.invalidateQueries({ queryKey: transactionsQueryKey });
    },
  });
};
