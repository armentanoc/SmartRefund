import { InfoSection } from "@/components/InfoSection";
import { ChangeStatusModal } from "@/components/Modals/ChangeStatusModal";
import { ReceiptDataType } from "@/types/refund/EventSourceType";
import {
  InternalReceiptStatusEnum,
  StatusRefundEnum,
  TranslatedVisionReceiptCategoryEnum,
} from "@/utils/constants/enums";
import { formattedDate } from "@/utils/helpers/formattedDate";
import {
  AcUnit,
  AccountCircle,
  AttachMoney,
  Category,
  DateRange,
  Description,
  Help,
  Info,
} from "@mui/icons-material";
import { Box, Divider, Typography } from "@mui/material";
import Image from "next/image";

type ReceiptInfoProps = {
  receiptData: ReceiptDataType | undefined;
  fetchReceiptData: () => Promise<void>;
  statusModalOpen: boolean;
  setStatusModalOpen: React.Dispatch<React.SetStateAction<boolean>>;
  hash: string;
};

export const ReceiptInfo = ({
  receiptData,
  fetchReceiptData,
  statusModalOpen,
  setStatusModalOpen,
  hash,
}: ReceiptInfoProps) => {
  return (
    <>
      <div className="w-full flex flex-col gap-4 md:p-8 md:flex-row">
        <Box
          component="section"
          className="flex-1 flex flex-col justify-start items-center"
        >
          {receiptData?.internalReceipt.image && (
            <Image
              src={`data:image/jpeg;base64,${receiptData.internalReceipt.image}`}
              alt="Nota fiscal"
              width={600}
              height={700}
              className="w-full md:w-[400px] h-full object-contain"
            />
          )}
        </Box>

        <Box
          component="section"
          className="flex-1 flex flex-col justify-start gap-8"
        >
          <div className="flex flex-col justify-start gap-2 w-full md:w-2/3 md:min-w-[400px]">
            <Typography variant="subtitle1" color="primary">
              Informações sobre o envio
            </Typography>
            <Divider />
            <InfoSection
              icon={<AcUnit />}
              label="Hash"
              value={receiptData?.internalReceipt.uniqueHash}
            />
            <InfoSection
              icon={<AccountCircle />}
              label="Id do funcionário"
              value={receiptData?.internalReceipt.employeeId}
            />
            <InfoSection
              icon={<DateRange />}
              label="Data de criação"
              value={formattedDate(receiptData?.internalReceipt.creationDate)}
            />
            {receiptData?.internalReceipt.status && (
              <InfoSection
                icon={<Info />}
                label="Status"
                chip={{
                  label:
                    InternalReceiptStatusEnum[
                      receiptData?.internalReceipt
                        .status as keyof typeof InternalReceiptStatusEnum
                    ].label,
                  color:
                    InternalReceiptStatusEnum[
                      receiptData?.internalReceipt
                        .status as keyof typeof InternalReceiptStatusEnum
                    ].color,
                }}
              />
            )}
          </div>

          <div className="flex flex-col justify-start gap-2 w-full md:w-2/3 md:min-w-[400px]">
            <Typography variant="subtitle1" color="primary">
              Resultado da solicitação
            </Typography>
            <Divider />
            {receiptData?.translatedVision.category !== undefined && (
              <InfoSection
                icon={<Category />}
                label="Categoria"
                chip={{
                  label:
                    TranslatedVisionReceiptCategoryEnum[
                      receiptData.translatedVision
                        .category as keyof typeof TranslatedVisionReceiptCategoryEnum
                    ].label,
                  color:
                    TranslatedVisionReceiptCategoryEnum[
                      receiptData.translatedVision
                        .category as keyof typeof TranslatedVisionReceiptCategoryEnum
                    ].color,
                }}
              />
            )}
            <InfoSection
              icon={<AttachMoney />}
              label="Total"
              value={`R$ ${receiptData?.translatedVision.total.toFixed(2)}`}
            />
            {receiptData?.translatedVision.status !== undefined && (
              <InfoSection
                icon={<Info />}
                label="Status da solicitação"
                chip={{
                  label:
                    StatusRefundEnum[
                      receiptData.translatedVision
                        .status as keyof typeof StatusRefundEnum
                    ].label,
                  color:
                    StatusRefundEnum[
                      receiptData.translatedVision
                        .status as keyof typeof StatusRefundEnum
                    ].color,
                }}
              />
            )}
            <InfoSection
              icon={<Description />}
              label="Descrição"
              value={receiptData?.translatedVision.description}
            />
          </div>

          <div className="flex flex-col justify-start gap-2 w-full md:w-2/3 md:min-w-[400px]">
            <Typography variant="subtitle1" color="primary">
              Resposta do ChatGPT Vision
            </Typography>
            <Divider />
            <InfoSection
              icon={<Help />}
              label="É uma nota fiscal"
              value={receiptData?.rawVision.isReceipt}
            />
            <InfoSection
              icon={<Category />}
              label="Categoria"
              value={receiptData?.rawVision.category}
            />
            <InfoSection
              icon={<AttachMoney />}
              label="Total"
              value={receiptData?.rawVision.total}
            />
            <InfoSection
              icon={<Description />}
              label="Descrição"
              value={receiptData?.rawVision.description}
            />
          </div>
        </Box>
      </div>
      <ChangeStatusModal
        open={statusModalOpen}
        setIsOpen={setStatusModalOpen}
        uniqueHash={hash}
        fetchReceiptData={fetchReceiptData}
      />
    </>
  );
};
