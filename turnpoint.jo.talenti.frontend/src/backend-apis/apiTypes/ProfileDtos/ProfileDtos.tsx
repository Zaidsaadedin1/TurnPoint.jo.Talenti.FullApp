export type GetUserDto = {
  Id: number;
  Name: string;
  LastName: string;
  Email: string;
  PasswordHash: string;
  PhoneNumber: string;
  DateOfBirth: Date | null;
  Gender: string;
  CreatedAt: Date;
  UpdatedAt: Date | null;
  DeletedAt: Date | null;
  UserInterests: GetInterestDto[];
};

export type UpdateUserDto = {
  Name: string;
  LastName: string;
  DateOfBirth: Date | null;
  Gender: string;
  Email: string;
  PhoneNumber: string;
  CreatedAt: Date;
  UpdatedAt: Date;
  DeletedAt: Date;
  InterestIds: number[];
};
