export const NAV_ITEMS = [
  { label: "Products", path: "/products" },
  { label: "Rankings", path: "/rankings" },
  { label: "Download", path: "/download" },
  { label: "About", path: "/about" },
] as const;

export function normalizePath(pathname: string) {
  return pathname.replace(/\/+$/, "") || "/";
}
