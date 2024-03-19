"use client";

import { ReceiptHeader } from "@/components/refundPage/hashPage/ReceiptHeader";
import { ReceiptInfo } from "@/components/refundPage/hashPage/ReceiptInfoSide";
import { EventSourceServices } from "@/services/recipt/eventSource_services";
import { ReceiptDataType } from "@/types/refund/EventSourceType";
import { getCookie } from "@/utils/helpers/manageCookies";
import React from "react";

export default function Hash({ params }: { params: { hash: string } }) {
  const [statusModalOpen, setStatusModalOpen] = React.useState<boolean>(false);
  const [userType, setUserType] = React.useState<string | undefined>();
  const [receiptData, setReceiptData] = React.useState<ReceiptDataType>();

  const fetchUserTypeData = async () => {
    try {
      const user_type = await getCookie();
      setUserType(user_type.userType?.value);
    } catch (error) {
      console.error(error);
    }
  };

  const fetchReceiptData = async () => {
    try {
      const data = await EventSourceServices.getReceiptByHash(params.hash);
      setReceiptData(data.data);
    } catch (error) {
      console.error("Erro ao buscar notas fiscais:", error);
    }
  };

  React.useEffect(() => {
    fetchUserTypeData();
    fetchReceiptData();
  }, []);

  return (
    <main
      className="flex flex-col items-start gap-4 bg-slate-50 p-4 md:p-8"
      style={{ gridArea: "main" }}
    >
      <ReceiptHeader
        userType={userType}
        receiptData={receiptData}
        setStatusModalOpen={setStatusModalOpen}
      />
      <ReceiptInfo
        receiptData={receiptData}
        fetchReceiptData={fetchReceiptData}
        statusModalOpen={statusModalOpen}
        setStatusModalOpen={setStatusModalOpen}
        hash={params.hash}
      />
    </main>
  );
}
