import { AccountsList } from "@/features/accounts/AccountsList";
import { TransferForm } from "@/features/transfers/TransferForm";

export default function Home() {
  return (
    <div className="grid gap-6 lg:grid-cols-2">
      <AccountsList />
      <TransferForm />
    </div>
  );
}
