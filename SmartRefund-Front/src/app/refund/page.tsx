"use client";

import { AsideFilter } from "@/components/refundPage/AsideFilter";
import { MainSection } from "@/components/refundPage/MainSection";
import { EventSourceServices } from "@/services/recipt/eventSource_services";
import { AllReceiptDataType } from "@/types/refund/EventSourceType";
import { FetchReceiptsDataOptions } from "@/types/refund/ReciptValidationType";
import React from "react";

export default function Refund() {
  const [receiptsData, setReceiptsData] = React.useState<AllReceiptDataType>();
  const [openFilterMenu, setOpenFilterMenu] = React.useState<boolean>(false);

  const fetchReceiptsData = async (options?: FetchReceiptsDataOptions) => {
    try {
      const data = await EventSourceServices.getAllReceipts(options);
      setReceiptsData(data.data);
    } catch (error) {
      console.error("Erro ao buscar notas fiscais:", error);
    }
  };

  React.useEffect(() => {
    fetchReceiptsData();
  }, []);

  return (
    <main className="refund-grid-container bg-slate-50">
      <AsideFilter
        fetchReceiptsData={fetchReceiptsData}
        openFilterMenu={openFilterMenu}
        setOpenFilterMenu={setOpenFilterMenu}
      />
      <MainSection
        receiptsData={receiptsData}
        fetchReceiptsData={fetchReceiptsData}
        openFilterMenu={openFilterMenu}
        setOpenFilterMenu={setOpenFilterMenu}
      />
    </main>
  );
}
