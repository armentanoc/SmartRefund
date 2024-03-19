import { EntryServices } from "@/services/recipt/entry_services";
import { ReceipEntryType } from "@/types/refund/ReciptEntryType";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import { Box, Button, Modal, Typography, styled } from "@mui/material";
import Image from "next/image";
import { useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { toast } from "react-toastify";

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

type AddReciptModalProps = {
  open: boolean;
  setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
  fetchReceiptsData: () => void;
};

const VisuallyHiddenInput = styled("input")({
  clip: "rect(0 0 0 0)",
  clipPath: "inset(50%)",
  height: 1,
  overflow: "hidden",
  position: "absolute",
  bottom: 0,
  left: 0,
  whiteSpace: "nowrap",
  width: 1,
});

export const AddReciptModal = ({
  open,
  setIsOpen,
  fetchReceiptsData,
}: AddReciptModalProps) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ReceipEntryType>();
  const [selectedFile, setSelectedFile] = useState<File>({} as File);
  const [previewImage, setPreviewImage] = useState<string>("");

  const onSubmit: SubmitHandler<ReceipEntryType> = async (data) => {
    try {
      const receipEntryResponse = await EntryServices.sendReceipt({
        file: selectedFile,
      });
      toast.success(
        `Nota fiscal enviada com sucesso, o hash para acompanhar é: ${receipEntryResponse.data.uniqueHash}`,
      );
      fetchReceiptsData();
      handleCloseModal();
    } catch (error) {
      console.error("error:", error);
      toast.error(`${error}`);
    }
  };

  const handleCloseModal = () => {
    setIsOpen(false);
    setPreviewImage("");
    setSelectedFile({} as File);
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (files && files.length > 0) {
      setSelectedFile(files[0]);
      setPreviewImage(URL.createObjectURL(files[0]));
    } else {
      setSelectedFile({} as File);
      setPreviewImage("");
    }
  };

  return (
    <Modal
      open={open}
      onClose={handleCloseModal}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
    >
      <Box sx={{ ...style }} className="w-full md:w-[600px]">
        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Adicionar nota fiscal
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            Selecione a nota fiscal que deseja enviar
          </Typography>

          <Button
            component="label"
            role={undefined}
            variant="contained"
            color="secondary"
            tabIndex={-1}
            startIcon={<CloudUploadIcon />}
          >
            Upload imagem
            <VisuallyHiddenInput
              {...register("file")}
              type="file"
              accept="image/png, image/jpg, image/jpeg"
              required
              onChange={handleFileChange}
            />
          </Button>
          <Typography variant="body2" className="pb-4">
            {selectedFile.name}
          </Typography>
          {previewImage && (
            <div className="text-center">
              <Image
                src={previewImage}
                alt="Preview"
                width={600}
                height={700}
                className="w-full h-[200px] rounded-md object-cover object-top"
              />
            </div>
          )}

          <Button type="submit" variant="contained">
            Adicionar
          </Button>
        </form>
      </Box>
    </Modal>
  );
};
