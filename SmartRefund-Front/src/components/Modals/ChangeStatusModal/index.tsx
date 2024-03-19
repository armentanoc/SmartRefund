import { StatusServices } from "@/services/recipt/status_service";
import { ChangeStatusType } from "@/types/refund/ChangeStatusType";
import { filters, optionsType } from "@/utils/constants/filters";
import { yupResolver } from "@hookform/resolvers/yup";
import {
  Box,
  Button,
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Modal,
  Select,
  Typography,
} from "@mui/material";
import React from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as yup from "yup";

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  bgcolor: "background.paper",
  boxShadow: 24,
  px: 4,
  py: 3,
  borderRadius: 4,
};

type changeStatusModalProps = {
  open: boolean;
  setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
  uniqueHash: string;
  fetchReceiptData: () => Promise<void>;
};

const changeStatusModalSchema = yup.object().shape({
  uniqueHash: yup.string().required(),
  newStatus: yup
    .number()
    .typeError("O status é obrigatório")
    .required("O status é obrigatório")
    .min(1, "O status é obrigatório"),
});

export const ChangeStatusModal = ({
  open,
  setIsOpen,
  uniqueHash,
  fetchReceiptData,
}: changeStatusModalProps) => {
  const {
    register,
    handleSubmit,
    watch,
    setValue,
    formState: { errors },
  } = useForm<ChangeStatusType>({
    resolver: yupResolver(changeStatusModalSchema),
  });

  const onSubmit: SubmitHandler<ChangeStatusType> = async (data) => {
    try {
      const changeStatusResponse = await StatusServices.changeStatus(data);
      fetchReceiptData();
      toast.success(
        `Status da nota fiscal ${changeStatusResponse.data.uniqueHash} atualizado com sucesso para 
        ${
          filters.optionsStatusRefund.options.find(
            (option) =>
              option.value === changeStatusResponse.data.status.toString(),
          )?.label
        }!`,
      );
      setIsOpen(false);
    } catch (error) {
      console.error("error:", error);
      toast.error(`${error}`);
    }
  };

  React.useEffect(() => {
    setValue("uniqueHash", uniqueHash);
  }, []);

  return (
    <Modal
      open={open}
      onClose={() => setIsOpen(false)}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
    >
      <Box sx={{ ...style }} className="w-full md:w-[600px]">
        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Alterar status da solicitação
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            Selecione o status atual dessa solicitação
          </Typography>

          <FormControl fullWidth error={!!errors.newStatus?.message}>
            <InputLabel id="demo-simple-select-label">Status</InputLabel>
            <Select
              labelId="demo-simple-select-label"
              id="demo-simple-select"
              label="Status"
              {...register("newStatus")}
            >
              {filters.optionsStatusRefund.options.map(
                (option: optionsType) => (
                  <MenuItem key={option.value} value={option.value}>
                    {option.label}
                  </MenuItem>
                ),
              )}
            </Select>
            <FormHelperText error>{errors.newStatus?.message}</FormHelperText>
          </FormControl>

          <Button type="submit" variant="contained">
            Alterar
          </Button>
        </form>
      </Box>
    </Modal>
  );
};
