"use client";

import { ReceiptDataType } from "@/types/refund/EventSourceType";
import { APP_ROUTES } from "@/utils/constants/app-routes";
import { ArrowBack, Edit } from "@mui/icons-material";
import { Button } from "@mui/material";
import { useRouter } from "next/navigation";

type ReceiptSideProps = {
  userType: string | undefined;
  receiptData: ReceiptDataType | undefined;
  setStatusModalOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

export const ReceiptHeader = ({
  userType,
  receiptData,
  setStatusModalOpen,
}: ReceiptSideProps) => {
  const { push } = useRouter();

  return (
    <div className="flex w-full justify-between gap-4">
      <Button variant="text" onClick={() => push(APP_ROUTES.private.refund)}>
        <ArrowBack />
        <span className="hidden md:inline">Voltar à Página Anterior</span>
        <span className="inline md:hidden">Voltar</span>
      </Button>
      {userType === process.env.NEXT_PUBLIC_API_TOKEN_FINANCE_EMPLOYE &&
        receiptData?.translatedVision.status === 1 && (
          <Button variant="contained" onClick={() => setStatusModalOpen(true)}>
            <Edit />
            <span className="hidden md:inline">
              Alterar status da solicitação
            </span>
            <span className="inline md:hidden">Alterar status</span>
          </Button>
        )}
    </div>
  );
};
