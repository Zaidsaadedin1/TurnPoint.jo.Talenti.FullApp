import {
  Box,
  Center,
  Text,
  Button,
  TextInput,
  PasswordInput,
  Paper,
  Stepper,
  MultiSelect,
  Group,
  LoadingOverlay,
  Select,
} from "@mantine/core";
import { useEffect, useState } from "react";
import Link from "next/link";
import { IconHeart, IconUser, IconX } from "@tabler/icons-react";
import { RegisterUserDto } from "@/backend-apis/apiTypes/AuthDtos/AuthDtos";
import { useRouter } from "next/router";
import { apis } from "@/backend-apis";
import { DatePickerInput } from "@mantine/dates";
import VideoBackground from "@/components/blocks/VideoBackground";

const styles = {
  registerContainer: {
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
  stepperContainer: {
    maxHeight: "80vh",
    overflowY: "auto",
    scrollbarWidth: "thin",
    scrollbarColor: "rgba(218, 213, 213, 0.2) transparent",
  },
  "::-webkit-scrollbar": {
    width: "6px",
  },
  "::-webkit-scrollbar-track": {
    background: "transparent",
  },
  "::-webkit-scrollbar-thumb": {
    background: "black",
    borderRadius: "10px",
  },
  linkStyle: { textDecoration: "none", color: "inherit" },
};

export default function Register({ interests }) {
  const [activeStep, setActiveStep] = useState(0);
  const [formData, setFormData] = useState<RegisterUserDto>({
    Name: "",
    LastName: "",
    UserName: "",
    Email: "",
    PhoneNumber: "",
    Password: "",
    Gender: "",
    DateOfBirth: null,
    InterestIds: [],
  });
  const [errors, setErrors] = useState<Any>({});
  const [isTouched, setIsTouched] = useState<Any>({});
  const [isLoading, setIsLoading] = useState(false);
  const router = useRouter();

  const validateStep = (): boolean => {
    const newErrors: Any = {};
    let isValid = true;

    // Step 1 Validations
    if (activeStep === 0) {
      // Name Validation
      if (!formData.Name) {
        newErrors.Name = "Name is required.";
        isValid = false;
      } else if (formData.Name.length > 50) {
        newErrors.Name = "Name cannot exceed 50 characters.";
        isValid = false;
      }

      // Last Name Validation
      if (!formData.LastName) {
        newErrors.LastName = "Last name is required.";
        isValid = false;
      } else if (formData.LastName.length > 50) {
        newErrors.LastName = "Last name cannot exceed 50 characters.";
        isValid = false;
      }

      // Username Validation
      if (!formData.UserName) {
        newErrors.UserName = "Username is required.";
        isValid = false;
      } else if (formData.UserName.length > 30) {
        newErrors.UserName = "Username cannot exceed 30 characters.";
        isValid = false;
      }

      // Email Validation
      if (!formData.Email) {
        newErrors.Email = "Email is required.";
        isValid = false;
      } else if (
        !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.Email) // Matches FluentValidation regex
      ) {
        newErrors.Email = "Invalid email format.";
        isValid = false;
      }

      // Phone Number Validation
      if (!formData.PhoneNumber) {
        newErrors.PhoneNumber = "Phone number is required.";
        isValid = false;
      } else if (!/^\+\d{10,15}$/.test(formData.PhoneNumber)) {
        newErrors.PhoneNumber =
          "Invalid phone number format. Include the country code (e.g., +1234567890).";
        isValid = false;
      }

      // Password Validation
      if (!formData.Password) {
        newErrors.Password = "Password is required.";
        isValid = false;
      } else if (formData.Password.length < 8) {
        newErrors.Password = "Password must be at least 8 characters long.";
        isValid = false;
      } else if (!/[A-Z]/.test(formData.Password)) {
        newErrors.Password =
          "Password must contain at least one uppercase letter.";
        isValid = false;
      } else if (!/[a-z]/.test(formData.Password)) {
        newErrors.Password =
          "Password must contain at least one lowercase letter.";
        isValid = false;
      } else if (!/\d/.test(formData.Password)) {
        newErrors.Password = "Password must contain at least one number.";
        isValid = false;
      } else if (!/[^\w\d\s]/.test(formData.Password)) {
        newErrors.Password =
          "Password must contain at least one special character.";
        isValid = false;
      }

      // Gender Validation
      if (!formData.Gender) {
        newErrors.Gender = "Gender is required.";
        isValid = false;
      } else if (!["Male", "Female", "Other"].includes(formData.Gender)) {
        newErrors.Gender = "Gender must be 'Male', 'Female', or 'Other'.";
        isValid = false;
      }

      // Date of Birth Validation
      if (!formData.DateOfBirth) {
        newErrors.DateOfBirth = "Date of Birth is required.";
        isValid = false;
      } else if (
        new Date(formData.DateOfBirth) >
        new Date(Date.now() - 13 * 365 * 24 * 60 * 60 * 1000)
      ) {
        newErrors.DateOfBirth = "User must be at least 13 years old.";
        isValid = false;
      }
    }

    // Step 2 Validations
    if (activeStep === 1) {
      if (formData.InterestIds.length === 0) {
        newErrors.InterestIds = "Please select at least one interest.";
        isValid = false;
      }
    }

    setErrors(newErrors);
    return isValid;
  };

  useEffect(() => {
    if (Object.keys(isTouched).length > 0) {
      validateStep();
    }
  }, [formData]);

  const handleChange = (field: string, value: Any) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    setIsTouched((prev) => ({ ...prev, [field]: true }));
  };

  const handleNext = () => {
    if (validateStep()) {
      setActiveStep((prev) => prev + 1);
    }
  };

  const handlePrevious = () => {
    setActiveStep((prev) => prev - 1);
  };

  const handleRegister = async () => {
    if (!validateStep()) return;

    setIsLoading(true);
    try {
      const registerData: RegisterUserDto = formData;
      await apis.authApi.register(registerData);
      setIsLoading(false);
      router.push("/login");
    } catch (error) {
      setIsLoading(false);
      console.error("Registration failed:", error);
    }
  };

  return (
    <Box style={styles.registerContainer}>
      <LoadingOverlay visible={isLoading} overlayBlur={2} />
      <VideoBackground
        videos={[{ src: "/videos/video1.mp4", type: "video/mp4" }]}
      />{" "}
      <Center style={styles.centerContainer}>
        <Paper style={styles.paper}>
          <Text size="xl" weight={700} mb="lg" align="center">
            Register to Talenti
          </Text>
          <Stepper
            active={activeStep}
            onStepClick={setActiveStep}
            breakpoint="sm"
            style={styles.stepperContainer}
          >
            <Stepper.Step
              icon={
                errors.Email || errors.Gender || errors.PhoneNumber ? (
                  <IconX stroke={2} color="red" />
                ) : (
                  <IconUser stroke={2} />
                )
              }
              label="Step 1"
              description="User Details"
            >
              <TextInput
                label="Name"
                placeholder="Enter your name"
                value={formData.Name}
                onChange={(e) => handleChange("Name", e.target.value)}
                error={errors.Name}
                mb="md"
              />
              <TextInput
                label="Last Name"
                placeholder="Enter your last name"
                value={formData.LastName}
                onChange={(e) => handleChange("LastName", e.target.value)}
                error={errors.LastName}
                mb="md"
              />
              <TextInput
                label="Username"
                placeholder="Enter your username"
                value={formData.UserName}
                onChange={(e) => handleChange("UserName", e.target.value)}
                error={errors.UserName}
                mb="md"
              />
              <TextInput
                label="Phone Number"
                placeholder="Enter your phone number"
                value={formData.PhoneNumber}
                onChange={(e) => handleChange("PhoneNumber", e.target.value)}
                error={errors.PhoneNumber}
                mb="md"
              />
              <TextInput
                label="Email"
                placeholder="Enter your email"
                value={formData.Email}
                onChange={(e) => handleChange("Email", e.target.value)}
                error={errors.Email}
                mb="md"
              />
              <PasswordInput
                label="Password"
                placeholder="Enter your password"
                value={formData.Password}
                onChange={(e) => handleChange("Password", e.target.value)}
                error={errors.Password}
                mb="md"
              />
              <Select
                label="Gender"
                placeholder="Select your gender"
                data={[
                  { value: "Male", label: "Male" },
                  { value: "Female", label: "Female" },
                  { value: "Other", label: "Other" },
                ]}
                value={formData.Gender}
                onChange={(value) => handleChange("Gender", value)}
                error={errors.Gender}
              />
              <DatePickerInput
                label="Date of Birth"
                placeholder="Pick your date of birth"
                value={formData.DateOfBirth}
                onChange={(value) => handleChange("DateOfBirth", value)}
                error={errors.DateOfBirth}
                mb="md"
              />
              <Button fullWidth mt="md" onClick={handleNext}>
                Next
              </Button>
              <Text size="sm" align="center" mt="md" component="div">
                have an account?
                <Link style={styles.linkStyle} href="/login">
                  <Text color="white" weight={700}>
                    Login here
                  </Text>
                </Link>
              </Text>
            </Stepper.Step>
            <Stepper.Step
              icon={
                errors.InterestIds ? (
                  <IconX stroke={2} color="red" />
                ) : (
                  <IconHeart stroke={2} />
                )
              }
              label="Step 2"
              description="Select Interests"
            >
              <MultiSelect
                label="Interests"
                placeholder="Select your interests"
                data={interests}
                value={formData.InterestIds}
                onChange={(value) => handleChange("InterestIds", value)}
                error={errors.InterestIds}
                mb="md"
              />
              <Group position="apart" mt="md">
                <Button variant="default" onClick={handlePrevious}>
                  Back
                </Button>
                <Button onClick={handleRegister}>Register</Button>
              </Group>
            </Stepper.Step>
          </Stepper>
        </Paper>
      </Center>
    </Box>
  );
}
