import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useAccounts } from "../accounts/useAccounts";
import { useTransfer } from "./useTransfer";
import {
  transferSchema,
  type TransferFormInput,
  type TransferFormOutput,
} from "./transferSchema";

export const TransferForm = () => {
  const { data: accounts } = useAccounts();
  const transfer = useTransfer();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<TransferFormInput, unknown, TransferFormOutput>({
    resolver: zodResolver(transferSchema),
    defaultValues: { fromAccountId: "", toAccountId: "", amount: 0 },
  });

  const onSubmit = (values: TransferFormOutput) => {
    transfer.mutate(values, {
      onSuccess: () => reset(),
    });
  };

  const serverError = transfer.error;

  return (
    <div className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm">
      <h2 className="mb-4 text-xl font-semibold">Transfer Money</h2>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <label className="mb-1 block text-sm font-medium">From</label>
          <select
            {...register("fromAccountId")}
            className="w-full rounded-md border border-gray-300 px-3 py-2"
          >
            <option value="">— select account —</option>
            {accounts?.map((a) => (
              <option key={a.id} value={a.id}>
                {a.ownerName} — {a.balance.toFixed(2)} {a.currency}
              </option>
            ))}
          </select>
          {errors.fromAccountId && (
            <p className="mt-1 text-sm text-red-600">
              {errors.fromAccountId.message}
            </p>
          )}
        </div>

        <div>
          <label className="mb-1 block text-sm font-medium">To</label>
          <select
            {...register("toAccountId")}
            className="w-full rounded-md border border-gray-300 px-3 py-2"
          >
            <option value="">— select account —</option>
            {accounts?.map((a) => (
              <option key={a.id} value={a.id}>
                {a.ownerName} — {a.number}
              </option>
            ))}
          </select>
          {errors.toAccountId && (
            <p className="mt-1 text-sm text-red-600">
              {errors.toAccountId.message}
            </p>
          )}
        </div>

        <div>
          <label className="mb-1 block text-sm font-medium">Amount</label>
          <input
            type="number"
            step="0.01"
            {...register("amount")}
            className="w-full rounded-md border border-gray-300 px-3 py-2"
          />
          {errors.amount && (
            <p className="mt-1 text-sm text-red-600">{errors.amount.message}</p>
          )}
        </div>

        {serverError && (
          <div className="rounded-md bg-red-50 p-3 text-sm text-red-700">
            {serverError.message ?? "Transfer failed"}
          </div>
        )}

        {transfer.isSuccess && (
          <div className="rounded-md bg-green-50 p-3 text-sm text-green-700">
            Transfer completed successfully.
          </div>
        )}

        <button
          type="submit"
          disabled={transfer.isPending}
          className="w-full rounded-md bg-blue-600 px-4 py-2 font-medium text-white hover:bg-blue-700 disabled:opacity-50"
        >
          {transfer.isPending ? "Sending..." : "Send Transfer"}
        </button>
      </form>
    </div>
  );
};
