import { lazy, Suspense } from "react";
import type { RouteObject } from "react-router-dom";
import AppLayout from "@/layouts/AppLayout";
import Loader from "@/components/Loader";

const HomePage = lazy(() => import("@/pages/HomePage"));
const TransactionsPage = lazy(() => import("@/pages/TransactionsPage"));
const UserAccountPage = lazy(() => import("@/pages/UserAccountPage"));
const NotFoundPage = lazy(() => import("@/pages/NotFoundPage"));

const withSuspense = (element: React.ReactNode) => (
  <Suspense fallback={<Loader />}>{element}</Suspense>
);

export const routes: RouteObject[] = [
  {
    element: <AppLayout />,
    children: [
      {
        path: "/",
        element: withSuspense(<HomePage />),
      },
      {
        path: "/transactions",
        element: withSuspense(<TransactionsPage />),
      },
      {
        path: "/account/:id",
        element: withSuspense(<UserAccountPage />),
      },
      {
        path: "*",
        element: withSuspense(<NotFoundPage />),
      },
    ],
  },
];
