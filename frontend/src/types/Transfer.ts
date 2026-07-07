export interface TransferRequest {
  fromAccountId: string;
  toAccountId: string;
  amount: number;
}

export interface TransferSuccessResponse {
  success: true;
  transaction: {
    id: string;
    fromAccountId: string;
    toAccountId: string;
    amount: number;
    currency: string;
    createdAt: string;
  };
}

export interface TransferErrorResponse {
  success: false;
  error: string;
  message: string;
}
