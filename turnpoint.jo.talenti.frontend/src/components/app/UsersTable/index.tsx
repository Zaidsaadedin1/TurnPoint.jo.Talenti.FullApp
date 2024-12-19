import React, { useState } from "react";
import { Stack, Drawer } from "@mantine/core";
import DynamicTable from "@/components/blocks/DynamicTable";
import HomeNavbar from "../HomePage/HomeNavbar";
import UserForm from "./UserForm/UserForm";
import { IconEdit, IconEye, IconTrash } from "@tabler/icons-react";

const UsersTable = ({ users = [], interests = [] }) => {
  const [isDrawerOpen, setDrawerOpen] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [isLoading, setIsLoading] = useState<bool>(false);
  const handleDelete = (row: GetUserDto) => {
    console.log("Delete User:", row);
  };
  const handleAdd = () => {
    setSelectedUser(null);
    setDrawerOpen(true);
  };
  const handleEdit = (user) => {
    setSelectedUser(user);
    setDrawerOpen(true);
  };
  const handleView = (row: GetUserDto) => {
    setSelectedUser(row);
    console.log("View User:", row);
  };
  const actions = [
    { icon: <IconEye size="20px" />, color: "blue", action: handleView },
    { icon: <IconEdit size="20px" />, color: "green", action: handleEdit },
    { icon: <IconTrash size="20px" />, color: "red", action: handleDelete },
  ];
  const columns = [
    { accessor: "id", header: "ID" },
    { accessor: "name", header: "Name" },
    { accessor: "lastName", header: "Last Name" },
    { accessor: "email", header: "Email" },
    { accessor: "phoneNumber", header: "Phone Number" },
    { accessor: "dateOfBirth", header: "Date Of Birth" },
    { accessor: "gender", header: "Gender" },
    { accessor: "createdAt", header: "Created At" },
    { accessor: "updatedAt", header: "Updated At" },
    { accessor: "deletedAt", header: "Deleted At" },
    { accessor: "userInterests", header: "User Interests" },
  ];

  return (
    <Stack>
      <HomeNavbar />
      <DynamicTable
        title="User Table"
        buttonTitle="Add User"
        columns={columns}
        data={users}
        onAdd={handleAdd}
        actions={actions}
      />

      <Drawer
        opened={isDrawerOpen}
        onClose={() => setDrawerOpen(false)}
        padding="md"
        title={selectedUser ? "Edit User" : "Add User"}
      >
        <UserForm
          isDrawerOpen={isDrawerOpen}
          setDrawerOpen={setDrawerOpen}
          isLoading={isLoading}
          setIsLoading={setIsLoading}
          selectedUser={selectedUser}
          interests={interests}
        />
      </Drawer>
    </Stack>
  );
};

export default UsersTable;
