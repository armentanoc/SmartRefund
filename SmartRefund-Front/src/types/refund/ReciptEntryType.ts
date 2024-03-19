export type ReceipEntryType = {
  file: File;
};

export type ReceipEntryResponseType = {
  uniqueHash: string;
  employeeId: number;
  creationDate: Date;
  status: number;
  image: string;
};
