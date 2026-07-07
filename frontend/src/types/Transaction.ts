export interface Transaction {
  id: string;
  fromAccountId: string;
  toAccountId: string;
  amount: number;
  currency: string;
  createdAt: string;
}
