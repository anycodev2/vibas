import { Anchor, Button, Group, Image } from "@mantine/core";
import { Link, useRouterState } from "@tanstack/react-router";

import { NAV_ITEMS, normalizePath } from "../../routes";

export function Header() {
  const activePath = useRouterState({
    select: (state) => normalizePath(state.location.pathname),
  });

  return (
    <Group h="100%" px={150} justify="space-between">
      <Anchor component={Link} to="/" aria-label="Vibas">
        <Image src="/assets/logo-vibas.svg" alt="Vibas" w={120} />
      </Anchor>

      <Group gap="xs" wrap="nowrap">
        {NAV_ITEMS.map((item) => {
          const isActive = item.path === activePath;

          return (
            <Button
              key={item.path}
              component={Link}
              to={item.path}
              variant="transparent"
              color={isActive ? undefined : "gray"}
              aria-current={isActive ? "page" : undefined}
            >
              {item.label}
            </Button>
          );
        })}
      </Group>
    </Group>
  );
}
