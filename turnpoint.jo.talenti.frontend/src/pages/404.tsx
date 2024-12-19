import { Center, Container, Text, Button } from "@mantine/core";
import Link from "next/link";

export default function NotFound() {
  return (
    <Container>
      <Center style={{ height: "100vh", flexDirection: "column" }}>
        <Text size="xl" weight={700} color="red">
          404 - Page Not Found
        </Text>
        <Text mt="md">The page you’re looking for doesn’t exist.</Text>
        <Link href="/">
          <Button mt="lg" variant="outline">
            Go to Home
          </Button>
        </Link>
      </Center>
    </Container>
  );
}
