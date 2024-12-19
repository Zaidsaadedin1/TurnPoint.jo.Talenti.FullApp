import React from "react";
import {
  Drawer,
  Stack,
  TextInput,
  Button,
  Group,
  Select,
  MultiSelect,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { apis } from "@/backend-apis";

const UserForm = ({
  isDrawerOpen,
  setDrawerOpen,
  isLoading,
  setIsLoading,
  selectedUser,
  interests,
}) => {
  const form = useForm({
    initialValues: {
      Name: "",
      LastName: "",
      UserName: "",
      Email: "",
      PhoneNumber: "",
      Password: "",
      Gender: "",
      DateOfBirth: "",
      InterestIds: [],
    },
    validate: {
      Name: (value) =>
        !value
          ? "Name is required"
          : value.length > 50
          ? "Max 50 characters"
          : null,
      LastName: (value) =>
        !value
          ? "Last name is required"
          : value.length > 50
          ? "Max 50 characters"
          : null,
      UserName: (value) =>
        !value
          ? "Username is required"
          : value.length > 30
          ? "Max 30 characters"
          : null,
      Email: (value) =>
        !value
          ? "Email is required"
          : /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)
          ? null
          : "Invalid email format",
      PhoneNumber: (value) =>
        !value
          ? "Phone number is required"
          : /^\+\d{10,15}$/.test(value)
          ? null
          : "Invalid phone format (e.g., +1234567890)",
      Password: (value) =>
        !value
          ? "Password is required"
          : value.length < 8
          ? "Password must be at least 8 characters"
          : !/[A-Z]/.test(value)
          ? "Must contain an uppercase letter"
          : !/[a-z]/.test(value)
          ? "Must contain a lowercase letter"
          : !/\d/.test(value)
          ? "Must contain a number"
          : !/[^\w\d\s]/.test(value)
          ? "Must contain a special character"
          : null,
      Gender: (value) =>
        !value
          ? "Gender is required"
          : ["Male", "Female", "Other"].includes(value)
          ? null
          : "Invalid gender",
      DateOfBirth: (value) =>
        !value
          ? "Date of Birth is required"
          : new Date(value) >
            new Date(Date.now() - 13 * 365 * 24 * 60 * 60 * 1000)
          ? "Must be at least 13 years old"
          : null,
      InterestIds: (value) =>
        value.length === 0 ? "At least one interest is required" : null,
    },
  });

  React.useEffect(() => {
    if (selectedUser) {
      form.setValues({
        Name: selectedUser.name,
        LastName: selectedUser.lastName,
        UserName: selectedUser.username,
        Email: selectedUser.email,
        PhoneNumber: selectedUser.phoneNumber,
        Password: "",
        Gender: selectedUser.gender,
        DateOfBirth: selectedUser.dateOfBirth,
        InterestIds: selectedUser.userInterests || [],
      });
    }
  }, [selectedUser]);

  const handleSubmit = async (userData) => {
    setIsLoading(true);
    try {
      if (selectedUser) {
        await apis.profileApi.updateUser(selectedUser.id, userData);
      } else {
        await apis.authApi.register(form.values);
        setIsLoading(false);
      }
      setDrawerOpen(false);
      router.reload();
    } catch (error) {
      console.error("Error saving user:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Drawer
      opened={isDrawerOpen}
      onClose={() => setDrawerOpen(false)}
      padding="md"
      title={selectedUser ? "Edit User" : "Add User"}
    >
      <form onSubmit={form.onSubmit(handleSubmit)}>
        <Stack>
          <TextInput
            label="Name"
            {...form.getInputProps("Name")}
            onChange={(event) =>
              form.setFieldValue("Name", event.currentTarget.value)
            }
            onBlur={() => form.validateField("Name")}
            error={form.errors.Name}
          />
          <TextInput
            label="Last Name"
            {...form.getInputProps("LastName")}
            onChange={(event) =>
              form.setFieldValue("LastName", event.currentTarget.value)
            }
            onBlur={() => form.validateField("LastName")}
            error={form.errors.LastName}
          />
          <TextInput
            label="Username"
            {...form.getInputProps("UserName")}
            onChange={(event) =>
              form.setFieldValue("UserName", event.currentTarget.value)
            }
            onBlur={() => form.validateField("UserName")}
            error={form.errors.UserName}
          />
          <TextInput
            label="Email"
            {...form.getInputProps("Email")}
            onChange={(event) =>
              form.setFieldValue("Email", event.currentTarget.value)
            }
            onBlur={() => form.validateField("Email")}
            error={form.errors.Email}
          />
          <TextInput
            label="Phone Number"
            {...form.getInputProps("PhoneNumber")}
            onChange={(event) =>
              form.setFieldValue("PhoneNumber", event.currentTarget.value)
            }
            onBlur={() => form.validateField("PhoneNumber")}
            error={form.errors.PhoneNumber}
          />
          <TextInput
            label="Password"
            type="password"
            {...form.getInputProps("Password")}
            onChange={(event) =>
              form.setFieldValue("Password", event.currentTarget.value)
            }
            onBlur={() => form.validateField("Password")}
            error={form.errors.Password}
          />
          <Select
            label="Gender"
            data={["Male", "Female", "Other"]}
            {...form.getInputProps("Gender")}
            onChange={(value) => form.setFieldValue("Gender", value)}
            onBlur={() => form.validateField("Gender")}
            error={form.errors.Gender}
          />
          <TextInput
            label="Date of Birth"
            type="date"
            {...form.getInputProps("DateOfBirth")}
            onChange={(event) =>
              form.setFieldValue("DateOfBirth", event.currentTarget.value)
            }
            onBlur={() => form.validateField("DateOfBirth")}
            error={form.errors.DateOfBirth}
          />
          <MultiSelect
            label="Interests"
            placeholder="Select your interests"
            data={interests}
            {...form.getInputProps("InterestIds")}
            onChange={(value) => form.setFieldValue("InterestIds", value)}
            onBlur={() => form.validateField("InterestIds")}
            error={form.errors.InterestIds}
          />
          <Group position="right">
            <Button type="submit" loading={isLoading}>
              {selectedUser ? "Update User" : "Add User"}
            </Button>
          </Group>
        </Stack>
      </form>
    </Drawer>
  );
};

export default UserForm;
