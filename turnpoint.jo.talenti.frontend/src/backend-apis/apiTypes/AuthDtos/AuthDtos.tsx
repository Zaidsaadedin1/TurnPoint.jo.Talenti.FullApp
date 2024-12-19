export type LoginUserDto = {
  EmailOrPhone: string;
  Password: string;
};

export type ResetPasswordDto = {
  EmailOrPhone: string;
  Opt: string;
  NewPassword: string;
  ConfirmedPassword: string;
};

export type RegisterUserDto = {
  Name: string;
  LastName: string;
  UserName: string;
  Email: string;
  PhoneNumber: string;
  Password: string;
  Gender: string;
  DateOfBirth: Date | null;
  InterestIds: number[];
};
