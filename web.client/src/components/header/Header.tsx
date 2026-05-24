import { Anchor, Button, Group, Image, TextInput, ActionIcon } from "@mantine/core";
import { Link, useRouterState } from "@tanstack/react-router";
import { IconSearch, IconUser, IconWorld } from "@tabler/icons-react";

import { NAV_ITEMS, normalizePath } from "../../routes";

export function Header() {
  const activePath = useRouterState({
    select: (state) => normalizePath(state.location.pathname),
  });

  return (
    <Group h="100%" px={150} justify="space-between">
      <Anchor component={Link} to="/" aria-label="Vibas" underline="never">
        <Image src="/assets/logo-vibas.svg" alt="Vibas" w={80} />
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
              color={isActive ? "dark" : "gray"}
              fw={isActive ? 600 : 400}
              size="sm"
              aria-current={isActive ? "page" : undefined}
            >
              {item.label}
            </Button>
          );
        })}
      </Group>

      <Group gap="sm" wrap="nowrap">
        <ActionIcon variant="transparent" color="gray" size="lg" aria-label="User account">
          <IconUser size={20} stroke={1.5} />
        </ActionIcon>
        <ActionIcon variant="transparent" color="gray" size="lg" aria-label="Language">
          <IconWorld size={20} stroke={1.5} />
        </ActionIcon>
        <TextInput
          placeholder="Search"
          leftSection={<IconSearch size={16} stroke={1.5} />}
          size="sm"
          radius="md"
          w={160}
          variant="default"
          styles={{
            input: {
              border: '1px solid var(--mantine-color-gray-3)',
            },
          }}
        />
      </Group>
    </Group>
  );
}
