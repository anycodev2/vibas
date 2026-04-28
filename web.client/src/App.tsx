import { createTheme, MantineProvider } from "@mantine/core";
import { RouterProvider } from "@tanstack/react-router";

import { router } from "./router";

const theme = createTheme({
  primaryColor: "blue",
  fontFamily:
    'Inter, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
});

export function App() {
  return (
    <MantineProvider theme={theme} defaultColorScheme="light">
      <RouterProvider router={router} />
    </MantineProvider>
  );
}
