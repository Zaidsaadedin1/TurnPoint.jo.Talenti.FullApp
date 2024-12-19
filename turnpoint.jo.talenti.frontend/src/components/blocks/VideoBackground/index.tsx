import { Box } from "@mantine/core";

const styles = {
  videoContainer: {
    display: "flex",
    flexDirection: "row",
    position: "absolute",
    top: 0,
    left: 0,
    width: "100%",
    height: "100vh",
    zIndex: -1,
  },
  videoBackground: {
    flex: 1,
    objectFit: "cover",
    width: "100%",
    height: "100vh",
    top: 0,
    left: 0,
  },
  linkStyle: {
    textDecoration: "none",
    color: "inherit",
  },
  stepperContainer: {
    maxHeight: "80vh",
    overflowY: "auto",
    scrollbarWidth: "thin", // For Firefox
    scrollbarColor: "black transparent", // For Firefox
  },
  "::-webkit-scrollbar": {
    width: "6px", // For Chrome, Edge, and Safari
  },
  "::-webkit-scrollbar-track": {
    background: "transparent",
  },
  "::-webkit-scrollbar-thumb": {
    background: "black",
    borderRadius: "10px",
  },
};

export default function VideoBackground({ videos }) {
  if (videos.length > 1) {
    return (
      <Box style={styles.videoContainer}>
        {videos.map((video, index) => (
          <Box
            key={index}
            component="video"
            autoPlay
            loop
            muted
            style={styles.videoBackground}
          >
            <source src={video.src} type={video.type} />
          </Box>
        ))}
      </Box>
    );
  }

  return (
    <Box component="video" autoPlay loop muted style={styles.videoBackground}>
      <source src={videos[0].src} type={videos[0].type} />
    </Box>
  );
}
