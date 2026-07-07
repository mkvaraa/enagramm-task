import { Link } from "react-router-dom";
import { useAccounts } from "./useAccounts";

export const AccountsList = () => {
  const { data: accounts, isLoading, error } = useAccounts();

  if (isLoading) return <p className="text-gray-500">Loading accounts...</p>;
  if (error) return <p className="text-red-600">Failed to load accounts.</p>;
  if (!accounts?.length) return <p className="text-gray-500">No accounts.</p>;

  return (
    <div className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm">
      <h2 className="mb-4 text-xl font-semibold">Accounts</h2>
      <ul className="divide-y divide-gray-100">
        {accounts.map((account) => (
          <li key={account.id}>
            <Link
              to={`/account/${account.id}`}
              className="flex items-center justify-between py-3 hover:bg-gray-50 -mx-2 px-2 rounded"
            >
              <div>
                <p className="font-medium">{account.ownerName}</p>
                <p className="text-sm text-gray-500">{account.number}</p>
              </div>
              <p className="text-lg font-semibold tabular-nums">
                {account.balance.toFixed(2)}{" "}
                <span className="text-sm text-gray-500">
                  {account.currency}
                </span>
              </p>
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
};
