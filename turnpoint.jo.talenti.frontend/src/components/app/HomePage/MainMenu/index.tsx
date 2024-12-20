import { useState } from "react";
import {
  ActionIcon,
  Box,
  Group,
  Modal,
  Paper,
  SimpleGrid,
  Text,
  Title,
  UnstyledButton,
} from "@mantine/core";
import { IconStackBackward } from "@tabler/icons-react";
import { useRouter } from "next/router";

type Props = {
  isOpen: boolean;
  onClose: () => void;
};

export default function MainMenu({ isOpen, onClose }: Props) {
  const [selectedItem, setSelectedItem] = useState<any | null>(null);
  const router = useRouter();

  const onModalClose = () => {
    setSelectedItem(null);
    onClose();
  };

  const styles = {
    subLink: {
      width: "100%",
      padding: "8px 16px",
      borderRadius: "8px",
      cursor: "pointer",
      backgroundColor: "#f8f9fa",
      "&:hover": {
        backgroundColor: "#e9ecef",
      },
    },
    modal: {
      backgroundColor: "rgba(255, 255, 255, 0.6)",
      position: "relative",
    },
    detailsWrapper: {
      padding: "16px",
      backgroundColor: "#fff",
      borderRadius: "8px",
      boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.1)",
    },
    closeButton: {
      position: "absolute",
      top: 12,
      right: 12,
    },
  };

  const mainLinksConfig = [
    {
      id: "users",
      title: "Users",
      description: "Manage users",
      icon: IconStackBackward,
      subMenu: [
        {
          id: "usersTable",
          title: "UsersTable",
          description: "Manage Users",
          path: "/users-table",
        },
        {
          id: "usersInterests",
          title: "Users Interests",
          description: "Manage Users Interests",
          path: "/users-interests",
        },
        {
          id: "interests",
          title: "Interests",
          description: "Manage Interests",
          path: "/users-interests-list",
        },
      ],
    },
    {
      id: "roles",
      title: "Roles",
      description: "Manage roles",
      icon: IconStackBackward,
      subMenu: [
        {
          id: "role-management",
          title: "Role Management",
          description: "Create, delete, and update roles",
          path: "/roles/management",
        },
        {
          id: "user-assignment",
          title: "User Assignment",
          description: "Assign or remove users from roles",
          path: "/roles/user-assignment",
        },
        {
          id: "role-permissions",
          title: "Role Permissions",
          description: "Define and manage role permissions",
          path: "/roles/permissions",
        },
      ],
    },
  ];

  return (
    <Modal
      opened={isOpen}
      onClose={onModalClose}
      size="lg"
      overlayColor="#f8f9fa"
      overlayOpacity={0.8}
      overlayBlur={3}
      shadow="md"
      withCloseButton={false}
      style={styles.modal}
    >
      <SimpleGrid cols={3}>
        {mainLinksConfig.map((item) => (
          <div
            key={item.id}
            onClick={() => setSelectedItem(item)}
            style={{ cursor: "pointer" }}
          >
            <Paper p="lg" shadow="md">
              <Group noWrap position="apart">
                <Box>
                  <Title order={6}>{item.title}</Title>
                  <Text size="xs" color="dimmed">
                    {item.description}
                  </Text>
                </Box>
                <item.icon size={32} stroke={1.5} />
              </Group>
            </Paper>
          </div>
        ))}
      </SimpleGrid>
      {selectedItem && (
        <div style={styles.detailsWrapper}>
          <ActionIcon
            style={styles.closeButton}
            onClick={() => setSelectedItem(null)}
          >
            <IconStackBackward size={22} />
          </ActionIcon>
          {selectedItem.subMenu &&
            selectedItem.subMenu.map((subItem) => (
              <UnstyledButton
                key={subItem.id}
                style={styles.subLink}
                onClick={() => router.push(subItem.path)}
              >
                <Group noWrap align="flex-start">
                  <div>
                    <Text size="sm" weight={500}>
                      {subItem.title}
                    </Text>
                    <Text size="xs" color="dimmed">
                      {subItem.description}
                    </Text>
                  </div>
                </Group>
              </UnstyledButton>
            ))}
        </div>
      )}
    </Modal>
  );
}
