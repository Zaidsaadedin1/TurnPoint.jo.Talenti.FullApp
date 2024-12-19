import { Center, Container, Text, Button } from "@mantine/core";
import Link from "next/link";

export default function Unauthorized() {
  return (
    <Container>
      <Center style={{ height: "100vh", flexDirection: "column" }}>
        <Text size="xl" weight={700} color="red">
          401 - Unauthorized
        </Text>
        <Text mt="md">You are not authorized to access this page.</Text>
        <Link href="/">
          <Button mt="lg" variant="outline">
            Go to Home
          </Button>
        </Link>
      </Center>
    </Container>
  );
}
