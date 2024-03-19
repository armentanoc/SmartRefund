import { FetchReceiptsDataOptions } from "@/types/refund/ReciptValidationType";
import { FilterOption, filters, optionsType } from "@/utils/constants/filters";
import { yupResolver } from "@hookform/resolvers/yup";
import { ClearRounded } from "@mui/icons-material";
import {
  Button,
  Checkbox,
  Divider,
  FormControl,
  FormControlLabel,
  FormLabel,
} from "@mui/material";
import React from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import * as yup from "yup";

const asideFilterSchema = yup.object().shape({
  optionsStatusGPT: yup.array().of(yup.number()),
  optionsStatusTranslate: yup.array().of(yup.number()),
  optionsStatusRefund: yup.array().of(yup.number()),
});

interface AsideFilterProps {
  fetchReceiptsData: (options?: FetchReceiptsDataOptions) => Promise<void>;
  openFilterMenu: boolean;
  setOpenFilterMenu: React.Dispatch<React.SetStateAction<boolean>>;
}

export const AsideFilter = ({
  fetchReceiptsData,
  openFilterMenu,
  setOpenFilterMenu,
}: AsideFilterProps) => {
  // const [resetFilter, setResetFilters] = React.useState(false);
  const {
    register,
    handleSubmit,
    watch,
    setValue,
    // reset,
  } = useForm<FetchReceiptsDataOptions>({
    resolver: yupResolver(asideFilterSchema),
  });

  // TODO: descobrir pq a solicitação nao funciona de primeira

  const onSubmit: SubmitHandler<FetchReceiptsDataOptions> = async (data) => {
    await fetchReceiptsData(data);
    setOpenFilterMenu(false);
  };

  // const handleReset = () => {
  //   reset();
  //   setResetFilters(true);
  //   console.log("reset");
  // };

  React.useEffect(() => {
    setValue("optionsStatusGPT", []);
    setValue("optionsStatusRefund", []);
    setValue("optionsStatusTranslate", []);
  }, []);

  return (
    <aside
      className={`fixed ${openFilterMenu ? "flex" : "hidden"} top-14 z-10 h-full w-full flex-col items-center justify-start overflow-y-auto bg-[#e5f4eb] px-8 pb-20 pt-4 md:flex md:w-auto md:justify-center md:py-20`}
      style={{ gridArea: "aside" }}
    >
      <span
        className="w-full text-right md:hidden"
        onClick={() => setOpenFilterMenu(false)}
      >
        <ClearRounded />
      </span>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="flex w-full flex-col gap-4"
      >
        {Object.values(filters).map((filter: FilterOption) => {
          return (
            <React.Fragment key={filter.key}>
              <FormControl size={"small"} variant={"outlined"}>
                <FormLabel component="legend">
                  <b>{filter.label}</b>
                </FormLabel>
                <div className="flex flex-col">
                  {filter.options.map((option: optionsType) => {
                    return (
                      <FormControlLabel
                        key={option.value}
                        control={
                          <Checkbox
                            value={option.value}
                            sx={{ padding: "4px" }}
                            {...register(filter.key)}
                          />
                        }
                        label={option.label}
                      />
                    );
                  })}
                </div>
              </FormControl>
              <Divider />
            </React.Fragment>
          );
        })}

        <div className="flex flex-col gap-2">
          <Button type="submit" variant="contained">
            Aplicar filtros
          </Button>
          {/* <Button type="button" variant="outlined" onClick={handleReset}>
            Limpar filtros
          </Button> */}
        </div>
      </form>
    </aside>
  );
};
