import {
  Box,
  Center,
  Text,
  Button,
  TextInput,
  PasswordInput,
  Paper,
  LoadingOverlay,
} from "@mantine/core";
import { useState, useEffect } from "react";
import { useRouter } from "next/router";
import VideoBackground from "@/components/blocks/VideoBackground";
import { apis } from "@/backend-apis";
import { LoginUserDto } from "@/backend-apis/apiTypes/AuthDtos/AuthDtos";
import Link from "next/link";

const styles = {
  loginContainer: {
    position: "relative",
    height: "100vh",
  },
  centerContainer: {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    zIndex: 2,
  },
  paper: {
    width: "400px",
    padding: "2rem",
    borderRadius: "12px",
    border: "1px solid rgba(255, 255, 255, 0.3)",
    boxShadow: "0 8px 12px rgba(218, 213, 213, 0.2)",
    background: "rgba(253, 253, 253, 0.415)",
    backdropFilter: "blur(20px)",
  },
  linkStyle: { textDecoration: "none", color: "inherit" },
};

export default function Login() {
  const [formData, setFormData] = useState<LoginUserDto>({
    emailOrPhone: "",
    password: "",
  });
  const [errors, setErrors] = useState<Any>({});
  const [isTouched, setIsTouched] = useState<Any>({});
  const [isLoading, setIsLoading] = useState(false);
  const [loginError, setLoginError] = useState<string | null>(null);
  const router = useRouter();

  const validateForm = (): boolean => {
    const newErrors: Any = {};
    let isValid = true;

    // Email or Phone Validation
    if (!formData.emailOrPhone) {
      newErrors.emailOrPhone = "Email or phone is required.";
      isValid = false;
    } else if (
      !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.emailOrPhone) &&
      !/^\+\d{10,15}$/.test(formData.emailOrPhone)
    ) {
      newErrors.emailOrPhone =
        "Enter a valid email address or phone number (e.g., +1234567890).";
      isValid = false;
    }

    // Password Validation
    if (!formData.password) {
      newErrors.password = "Password is required.";
      isValid = false;
    }

    setErrors(newErrors);
    return isValid;
  };

  useEffect(() => {
    if (Object.keys(isTouched).length > 0) {
      validateForm();
    }
  }, [formData]);

  const handleChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    setIsTouched((prev) => ({ ...prev, [field]: true }));
  };

  const handleLogin = async () => {
    if (!validateForm()) return;

    setIsLoading(true);
    setLoginError(null);
    try {
      const response = await apis.authApi.login(formData); // Use formData directly

      if (response.success) {
        sessionStorage.setItem("token", response.token); // Adjust if API returns token
        router.push("/");
      } else {
        setLoginError(response.message || "Login failed. Please try again.");
      }
    } catch (error) {
      setLoginError("An unexpected error occurred. Please try again later.");
      console.error("Login failed:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Box style={styles.loginContainer}>
      <LoadingOverlay visible={isLoading} overlayBlur={2} />
      <VideoBackground
        videos={[
          { src: "/videos/3.mp4", type: "video/mp4" },
          { src: "/videos/4.mp4", type: "video/mp4" },
          { src: "/videos/1.mp4", type: "video/mp4" },
        ]}
      />
      <Center style={styles.centerContainer}>
        <Paper style={styles.paper}>
          <Text size="xl" weight={700} mb="lg" align="center">
            Login to Talenti
          </Text>

          <TextInput
            label="Email or Phone"
            placeholder="Enter your email or phone"
            value={formData.emailOrPhone}
            onChange={(e) => handleChange("emailOrPhone", e.target.value)}
            error={isTouched.emailOrPhone && errors.emailOrPhone}
            mb="md"
          />
          <PasswordInput
            label="Password"
            placeholder="Enter your password"
            value={formData.password}
            onChange={(e) => handleChange("password", e.target.value)}
            error={isTouched.password && errors.password}
            mb="md"
          />
          {loginError && (
            <Text size="sm" color="red" align="center" mb="sm">
              {loginError}
            </Text>
          )}
          <Button fullWidth mt="md" onClick={handleLogin}>
            Login
          </Button>
          <Text size="sm" align="center" mt="md" component="div">
            Don’t have an account?
            <Link style={styles.linkStyle} href="/register">
              <Text color="white" weight={700}>
                Register here
              </Text>
            </Link>
          </Text>
        </Paper>
      </Center>
    </Box>
  );
}
