import { NavLink, Outlet } from "react-router-dom";

const linkBase = "px-3 py-2 rounded-md text-sm font-medium transition-colors";

const getLinkClass = ({ isActive }: { isActive: boolean }) =>
  isActive
    ? `${linkBase} bg-blue-600 text-white`
    : `${linkBase} text-gray-700 hover:bg-gray-100`;

export default function AppLayout() {
  return (
    <div className="min-h-screen bg-gray-50">
      <header className="border-b border-gray-200 bg-white">
        <nav className="mx-auto flex max-w-5xl items-center justify-between px-4 py-4">
          <NavLink to="/" className="text-lg font-bold">
            Money Transfer
          </NavLink>
          <div className="flex gap-2">
            <NavLink to="/" end className={getLinkClass}>
              Home
            </NavLink>
            <NavLink to="/transactions" className={getLinkClass}>
              Transactions
            </NavLink>
          </div>
        </nav>
      </header>

      <main className="mx-auto max-w-5xl px-4 py-8">
        <Outlet />
      </main>
    </div>
  );
}
