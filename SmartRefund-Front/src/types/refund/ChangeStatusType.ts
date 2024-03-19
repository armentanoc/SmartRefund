export type ChangeStatusType = {
  uniqueHash: string;
  newStatus: number;
};

export type ChangeStatusResponseType = {
  uniqueHash: string;
  employeeId: number;
  total: number;
  category: number;
  status: number;
  description: string;
};
