import { Typography } from "@mui/material";
import Image from "next/image";

export const BannerSide = () => {
  return (
    <div className="hidden bg-green-50 w-full h-full md:flex flex-col items-center justify-center gap-4">
      <Image src="/banner.svg" alt="logo" width={300} height={300} />
      <span className="w-24 h-4 bg-[#bfe5cf] rounded-full" />
      <div className="flex flex-col items-center justify-center gap-1 mt-4">
        <Typography align="center" variant="body1">
          Deixe a inteligência artificial cuidar das notas fiscais para você.
        </Typography>
        <Typography align="center" variant="body1">
          <b>Processo de reembolso fácil e eficiente!</b>
        </Typography>
      </div>
    </div>
  );
};
