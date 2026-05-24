export const NAV_ITEMS = [
  { label: "Features", path: "/features" },
  { label: "Use Cases", path: "/use-cases" },
  { label: "Downloads", path: "/download" },
  { label: "About", path: "/about" },
] as const;

export function normalizePath(pathname: string) {
  return pathname.replace(/\/+$/, "") || "/";
}
