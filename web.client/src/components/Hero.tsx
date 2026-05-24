import {
  Box,
  Button,
  Container,
  Grid,
  Group,
  Image,
  Stack,
  Text,
  Title,
} from "@mantine/core";
import { IconArrowRight } from "@tabler/icons-react";

import classes from "./Hero.module.css";

export function Hero() {
  return (
    <Box className={classes.heroWrapper}>
      <Container size="xl" py={80}>
        <Grid align="center" gap={60}>

          <Grid.Col span={{ base: 12, md: 7 }}>
            <Stack gap="xl">
              <Title
                order={1}
                fz={{ base: 36, sm: 44, md: 52 }}
                fw={800}
                lh={1.15}
                c="dark.9"
              >
                Transforming logic into{" "}
                <span className={classes.modularHighlight}>modular</span>
                <br />
                code and community assets.
              </Title>

              <Text
                fz={{ base: "sm", md: "md" }}
                c="dimmed"
                maw={480}
                lh={1.7}
              >
                Move from block-based simulation to academic documentation with{" "}
                <Text span fw={700} c="dark">
                  LaTeX TikZ
                </Text>{" "}
                exports. Fork community algorithms, optimize for time
                complexity, and contribute to the ever-growing{" "}
                <Text span fw={700} c="dark">
                  VIBAS library
                </Text>{" "}
                of reusable components.
              </Text>

              <Group gap="md" mt="sm">
                <Button
                  size="lg"
                  radius="xl"
                  color="dark"
                  rightSection={<IconArrowRight size={18} />}
                >
                  Launch Web Editor
                </Button>
                <Button
                  size="lg"
                  radius="xl"
                  variant="outline"
                  color="dark"
                >
                  View GitHub Repo
                </Button>
              </Group>
            </Stack>
          </Grid.Col>


          <Grid.Col span={{ base: 12, md: 5 }}>
            <Image
              src="/Hero.png"
              alt="Vibas"
              fit="contain"
              maw={520}
              w="100%"
              ml="auto"
              className={classes.heroImage}
            />
          </Grid.Col>
        </Grid>
      </Container>
    </Box>
  );
}
