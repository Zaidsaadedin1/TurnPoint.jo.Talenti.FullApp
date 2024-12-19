import React from "react";
import { useRouter } from "next/router";
import { Loader, Center, Text } from "@mantine/core";

// Mock function to get user permissions (replace with your real logic)
const getUserPermissions = () => {
  // Example: Fetch from a global state, context, or API
  return ["view_users", "view_roles"]; // Example permissions
};

// Mock function to check if a user is authenticated (replace with your real logic)
const isAuthenticated = () => {
  // Example: Check authentication state
  return true; // Replace with real authentication check
};

export const AuthGuard = ({
  children,
  requiredClaims = [],
  fallback = null, // Fallback content if user is not authorized
  loadingFallback = (
    <Center style={{ height: "100%" }}>
      <Loader />
    </Center>
  ),
}) => {
  const router = useRouter();
  const [loading, setLoading] = React.useState(true);
  const [isAuthorized, setIsAuthorized] = React.useState(false);

  React.useEffect(() => {
    const checkAuthorization = async () => {
      if (!isAuthenticated()) {
        // Redirect to login or other action
        router.push("/login");
        return;
      }

      const userPermissions = getUserPermissions();
      const hasRequiredClaims =
        requiredClaims.length === 0 ||
        requiredClaims.every((claim) => userPermissions.includes(claim));

      setIsAuthorized(hasRequiredClaims);
      setLoading(false);
    };

    checkAuthorization();
  }, [router, requiredClaims]);

  if (loading) return loadingFallback;

  if (!isAuthorized) {
    return (
      fallback || (
        <Center style={{ height: "100%" }}>
          <Text color="red">You are not authorized to view this content.</Text>
        </Center>
      )
    );
  }

  return <>{children}</>;
};
