import { apis } from "@/backend-apis";
import Register from "@/components/app/Register";

export default Register;

export async function getServerSideProps() {
  try {
    const rawResponse = await apis.interestsApi.getAllInterests();
    console.error("Raw interests response", rawResponse);

    const interests = rawResponse?.data || [];

    const transformedInterests = Array.isArray(interests)
      ? interests.map((interest) => ({
          value: String(interest.id),
          label: interest.name,
        }))
      : [];

    console.error("Transformed interests", transformedInterests);

    return { props: { interests: transformedInterests } };
  } catch (error) {
    console.error("Error fetching interests:", error);
    return { props: { interests: [] } };
  }
}
