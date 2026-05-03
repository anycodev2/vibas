import { AppShell } from "@mantine/core";
import { Outlet } from "@tanstack/react-router";

import { Header } from "@/components/header";

export function RootLayout() {
  return (
    <AppShell header={{ height: 72 }}>
      <AppShell.Header>
        <Header />
      </AppShell.Header>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>
    </AppShell>
  );
}
