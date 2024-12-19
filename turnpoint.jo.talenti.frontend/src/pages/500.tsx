import { Center, Container, Text, Button } from "@mantine/core";
import Link from "next/link";

export default function ServerError() {
  return (
    <Container>
      <Center style={{ height: "100vh", flexDirection: "column" }}>
        <Text size="xl" weight={700} color="red">
          500 - Server Error
        </Text>
        <Text mt="md">Something went wrong. Please try again later.</Text>
        <Link href="/">
          <Button mt="lg" variant="outline">
            Go to Home
          </Button>
        </Link>
      </Center>
    </Container>
  );
}
