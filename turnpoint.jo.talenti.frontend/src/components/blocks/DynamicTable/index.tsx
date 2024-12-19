import React from "react";
import {
  Table,
  Button,
  Group,
  ActionIcon,
  Title,
  Stack,
  Paper,
  Center,
} from "@mantine/core";
import { isValid, parseISO, format } from "date-fns";

interface Column {
  accessor: string;
  header: string;
}

interface Action {
  icon: React.ReactNode;
  color: string;
  action: (row: Any) => void;
}

interface DynamicTableProps {
  columns: Column[];
  data: Any[];
  onAdd: () => void;
  actions?: Action[];
  title: string;
  buttonTitle: string;
}

const DynamicTable: React.FC<DynamicTableProps> = ({
  columns,
  data,
  onAdd,
  actions,
  title,
  buttonTitle,
}) => {
  const formatValue = (value: Any): string => {
    if (typeof value === "string" && isValid(parseISO(value))) {
      return format(parseISO(value), "yyyy-MM-dd");
    }
    return value ?? "N/A";
  };

  return (
    <Stack>
      <Center>
        <Paper>
          <Group position="apart" mb="md" justify="space-between">
            <Title order={5}> {title}</Title>
            <Button size="xs" onClick={onAdd}>
              {buttonTitle}
            </Button>
          </Group>

          <Table
            withColumnBorders
            withRowBorders
            striped
            highlightOnHover
            withBorder
          >
            <Table.Thead>
              <Table.Tr>
                {columns.map((col) => (
                  <Table.Th key={col.accessor}>{col.header}</Table.Th>
                ))}
                {actions && <Table.Th>Actions</Table.Th>}
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {data?.length ? (
                data.map((row, rowIndex) => (
                  <Table.Tr key={rowIndex}>
                    {columns.map((col) => (
                      <Table.Td key={col.accessor}>
                        {formatValue(row[col.accessor])}
                      </Table.Td>
                    ))}
                    {actions && (
                      <Table.Td>
                        <Group>
                          {actions.map((action, actionIndex) => (
                            <ActionIcon
                              key={actionIndex}
                              color={action.color}
                              variant="transparent"
                              onClick={() => action.action(row)}
                            >
                              {action.icon}
                            </ActionIcon>
                          ))}
                        </Group>
                      </Table.Td>
                    )}
                  </Table.Tr>
                ))
              ) : (
                <Table.Tr>
                  <Table.Td colSpan={columns.length + (actions ? 1 : 0)}>
                    No data available.
                  </Table.Td>
                </Table.Tr>
              )}
            </Table.Tbody>
          </Table>
        </Paper>
      </Center>
    </Stack>
  );
};

export default DynamicTable;
