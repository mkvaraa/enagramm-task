import { useTransactions } from "./useTransactions";
import { useAccounts } from "../accounts/useAccounts";

export const TransactionsList = () => {
  const { data: transactions, isLoading } = useTransactions();
  const { data: accounts } = useAccounts();

  const accountLabel = (id: string) => {
    const a = accounts?.find((x) => x.id === id);
    return a ? `${a.ownerName} (${a.number})` : id.slice(0, 8);
  };

  if (isLoading)
    return <p className="text-gray-500">Loading transactions...</p>;
  if (!transactions?.length)
    return <p className="text-gray-500">No transactions yet.</p>;

  return (
    <div className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm">
      <h2 className="mb-4 text-xl font-semibold">Transactions</h2>
      <ul className="divide-y divide-gray-100">
        {transactions.map((t) => (
          <li key={t.id} className="py-3 text-sm">
            <div className="flex items-center justify-between">
              <span>
                <span className="font-medium">
                  {accountLabel(t.fromAccountId)}
                </span>
                {" → "}
                <span className="font-medium">
                  {accountLabel(t.toAccountId)}
                </span>
              </span>
              <span className="font-semibold tabular-nums">
                {t.amount.toFixed(2)} {t.currency}
              </span>
            </div>
            <p className="text-xs text-gray-500">
              {new Date(t.createdAt).toLocaleString()}
            </p>
          </li>
        ))}
      </ul>
    </div>
  );
};
