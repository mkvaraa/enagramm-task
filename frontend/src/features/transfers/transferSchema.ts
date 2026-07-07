import { z } from "zod";

export const transferSchema = z
  .object({
    fromAccountId: z.string().min(1, "Sender is required"),
    toAccountId: z.string().min(1, "Receiver is required"),
    amount: z.coerce.number().positive("Amount must be positive"),
  })
  .refine((d) => d.fromAccountId !== d.toAccountId, {
    message: "Sender and receiver must differ",
    path: ["toAccountId"],
  });

export type TransferFormInput = z.input<typeof transferSchema>;
export type TransferFormOutput = z.output<typeof transferSchema>;
