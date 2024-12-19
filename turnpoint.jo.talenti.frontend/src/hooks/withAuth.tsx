import { useEffect } from "react";
import { useRouter } from "next/router";

export default function withAuth(Component: React.ComponentType) {
  return function AuthenticatedComponent(props: any) {
    const router = useRouter();

    useEffect(() => {
      const isAuthenticated = false; // Replace with your auth logic
      if (!isAuthenticated) {
        router.push("/login");
      }
    }, [router]);

    return <Component {...props} />;
  };
}
