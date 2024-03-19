import { ReceiptDataType } from "@/types/refund/EventSourceType";
import { APP_ROUTES } from "@/utils/constants/app-routes";
import {
  InternalReceiptStatusEnum,
  StatusRefundEnum,
} from "@/utils/constants/enums";
import { formattedDate } from "@/utils/helpers/formattedDate";
import { Card, CardContent, CardMedia, Chip, Typography } from "@mui/material";
import { useRouter } from "next/navigation";

interface RefundCardProps {
  cardInfo: ReceiptDataType;
}

export const RefundCard = ({ cardInfo }: RefundCardProps) => {
  const { push } = useRouter();
  return (
    <Card
      sx={{ maxWidth: 300, width: "100%" }}
      className="cursor-pointer"
      onClick={() =>
        push(
          `${APP_ROUTES.private.refund}/${cardInfo.internalReceipt.uniqueHash}`,
        )
      }
    >
      <CardMedia
        component="img"
        height="194"
        image={`data:image/jpeg;base64,${cardInfo.internalReceipt.image}`}
        alt="Nota fiscal"
      />
      <CardContent>
        <Typography variant="body2">
          <b>Hash:</b> {cardInfo.internalReceipt.uniqueHash}
        </Typography>
        <Typography variant="body2">
          <b>Data de criação:</b>{" "}
          {formattedDate(cardInfo.internalReceipt.creationDate)}
        </Typography>
        <div className="flex justify-end gap-2 pt-2">
          {cardInfo.internalReceipt.status !== 0 && (
            <Chip
              label={
                InternalReceiptStatusEnum[
                  cardInfo.internalReceipt
                    .status as keyof typeof InternalReceiptStatusEnum
                ].label
              }
              color={
                InternalReceiptStatusEnum[
                  cardInfo.internalReceipt
                    .status as keyof typeof InternalReceiptStatusEnum
                ].color
              }
            />
          )}

          {cardInfo.translatedVision.status !== 0 && (
            <Chip
              label={
                StatusRefundEnum[
                  cardInfo.translatedVision
                    .status as keyof typeof StatusRefundEnum
                ].label
              }
              color={
                StatusRefundEnum[
                  cardInfo.translatedVision
                    .status as keyof typeof StatusRefundEnum
                ].color
              }
            />
          )}
        </div>
      </CardContent>
    </Card>
  );
};
