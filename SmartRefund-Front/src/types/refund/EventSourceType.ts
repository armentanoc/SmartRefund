export type ReceiptDataType = {
  internalReceipt: {
    uniqueHash: string;
    employeeId: number;
    creationDate: Date;
    status: number;
    image: string;
  };
  rawVision: {
    isReceipt: string;
    total: string;
    category: string;
    description: string;
  };
  translatedVision: {
    uniqueHash: string;
    employeeId: number;
    total: number;
    category: number;
    status: number;
    description: string;
  };
};

export type AllReceiptDataType = ReceiptDataType[];
