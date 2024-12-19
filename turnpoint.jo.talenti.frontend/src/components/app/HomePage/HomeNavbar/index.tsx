import React, { useState } from "react";
import { Group, Menu, Text, UnstyledButton, Box } from "@mantine/core";
import { useRouter } from "next/router";
import { IconDropletDown, IconLogout } from "@tabler/icons-react";
import MainMenu from "../MainMenu";

const styles = {
  navbar: {
    background: "rgba(253, 253, 253, 0.415)",
    padding: "1rem",
  },
  menuButton: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    position: "fixed",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    zIndex: 1000,
  },
  overlay: {
    position: "fixed",
    top: 0,
    left: 0,
    width: "100%",
    height: "100%",
    backgroundColor: "rgba(0, 0, 0, 0.5)",
    zIndex: 999,
  },
};

export default function HomeNavbar() {
  const [menuOpen, setMenuOpen] = useState(false);
  const router = useRouter();

  const handleLogout = () => {
    sessionStorage.removeItem("token");
    router.push("/login");
  };

  return (
    <Box style={styles.navbar}>
      <Group justify="space-between" gap="xl" position="center">
        <Menu shadow="md">
          <Menu.Target>
            <UnstyledButton>
              <Group spacing={5}>
                <Text size="sm">Admin</Text>
                <IconDropletDown size={16} />
              </Group>
            </UnstyledButton>
          </Menu.Target>

          <Menu.Dropdown>
            <Menu.Item>Language</Menu.Item>
            <Menu.Item>Profile</Menu.Item>
          </Menu.Dropdown>
        </Menu>
        <UnstyledButton mr="xl" onClick={() => setMenuOpen(true)}>
          <Text color="black" content="Main Menu">
            Main Menu
          </Text>
        </UnstyledButton>
        <UnstyledButton color="red" onClick={handleLogout}>
          <IconLogout size={20} />
        </UnstyledButton>
      </Group>

      {menuOpen && (
        <MainMenu isOpen={menuOpen} onClose={() => setMenuOpen(false)} />
      )}
    </Box>
  );
}
