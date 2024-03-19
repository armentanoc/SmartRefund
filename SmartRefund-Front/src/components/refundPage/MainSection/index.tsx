import { AllReceiptDataType } from "@/types/refund/EventSourceType";
import { getCookie } from "@/utils/helpers/manageCookies";
import { Add, RefreshRounded, Search, Tune } from "@mui/icons-material";
import { Box, Button, InputAdornment, TextField } from "@mui/material";
import React from "react";
import { AddReciptModal } from "../../Modals/AddReciptModal";
import { RefundCard } from "../../RefundCard";

interface MainSectionProps {
  receiptsData: AllReceiptDataType | undefined;
  fetchReceiptsData: () => void;
  openFilterMenu: boolean;
  setOpenFilterMenu: React.Dispatch<React.SetStateAction<boolean>>;
}

export const MainSection = ({
  receiptsData,
  fetchReceiptsData,
  openFilterMenu,
  setOpenFilterMenu,
}: MainSectionProps) => {
  const [openModal, setOpenModal] = React.useState(false);
  const [userType, setUserType] = React.useState<string | undefined>();
  const [filteredReceiptsData, setFilteredReceiptsData] = React.useState<
    AllReceiptDataType | undefined
  >(receiptsData);

  React.useEffect(() => {
    const fetchData = async () => {
      try {
        const user_type = await getCookie();
        setUserType(user_type.userType?.value);
      } catch (error) {
        console.error(error);
      }
    };

    fetchData();
  }, []);

  React.useEffect(() => {
    setFilteredReceiptsData(receiptsData);
  }, [receiptsData]);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const searchValue = event.target.value;
    if (searchValue !== "") {
      setFilteredReceiptsData(
        receiptsData?.filter((receiptData) =>
          receiptData.internalReceipt.uniqueHash
            .toLowerCase()
            .includes(searchValue.toLowerCase()),
        ),
      );
    } else {
      setFilteredReceiptsData(receiptsData);
    }
  };

  return (
    <>
      <Box
        component="section"
        className="flex h-full flex-col justify-start gap-4 overflow-x-hidden bg-slate-50 p-4 md:p-6"
        bgcolor="primary"
        style={{ gridArea: "mainSection" }}
      >
        <div className="sticky flex items-center justify-end gap-4">
          <div
            className="flex cursor-pointer items-center justify-center"
            onClick={() => window.location.reload()}
          >
            <RefreshRounded color="primary" fontSize="large" />
          </div>
          <TextField
            size="small"
            className="w-80"
            variant="outlined"
            placeholder="Pesquisar hash da nota fiscal"
            type="search"
            onChange={handleChange}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <Search />
                </InputAdornment>
              ),
            }}
          />

          {userType === process.env.NEXT_PUBLIC_API_TOKEN_EMPLOYEE && (
            <div className="hidden md:flex">
              <Button variant="contained" onClick={() => setOpenModal(true)}>
                <Add />
                <span className="hidden md:inline">Adicionar nota fiscal</span>
              </Button>
            </div>
          )}
        </div>
        <div className="flex items-center justify-end gap-4 md:hidden">
          <Button onClick={() => setOpenFilterMenu(!openFilterMenu)}>
            <Tune />
            Filtrar
          </Button>
          {userType === process.env.NEXT_PUBLIC_API_TOKEN_EMPLOYEE && (
            <Button
              variant="contained"
              onClick={() => setOpenModal(true)}
              className="md:hidden"
            >
              <Add />
              <span className="inline md:hidden">Nota fiscal</span>
            </Button>
          )}
        </div>
        <div className="flex flex-wrap justify-center gap-4">
          {filteredReceiptsData?.map((receiptData, index) => (
            <RefundCard key={index} cardInfo={receiptData} />
          ))}
        </div>
      </Box>
      <AddReciptModal
        open={openModal}
        setIsOpen={setOpenModal}
        fetchReceiptsData={fetchReceiptsData}
      />
    </>
  );
};
