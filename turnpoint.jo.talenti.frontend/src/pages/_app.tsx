import "@mantine/core/styles.css";
import "../styles/globals.css"; // Import your global CSS with font-face
import "@mantine/core/styles.css";
import "@mantine/dates/styles.css";
import { MantineProvider } from "@mantine/core";

const theme = {
  colors: {
    brand: [
      "#ffffff", // Lightest (white)
      "#f2f2f2", // Very light gray
      "#e5e5e5", // Light gray
      "#cccccc", // Gray
      "#999999", // Medium gray
      "#666666", // Dark gray
      "#333333", // Very dark gray
      "#1a1a1a", // Almost black
      "#0d0d0d", // Black
      "#000000", // Deep black
    ],
  },
  primaryColor: "brand",
  black: "#000000",
  white: "#ffffff",
  defaultRadius: "sm",
  fontFamily: "Roboto Condensed, Arial, sans-serif", // Use your custom font
  headings: {
    fontFamily: "Roboto Condensed, Arial, sans-serif", // Apply to headings
  },
};

export default function App({ Component, pageProps }) {
  if (pageProps?.statusCode === 400) {
    router.push("/400");
    return null;
  }

  if (pageProps?.statusCode === 404) {
    router.push("/404");
    return null;
  }

  if (pageProps?.statusCode === 500) {
    router.push("/500");
    return null;
  }
  return (
    <MantineProvider theme={theme} withGlobalStyles withNormalizeCSS>
      <Component {...pageProps} />
    </MantineProvider>
  );
}
