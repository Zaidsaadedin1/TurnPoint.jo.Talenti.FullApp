import React from "react";
import { Center, Text, Stack } from "@mantine/core";
import HomeNavbar from "./HomeNavbar";
import VideoBackground from "../../blocks/VideoBackground";

const styles = {
  registerContainer: {
    position: "relative",
    height: "100vh",
  },
  videoBackground: {
    position: "absolute",
    top: 0,
    left: 0,
    width: "100%",
    height: "100%",
    zIndex: -1,
  },
  content: {
    position: "relative",
    zIndex: 2,
    color: "#fff",
    padding: "1rem",
    textAlign: "center",
  },
  centerContainer: {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    zIndex: 2,
  },
};

export default function HomePage() {
  return (
    <Stack gap={0}>
      <HomeNavbar />
      <VideoBackground videos={[{ src: "/videos/5.mp4", type: "video/mp4" }]} />
      <Center style={styles.content}>
        <Text align="center" size="xl" weight={700}>
          Welcome to the HomePage
        </Text>
      </Center>
    </Stack>
  );
}
