import axios from "axios";
import {
  LoginUserDto,
  RegisterUserDto,
  ResetPasswordDto,
} from "./apiTypes/AuthDtos/AuthDtos";
import { GetInterestDto } from "./apiTypes/InterestDtos/InterestDtos";
import { GetUserDto, UpdateUserDto } from "./apiTypes/ProfileDtos/ProfileDtos";
import { VerifyOtpDto } from "./apiTypes/VerificationsDtos/VerificationsDtos";

export interface GenericResponse<T> {
  success: boolean;
  message: string;
  data: T | null;
}

const API_URL = "http://localhost:5148/api";

export const apis = {
  authApi: {
    register: async (registerUserDto: RegisterUserDto) => {
      try {
        const response = await axios.post<GenericResponse<boolean>>(
          `${API_URL}/Authentication/Register`,
          registerUserDto
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to register user"
        );
      }
    },

    login: async (loginDto: LoginUserDto) => {
      try {
        const response = await axios.post<GenericResponse<string>>(
          `${API_URL}/Authentication/Login`,
          loginDto
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to login user"
        );
      }
    },

    resetPassword: async (resetPasswordDto: ResetPasswordDto) => {
      try {
        const response = await axios.post<GenericResponse<boolean>>(
          `${API_URL}/Authentication/ResetPassword`,
          resetPasswordDto
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to reset password"
        );
      }
    },
  },
  interestsApi: {
    getAllInterests: async () => {
      try {
        const response = await axios.get<GenericResponse<GetInterestDto[]>>(
          `${API_URL}/interests/GetAllInterestsAsync`
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to fetch interests"
        );
      }
    },

    getInterestById: async (interestId: number) => {
      try {
        const response = await axios.get<GenericResponse<GetInterestDto>>(
          `${API_URL}/interests/GetInterestByIdAsync`,
          {
            params: { interestId },
          }
        );
        return response.data;
      } catch (error) {
        throw new Error(error.response?.data?.message || "Interest not found");
      }
    },

    addInterest: async (newInterest: string) => {
      try {
        const response = await axios.post<GenericResponse<boolean>>(
          `${API_URL}/interests/AddInterestAsync`,
          { newInterest }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to add interest"
        );
      }
    },

    updateInterest: async (interestId: number, updatedInterestName: string) => {
      try {
        const response = await axios.put<GenericResponse<boolean>>(
          `${API_URL}/interests/UpdateInterestAsync`,
          { updatedInterestName },
          { params: { interestId } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to update interest"
        );
      }
    },

    deleteInterest: async (interestId: number) => {
      try {
        const response = await axios.delete<GenericResponse<boolean>>(
          `${API_URL}/interests/DeleteInterestAsync`,
          { params: { interestId } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to delete interest"
        );
      }
    },
  },
  userInterestsApi: {
    addInterestsToUser: async (userId: number, newInterests: number[]) => {
      try {
        const response = await axios.post<GenericResponse<boolean>>(
          `${API_URL}/userinterests/AddInterestsToUser`,
          newInterests,
          { params: { userId } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to add interests to user"
        );
      }
    },

    removeInterestsFromUser: async (userId: number, interestIds: number[]) => {
      try {
        const response = await axios.delete<GenericResponse<boolean>>(
          `${API_URL}/userinterests/RemoveInterestsFromUser`,
          { params: { userId, interestIds } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message ||
            "Failed to remove interests from user"
        );
      }
    },

    getUserInterests: async (userId: number) => {
      try {
        const response = await axios.get<GenericResponse<number[]>>(
          `${API_URL}/userinterests/GetUserInterests`,
          { params: { userId } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to fetch user interests"
        );
      }
    },
  },
  otpApi: {
    sendOtp: async (emailOrPhone: string) => {
      try {
        const response = await axios.post<GenericResponse<boolean>>(
          `${API_URL}/otp/SendOtp`,
          { emailOrPhone }
        );
        return response.data;
      } catch (error) {
        throw new Error(error.response?.data?.message || "Failed to send OTP");
      }
    },

    verifyOtp: async (verifyOtpDto: VerifyOtpDto) => {
      try {
        const response = await axios.post<GenericResponse<boolean>>(
          `${API_URL}/otp/VerifyOtp`,
          verifyOtpDto
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to verify OTP"
        );
      }
    },
  },
  profileApi: {
    getUserById: async (userId: number) => {
      try {
        const response = await axios.get<GenericResponse<GetUserDto>>(
          `${API_URL}/Profiles/GetUserById`,
          { params: { userId } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to fetch user"
        );
      }
    },

    getAllUsers: async (page: number = 1, pageSize: number = 10) => {
      try {
        const response = await axios.get<GenericResponse<GetUserDto[]>>(
          `${API_URL}/Profiles/GetAllUsers`,
          { params: { page, pageSize } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to fetch users"
        );
      }
    },

    updateUser: async (updateUserDto: UpdateUserDto, userId: number) => {
      try {
        const response = await axios.put<GenericResponse<boolean>>(
          `${API_URL}/Profiles/UpdateUser`,
          updateUserDto,
          { params: { userId } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to update user"
        );
      }
    },

    deleteUser: async (userId: number) => {
      try {
        const response = await axios.delete<GenericResponse<boolean>>(
          `${API_URL}/Profiles/DeleteUser`,
          { params: { userId } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message || "Failed to delete user"
        );
      }
    },

    checkIfEmailOrPhoneIsTaken: async (emailOrPhone: string) => {
      try {
        const response = await axios.get<GenericResponse<boolean>>(
          `${API_URL}/Profiles/CheckIfEmailOrPhoneIsTaken`,
          { params: { emailOrPhone } }
        );
        return response.data;
      } catch (error) {
        throw new Error(
          error.response?.data?.message ||
            "Failed to check if email or phone is taken"
        );
      }
    },
  },
};
