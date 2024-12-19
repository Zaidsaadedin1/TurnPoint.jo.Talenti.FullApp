import { Center, Container, Text, Button } from "@mantine/core";
import Link from "next/link";

export default function Forbidden() {
  return (
    <Container>
      <Center style={{ height: "100vh", flexDirection: "column" }}>
        <Text size="xl" weight={700} color="red">
          403 - Forbidden
        </Text>
        <Text mt="md">You don’t have permission to view this page.</Text>
        <Link href="/">
          <Button mt="lg" variant="outline">
            Go to Home
          </Button>
        </Link>
      </Center>
    </Container>
  );
}
