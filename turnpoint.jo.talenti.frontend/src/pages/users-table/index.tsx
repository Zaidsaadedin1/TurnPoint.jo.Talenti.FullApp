import { apis } from "@/backend-apis";
import { GetInterestDto } from "@/backend-apis/apiTypes/InterestDtos/InterestDtos";
import { GetUserDto } from "@/backend-apis/apiTypes/ProfileDtos/ProfileDtos";
import UsersTable from "@/components/app/UsersTable";

export default UsersTable;

export const getServerSideProps = async () => {
  try {
    const allUsers: GetUserDto[] = await apis.profileApi.getAllUsers();
    const allInterests: GetInterestDto[] =
      await apis.interestsApi.getAllInterests();

    const allUserInterests = allInterests?.data || [];

    return {
      props: {
        users: allUsers,
        interests: allUserInterests,
      },
    };
  } catch (error) {
    console.error("Error fetching users or interests:", error);
    return {
      props: {
        users: [],
        interests: [],
      },
    };
  }
};
