import { Link, useParams } from "react-router-dom";
import { useTransactions } from "@/features/transactions/useTransactions";
import { useAccount } from "@/features/accounts/useAccount";

export default function UserAccount() {
  const { id } = useParams<{ id: string }>();
  const { data: account, isLoading, error } = useAccount(id ?? "");
  const { data: allTransactions } = useTransactions();

  if (isLoading) return <p className="text-gray-500">Loading account...</p>;
  if (error || !account)
    return <p className="text-red-600">Account not found.</p>;

  const transactions =
    allTransactions?.filter(
      (t) => t.fromAccountId === account.id || t.toAccountId === account.id,
    ) ?? [];

  return (
    <div className="space-y-6">
      <Link to="/" className="text-sm text-blue-600 hover:underline">
        ← Back to home
      </Link>

      <div className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm">
        <h2 className="text-xl font-semibold">{account.ownerName}</h2>
        <p className="text-sm text-gray-500">{account.number}</p>
        <p className="mt-4 text-3xl font-bold tabular-nums">
          {account.balance.toFixed(2)}{" "}
          <span className="text-lg text-gray-500">{account.currency}</span>
        </p>
      </div>

      <div className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm">
        <h3 className="mb-4 text-lg font-semibold">Account history</h3>
        {transactions.length === 0 ? (
          <p className="text-gray-500">No transactions for this account.</p>
        ) : (
          <ul className="divide-y divide-gray-100">
            {transactions.map((t) => {
              const isIncoming = t.toAccountId === account.id;
              return (
                <li
                  key={t.id}
                  className="flex items-center justify-between py-3"
                >
                  <span className="text-sm">
                    {isIncoming ? "Received from" : "Sent to"}{" "}
                    <span className="font-mono text-xs">
                      {(isIncoming ? t.fromAccountId : t.toAccountId).slice(
                        0,
                        8,
                      )}
                    </span>
                  </span>
                  <span
                    className={`font-semibold tabular-nums ${
                      isIncoming ? "text-green-600" : "text-red-600"
                    }`}
                  >
                    {isIncoming ? "+" : "-"}
                    {t.amount.toFixed(2)} {t.currency}
                  </span>
                </li>
              );
            })}
          </ul>
        )}
      </div>
    </div>
  );
}
