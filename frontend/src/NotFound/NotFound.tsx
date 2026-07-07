import { Link } from "react-router-dom";

export default function NotFound() {
  return (
    <div className="rounded-lg border border-gray-200 bg-white p-12 text-center shadow-sm">
      <h1 className="text-4xl font-bold text-gray-900">404</h1>
      <p className="mt-2 text-gray-600">Page not found.</p>
      <Link
        to="/"
        className="mt-6 inline-block rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700"
      >
        Go home
      </Link>
    </div>
  );
}
